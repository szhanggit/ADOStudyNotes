using Domain.Entities;
using Domain.Models;
using System.Data;

namespace Repository.Dapper
{
    public interface IClientOperation
    {
        Task<string> GenerateClientIdentityAsync(GenerateClientIdentityCodeModel request, IDbConnection _dbConnection);
        Task<Tuple<int?, string>> InsertClientAsync(Client client, GenerateClientIdentityCodeModel request, IDbConnection _dbConnection);
        Task<int?> UpdateClientAsync(Client client, IDbConnection _dbConnection);
        Task<Tuple<int, List<Client>>> GetClientAsync(GetClientListModel request, IDbConnection _dbConnection);
        Task<Tuple<bool, string>> CheckIfValidAddressAsync(int? CityId, int? StateOrProvinceId, int? CountryId, IDbConnection _dbConnection);
        Task<int> CheckClientIdAsync(int ClientId, IDbConnection _dbConnection);
        Task DeleteClientByIdAsync(int ClientId, IDbConnection _dbConnection);
        Task<Tuple<ResponseModel, int?, string>> CreateBXPClientAsync(Client client, string securityKey, GenerateClientIdentityCodeModel request, IDbConnection _dbConnection);
    }
    public class ClientOperation : IClientOperation
    {
        private readonly IOperationSession _operationSession;
        public ClientOperation(IOperationSession operationSession)
        {
            _operationSession = operationSession;
        }

        public async Task<string> GenerateClientIdentityAsync(GenerateClientIdentityCodeModel request, IDbConnection _dbConnection)
        {
            string _clientCode = string.Empty;

            try
            {
                _operationSession.SetConnection(_dbConnection);
                _clientCode = await _operationSession.clientRepository.GenerateClientIdentityAsync(request);
                _operationSession.UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                _operationSession.UnitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                _operationSession.Dispose();
            }

            return _clientCode;

        }

        public async Task<Tuple<int?,string>> InsertClientAsync(Client client, GenerateClientIdentityCodeModel request, IDbConnection _dbConnection)
        {
            int? _addressId = 0;
            int? _clientId = 0;
            string _clientCode = string.Empty;

            try
            {
                _operationSession.SetConnection(_dbConnection);
                _clientCode = await _operationSession.clientRepository.GenerateClientIdentityAsync(request);
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
                    _addressId = await _operationSession.addressRepository.CreateAddressAsync(_address);
                    client.AddressId = _addressId;
                }
                client.IdentityCode = _clientCode;
                _clientId = await _operationSession.clientRepository.CreateClientAsync(client);
                await _operationSession.UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _operationSession.UnitOfWork.RollbackAsync();
            }
            finally
            {
                _operationSession.Dispose();
            }

            return Tuple.Create(_clientId, _clientCode);

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

            try
            {
                _operationSession.SetConnection(_dbConnection);
                _addressId = await _operationSession.clientRepository.UpdateClientAsync(client);
                if (!_addressId.HasValue && client.CountryId.HasValue) //No existing address, and need to create new address.
                {
                    await _operationSession.addressRepository.CreateAddressAsync(_address);
                }
                else if (!_addressId.HasValue && !client.CountryId.HasValue) //No existing address, and do not need to create new address.
                {
                    //Do nothing
                }
                else if (_addressId.HasValue && client.CountryId.HasValue) //Has existing address, and need to update old address.
                {
                    await _operationSession.addressRepository.UpdateAddressAsync(_address);
                }
                else if (_addressId.HasValue && !client.CountryId.HasValue) //Has existing address, and do not need to update old address.
                {
                    //Do nothing
                }
                else
                {
                    //Do nothing
                }

                await _operationSession.UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _operationSession.UnitOfWork.RollbackAsync();
            }
            finally
            {
                _operationSession.Dispose();
            }

            return 1;
        }

        public async Task<Tuple<int, List<Client>>> GetClientAsync(GetClientListModel request, IDbConnection _dbConnection)
        {
            Tuple<int, List<Client>> _clientInfo = null;

            try
            {
                _operationSession.SetConnection(_dbConnection);
                if (request.ClientId.HasValue)
                {
                    List<Client> _clientList = new List<Client>();
                    Client _client = await _operationSession.clientRepository.GetClientByIdAsync(request.ClientId.Value);
                    _clientList.Add(_client);
                    _clientInfo = new Tuple<int, List<Client>>(1, _clientList);
                }
                else
                {
                    _clientInfo = await _operationSession.clientRepository.GetClientBySearchKeyAsync(request.SearchKeyWord, request.RowCount, request.PageNumber);
                }
                await _operationSession.UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _operationSession.UnitOfWork.RollbackAsync();
            }
            finally
            {
                _operationSession.Dispose();
            }

            if (_clientInfo != null)
                return _clientInfo;
            else
                return Tuple.Create(0, new List<Client>());

        }

        public async Task<Tuple<bool, string>> CheckIfValidAddressAsync(int? CityId, int? StateOrProvinceId, int? CountryId, IDbConnection _dbConnection)
        {
            List<ProvinceCityPair> _provinceCityPairsList = null;

            _operationSession.SetConnection(_dbConnection);
            try
            {
                _provinceCityPairsList = await _operationSession.dictionaryRepository.GetProvinceCityPairAsync(CountryId ?? 0);
                await _operationSession.UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _operationSession.UnitOfWork.RollbackAsync();
                throw;
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
                _operationSession.SetConnection(_dbConnection);
                _client = await _operationSession.clientRepository.GetClientByIdAsync(ClientId);
                await _operationSession.UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _operationSession.UnitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                _operationSession.Dispose();
            }

            if (_client != null)
                return 1;
            else
                return 0;
        }

        public async Task DeleteClientByIdAsync(int ClientId, IDbConnection _dbConnection)
        {

            try
            {
                _operationSession.SetConnection(_dbConnection);
                await _operationSession.clientRepository.DeleteClientById(ClientId);
                await _operationSession.UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _operationSession.UnitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                _operationSession.Dispose();
            }

        }

        public async Task<Tuple<ResponseModel, int?, string>> CreateBXPClientAsync(Client client, string securityKey, GenerateClientIdentityCodeModel request, IDbConnection _dbConnection)
        {
            string _clientCode = string.Empty;
            int? _clientId = 0;
            Address _address = null;

            try
            {
                _operationSession.SetConnection(_dbConnection);
                _clientCode = await _operationSession.clientRepository.GenerateClientIdentityAsync(request);
                client.IdentityCode = _clientCode;
                client.SecurityKey = securityKey;
                var _searchingResult = await _operationSession.clientRepository.GetClientBySearchKeyAsync(client.ClientName, 10, 1);
                if (_searchingResult.Item1 >= 1)
                {
                    _clientId = _searchingResult.Item2.FirstOrDefault().ClientId;
                    await _operationSession.UnitOfWork.CommitAsync();
                    return Tuple.Create(new ResponseModel() { Success = false, Message = "BXP client already exists." }, _clientId, string.Empty);
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
                    client.AddressId = await _operationSession.addressRepository.CreateAddressAsync(_address);
                }

                _clientId = await _operationSession.clientRepository.CreateBXPClientAsync(client);
                await _operationSession.UnitOfWork.CommitAsync();
                return Tuple.Create(new ResponseModel() { Success = true, Message = "Success" }, _clientId, _clientCode);
            }
            catch (Exception)
            {
                await _operationSession.UnitOfWork.RollbackAsync();
                return Tuple.Create(new ResponseModel() { Success = false, Message = "Failed to create new BXP client" }, _clientId, string.Empty);
            }
            finally
            {
                _operationSession.Dispose();
            }

        }
    }
}
