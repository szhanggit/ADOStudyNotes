using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IClientOperation
    {
        Task<string> GenerateClientIdentityAsync(GenerateClientIdentityCodeModel request, IDbConnection _dbConnection);
        Task<int?> InsertClientAsync(Client client, GenerateClientIdentityCodeModel request, IDbConnection _dbConnection);
        Task<int?> UpdateClientAsync(Client client, IDbConnection _dbConnection);
        Task<Tuple<int, List<Client>>> GetClientAsync(GetClientListModel request, IDbConnection _dbConnection);
        Task<Tuple<bool, string>> CheckIfValidAddressAsync(int? CityId, int? StateOrProvinceId, int? CountryId, IDbConnection _dbConnection);
        Task<int> CheckClientIdAsync(int ClientId, IDbConnection _dbConnection);
        Task DeleteClientByIdAsync(int ClientId, IDbConnection _dbConnection);
        Task<Tuple<ResponseModel, int?>> CreateBXPClientAsync(Client client, string securityKey, GenerateClientIdentityCodeModel request, IDbConnection _dbConnection);
    }
    public class ClientOperation : IClientOperation
    {
        public ClientOperation()
        {

        }

        public async Task<string> GenerateClientIdentityAsync(GenerateClientIdentityCodeModel request, IDbConnection _dbConnection)
        {
            string _clientCode = string.Empty;
            using (OperationSession oSession = new OperationSession(_dbConnection))
            {
                try
                {
                    _clientCode = await oSession.clientBasicInfoRepository.GenerateClientIdentityAsync(request);
                    oSession.UnitOfWork.CommitAsync();
                }
                catch (Exception)
                {
                    oSession.UnitOfWork.RollbackAsync();
                }

                return _clientCode;
            }
        }

        public async Task<int?> InsertClientAsync(Client client, GenerateClientIdentityCodeModel request, IDbConnection _dbConnection)
        {
            int? _addressId = 0;
            int? _clientId = 0;
            string _clientCode = string.Empty;
            using (OperationSession oSession = new OperationSession(_dbConnection))
            {
                try
                {
                    _clientCode = await oSession.clientBasicInfoRepository.GenerateClientIdentityAsync(request);
                    if (client.CountryId.HasValue)
                    {
                        Address _address = new Address
                        {
                            CountryId = client.CountryId,
                            CityId = client.CityId,
                            StateOrProvinceId = client.StateOrProvinceId,
                            AddressStatus = client.AddressStatus,
                            DetailAddressLine = client.DetailAddressLine,
                            District = client.District,
                            Postcode = client.Postcode,
                            Latitude = client.Latitude,
                            Longitude = client.Longitude,
                        };
                        _addressId = await oSession.addressRepository.CreateAddressAsync(_address);
                        client.AddressId = _addressId;
                    }
                    client.IdentityCode = _clientCode;
                    _clientId = await oSession.clientBasicInfoRepository.CreateClientAsync(client);
                    await oSession.UnitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    await oSession.UnitOfWork.RollbackAsync();
                }
                return _clientId;
            }
        }

        public async Task<int?> UpdateClientAsync(Client client, IDbConnection _dbConnection)
        {
            Address _address = null;
            int? _addressId = 0;
            if (client.CountryId.HasValue)
            {
                _address = new Address
                {
                    CountryId = client.CountryId,
                    CityId = client.CityId,
                    StateOrProvinceId = client.StateOrProvinceId,
                    AddressStatus = client.AddressStatus,
                    DetailAddressLine = client.DetailAddressLine,
                    District = client.District,
                    Postcode = client.Postcode,
                    Latitude = client.Latitude,
                    Longitude = client.Longitude,
                };
            }

            using (OperationSession oSession = new OperationSession(_dbConnection))
            {
                try
                {
                    _addressId = await oSession.clientBasicInfoRepository.UpdateClientAsync(client);
                    if (!_addressId.HasValue && client.CountryId.HasValue) //No existing address, and need to create new address.
                    {
                        await oSession.addressRepository.CreateAddressAsync(_address);
                    }
                    else if (!_addressId.HasValue && !client.CountryId.HasValue) //No existing address, and do not need to create new address.
                    {
                        //Do nothing
                    }
                    else if (_addressId.HasValue && client.CountryId.HasValue) //Has existing address, and need to update old address.
                    {
                        await oSession.addressRepository.UpdateAddressAsync(_address);
                    }
                    else if (_addressId.HasValue && !client.CountryId.HasValue) //Has existing address, and do not need to update old address.
                    {
                        //Do nothing
                    }
                    else
                    {
                        //Do nothing
                    }

                    await oSession.UnitOfWork.CommitAsync();
                }
                catch (Exception)
                {
                    await oSession.UnitOfWork.RollbackAsync();
                }
                return 1;
            }
        }

        public async Task<Tuple<int, List<Client>>> GetClientAsync(GetClientListModel request, IDbConnection _dbConnection)
        {
            Tuple<int, List<Client>> _clientInfo = null;
            using (OperationSession oSession = new OperationSession(_dbConnection))
            {
                try
                {
                    if (request.ClientId.HasValue)
                    {
                        List<Client> _clientList = new List<Client>();
                        Client _client = await oSession.clientBasicInfoRepository.GetClientByIdAsync(request.ClientId.Value);
                        _clientList.Add(_client);
                        _clientInfo = new Tuple<int, List<Client>>(1, _clientList);
                    }
                    else
                    {
                        _clientInfo = await oSession.clientBasicInfoRepository.GetClientBySearchKeyAsync(request.SearchKeyWord, request.RowCount, request.PageNumber);
                    }                    
                    await oSession.UnitOfWork.CommitAsync();
                }
                catch (Exception)
                {
                    await oSession.UnitOfWork.RollbackAsync();
                }

                if(_clientInfo != null)
                    return _clientInfo;
                else
                    return Tuple.Create(0, new List<Client>());
            }
        }

        public async Task<Tuple<bool, string>> CheckIfValidAddressAsync(int? CityId, int? StateOrProvinceId, int? CountryId, IDbConnection _dbConnection)
        {
            List<ProvinceCityPair> _provinceCityPairsList = null;
            using (OperationSession oSession = new OperationSession(_dbConnection))
            {
                _provinceCityPairsList = await oSession.dictionaryRepository.GetProvinceCityPairAsync(CountryId ?? 0);
                await oSession.UnitOfWork.CommitAsync();

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
        }

        public async Task<int> CheckClientIdAsync(int ClientId, IDbConnection _dbConnection)
        {
            Client _client = null;
            using (OperationSession oSession = new OperationSession(_dbConnection))
            {
                try
                {
                    _client = await oSession.clientBasicInfoRepository.GetClientByIdAsync(ClientId);
                    await oSession.UnitOfWork.CommitAsync();
                }
                catch (Exception)
                {
                    await oSession.UnitOfWork.RollbackAsync();
                }
            }

            if (_client != null)
                return 1;
            else
                return 0;
        }

        public async Task DeleteClientByIdAsync(int ClientId, IDbConnection _dbConnection)
        {
            using (OperationSession oSession = new OperationSession(_dbConnection))
            {
                try
                {
                    await oSession.clientBasicInfoRepository.DeleteClientById(ClientId);
                    await oSession.UnitOfWork.CommitAsync();
                }
                catch (Exception)
                {
                    await oSession.UnitOfWork.RollbackAsync();
                }
            }
        }

        public async Task<Tuple<ResponseModel, int?>> CreateBXPClientAsync(Client client, string securityKey, GenerateClientIdentityCodeModel request, IDbConnection _dbConnection)
        {
            string _clientCode = string.Empty;
            int? _clientId = 0;
            Address _address = null;
            using (OperationSession oSession = new OperationSession(_dbConnection))
            {
                try
                {
                    _clientCode = await oSession.clientBasicInfoRepository.GenerateClientIdentityAsync(request);
                    client.IdentityCode = _clientCode;
                    client.SecurityKey = securityKey;
                    var _searchingResult = await oSession.clientBasicInfoRepository.GetClientBySearchKeyAsync(client.ClientName, 10, 1);
                    if (_searchingResult.Item1 >= 1)
                    {
                        _clientId = _searchingResult.Item2.FirstOrDefault().ClientId;
                        await oSession.UnitOfWork.CommitAsync();
                        return Tuple.Create(new ResponseModel() { Success = false, Message = "BXP client already exists." }, _clientId);
                    }

                    if (client.CountryId.HasValue)
                    {
                        _address = new Address
                        {
                            CountryId = client.CountryId,
                            CityId = client.CityId,
                            StateOrProvinceId = client.StateOrProvinceId,
                            AddressStatus = client.AddressStatus,
                            DetailAddressLine = client.DetailAddressLine,
                            District = client.District,
                            Postcode = client.Postcode,
                            Latitude = client.Latitude,
                            Longitude = client.Longitude,
                        };
                        client.AddressId = await oSession.addressRepository.CreateAddressAsync(_address);
                    }

                    _clientId = await oSession.clientBasicInfoRepository.CreateBXPClientAsync(client);
                    await oSession.UnitOfWork.CommitAsync();
                    return Tuple.Create(new ResponseModel() { Success = true, Message = "Success" }, _clientId);
                }
                catch (Exception)
                {
                    await oSession.UnitOfWork.RollbackAsync();
                    return Tuple.Create(new ResponseModel() { Success = false, Message = "Failed to create new BXP client" }, _clientId);
                }               
            }
        }
    }
}
