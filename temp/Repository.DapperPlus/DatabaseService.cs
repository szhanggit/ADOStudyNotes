using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DapperPlus
{
    public interface IDatabaseService
    {
        Task<string> GenerateClientIdentityAsync(IDbConnection _dbConnection);
        Task<Tuple<int?, string>> InsertClientAsync(Client client, Address address, IDbConnection _dbConnection);
        Task<bool> UpdateClientAsync(Client client, Address address, IDbConnection _dbConnection);
        Task<Tuple<int, List<ClientAddress>>> GetClientAsync(GetClientListModel request, IDbConnection _dbConnection);
        Task<Tuple<bool, string>> CheckIfValidAddressAsync(int? CityId, int? StateOrProvinceId, int? CountryId, IDbConnection _dbConnection);
        Task<int> CheckClientIdAsync(int ClientId, IDbConnection _dbConnection);
    }
    public class DatabaseService : IDatabaseService
    {
        private IClientUnitOfWork _unit;
        public DatabaseService(IClientUnitOfWork clientUnitOfWork)
        {
            _unit = clientUnitOfWork;
        }

        public async Task<string> GenerateClientIdentityAsync(IDbConnection _dbConnection)
        {
            int NewId = 0;
            string _clientCode = string.Empty;

            try
            {
                _unit.SetConnection(_dbConnection);
                NewId = await _unit.ClientRepository.GetNewSequenceIdAsync();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _unit.Dispose();
            }

            NewId = 1000 + NewId;
            _clientCode = $"0000000000000000{NewId}";
            return _clientCode;
        }

        public async Task<Tuple<int?, string>> InsertClientAsync(Client client, Address address, IDbConnection _dbConnection)
        {
            int? _addressId = 0;
            int? _clientId = 0;
            try 
            {
                _unit.SetConnection(_dbConnection);
                await _unit.BeginAsync();

                if (address != null)
                {
                    _addressId = await _unit.AddressRepository.AddAsync(address, _unit.Transaction);
                    client.Address_Id = _addressId;
                }

                _clientId = await _unit.ClientRepository.AddAsync(client, _unit.Transaction);
                await _unit.CommitAsync();
            } 
            catch (Exception) 
            {
                await _unit.RollbackAsync();
                throw;
            }
            finally 
            {
                _unit.Dispose();
            }

            return Tuple.Create(_clientId, client.Identity_Code);
        }

        public async Task<bool> UpdateClientAsync(Client client, Address address, IDbConnection _dbConnection)
        {
            bool _updateClientSuccess = false;
            bool _updateAddressSuccess = false;
            int _newAddressId = 0;
            try
            {
                _unit.SetConnection(_dbConnection);
                await _unit.BeginAsync();

                Client origin = _unit.ClientRepository.GetClientLazily().Where(_ => _.Identity_Code.Equals(client.Identity_Code)).FirstOrDefault();
                client.Client_Id = origin.Client_Id;
                client.Address_Id = origin.Address_Id;

                if (!origin.Address_Id.HasValue && address != null) //No existing address, and need to create new address.
                {
                    _newAddressId = await _unit.AddressRepository.AddAsync(address, _unit.Transaction);
                    _updateClientSuccess = await _unit.ClientRepository.UpdateAsync(client, _unit.Transaction);
                }
                else if (!origin.Address_Id.HasValue && address == null) //No existing address, and do not need to create new address.
                {
                    //Do nothing
                }
                else if (origin.Address_Id.HasValue && address != null) //Has existing address, and need to update old address.
                {
                    address.Address_Id = origin.Address_Id??0;
                    _updateAddressSuccess = await _unit.AddressRepository.UpdateAsync(address, _unit.Transaction);
                    _updateClientSuccess = await _unit.ClientRepository.UpdateAsync(client, _unit.Transaction);
                }
                else if (origin.Address_Id.HasValue && address == null) //Has existing address, and do not need to update old address.
                {
                    //Do nothing
                }
                else
                {
                    //Do nothing
                }

                await _unit.CommitAsync();
            }
            catch (Exception)
            {
                await _unit.RollbackAsync();
                throw;
            }
            finally
            {
                _unit.Dispose();
            }

            return true;
        }

        public async Task<Tuple<int, List<ClientAddress>>> GetClientAsync(GetClientListModel request, IDbConnection _dbConnection)
        {
            Tuple<int, List<ClientAddress>> _clientInfo = null;
            ClientAddress clientAddress = new ClientAddress();

            try
            {
                _unit.SetConnection(_dbConnection);
                List<ClientAddress> _clientList = new List<ClientAddress>();
                IEnumerable<Client> _clientCollection = _unit.ClientRepository.GetClientLazily();
                IEnumerable<Address> _addressCollection = _unit.AddressRepository.GetAddressLazily();
                //await _unit.BeginAsync();
                if (request.ClientId.HasValue)
                {
                    var query =
                       from client in _clientCollection
                       join address in _addressCollection on client.Address_Id equals address.Address_Id
                       where client.Client_Id == request.ClientId
                       select new { client, address };

                    var item = query.FirstOrDefault();
                    clientAddress.Client = item.client;
                    clientAddress.Address = item.address;

                    _clientList.Add(clientAddress);


                    _clientInfo = new Tuple<int, List<ClientAddress>>(1, _clientList);
                }
                else
                {
                    var query =
                       from client in _clientCollection
                       join address in _addressCollection on client.Address_Id equals address.Address_Id
                       where client.Client_Name.Contains(request.SearchKeyWord) || client.Identity_Code.Contains(request.SearchKeyWord) || client.Invoice_Register_Number.Contains(request.SearchKeyWord)
                       select new { client, address };

                    if(query != null)
                    {
                        int _skipNum = (request.RowCount ?? 0) * ((request.PageNumber ?? 0) - 1);
                        var _list = query.ToList().Skip(_skipNum).Take(request.RowCount ?? 0);
                        int _returnNum = (_list == null)? 0 : _list.Count();
                        foreach (var item in _list)
                        {
                            ClientAddress tempClientAddress = new ClientAddress();
                            tempClientAddress.Client = item.client;
                            tempClientAddress.Address = item.address;
                            _clientList.Add(tempClientAddress);
                        }

                        _clientInfo = new Tuple<int, List<ClientAddress>>(_returnNum, _clientList);
                    }


                }
                //await _unit.CommitAsync();
            }
            catch (Exception)
            {
                //await _unit.RollbackAsync();
            }
            finally
            {
                _unit.Dispose();
            }

            if (_clientInfo != null)
                return _clientInfo;
            else
                return Tuple.Create(0, new List<ClientAddress>());

        }

        public async Task<Tuple<bool, string>> CheckIfValidAddressAsync(int? CityId, int? StateOrProvinceId, int? CountryId, IDbConnection _dbConnection)
        {
            List<ProvinceCityPair> _provinceCityPairsList = new List<ProvinceCityPair>();

            if (!CountryId.HasValue && CountryId > 0)
            {
                return new Tuple<bool, string>(false, "Invalid country id.");
            }

            try
            {
                _unit.SetConnection(_dbConnection);
                IEnumerable<Dictionary> _countryCollection = _unit.DictionaryRepository.GetDictionaryLazily();
                IEnumerable<Dictionary> _provinceCollection = _unit.DictionaryRepository.GetDictionaryLazily();
                IEnumerable<Dictionary> _cityCollection = _unit.DictionaryRepository.GetDictionaryLazily();

                var _result = from country in _countryCollection
                              join province in _provinceCollection on country.Dictionary_Id equals province.Parent_Id
                              join city in _cityCollection on province.Dictionary_Id equals city.Parent_Id
                              where province.Category.Equals("StateOrProvince") && city.Category.Equals("City") && country.Category.Equals("Country") && country.Dictionary_Id == CountryId
                              select new { province = province.Dictionary_Id, city = city.Dictionary_Id };

                foreach (var item in _result)
                {
                    ProvinceCityPair _pair = new ProvinceCityPair
                    {
                        province = item.province,
                        city = item.city
                    };
                    _provinceCityPairsList.Add(_pair);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _unit.Dispose();
            }

            if (_provinceCityPairsList == null)
            {
                return new Tuple<bool, string>(false, "Invalid country id.");
            }
            else if (_provinceCityPairsList != null && StateOrProvinceId.HasValue)
            {
                ProvinceCityPair p = _provinceCityPairsList.FirstOrDefault(_ => _.province == StateOrProvinceId);
                if (p == null)
                {
                    return new Tuple<bool, string>(false, "Invalid province id.");
                }
                else if (CityId.HasValue)
                {
                    ProvinceCityPair c = _provinceCityPairsList.FirstOrDefault(_ => _.province == StateOrProvinceId && _.city == CityId);
                    if (c == null)
                    {
                        return new Tuple<bool, string>(false, "Invalid city id.");
                    }
                }
                else
                {

                }
            }

            return new Tuple<bool, string>(true, string.Empty);

        }

        public async Task<int> CheckClientIdAsync(int ClientId, IDbConnection _dbConnection)
        {
            Client _client = null;
            try
            {
                _unit.SetConnection(_dbConnection);
                List<ClientAddress> _clientList = new List<ClientAddress>();
                IEnumerable<Client> _clientCollection = _unit.ClientRepository.GetClientLazily();
                _client = _clientCollection.Where(_ => _.Client_Id == ClientId)?.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _unit.Dispose();
            }

            if (_client != null)
                return 1;
            else
                return 0;
        }
    }
}
