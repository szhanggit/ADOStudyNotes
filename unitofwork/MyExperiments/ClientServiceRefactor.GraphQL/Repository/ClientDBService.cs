using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IClientDBService
    {
        Task<string> GenerateClientIdentityAsync(int TenantId, IDbConnection _dbConnection);
        Task<int?> InsertClientAsync(Client client, Address address, IDbConnection _dbConnection);
        Task<bool> UpdateClientAsync(Client client, Address address, IDbConnection _dbConnection);
        Task<Tuple<int, List<ClientAddress>>> GetClientAsync(GetClientListModel request, IDbConnection _dbConnection);
        Task<int> CheckClientIdAsync(int ClientId, IDbConnection _dbConnection);
        Task DeleteClientByIdAsync(int ClientId, IDbConnection _dbConnection);
    }
    public class ClientDBService : IClientDBService
    {
        private IClientUnitOfWork _unit;
        public ClientDBService(IClientUnitOfWork clientUnitOfWork)
        {
            _unit = clientUnitOfWork;
        }

        public async Task<string> GenerateClientIdentityAsync(int TenantId, IDbConnection _dbConnection)
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
                //_unit.Dispose();
            }

            NewId = 1000 + NewId;
            _clientCode = $"{TenantId}0000000000000000{NewId}";
            return _clientCode;
        }

        public async Task<int?> InsertClientAsync(Client client, Address address, IDbConnection _dbConnection)
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

            return _clientId;
        }

        public async Task<bool> UpdateClientAsync(Client client, Address address, IDbConnection _dbConnection)
        {
            bool _updateClientSuccess = false;
            bool _updateAddressSuccess = false;
            int _newAddressId = 0;
            try
            {
                _unit.SetConnection(_dbConnection);               
                Client origin = _unit.ClientRepository.GetClientLazily().Where(_ => _.Identity_Code.Equals(client.Identity_Code)).FirstOrDefault();
                if (origin == null)
                {
                    return false;
                }
                client.Client_Id = origin.Client_Id;
                client.Address_Id = origin.Address_Id;

                await _unit.BeginAsync();
                if (!origin.Address_Id.HasValue && address != null) //No existing address, and need to create new address.
                {
                    //Create new address
                    _newAddressId = await _unit.AddressRepository.AddAsync(address, _unit.Transaction);
                    client.Address_Id = _newAddressId;                    
                }
                else if (!origin.Address_Id.HasValue && address == null) //No existing address, and do not need to create new address.
                {
                    //Do nothing to address
                }
                else if (origin.Address_Id.HasValue && address != null) //Has existing address, and need to update old address.
                {
                    //Update address info
                    address.Address_Id = origin.Address_Id ?? 0;
                    _updateAddressSuccess = await _unit.AddressRepository.UpdateAsync(address, _unit.Transaction);
                }
                else if (origin.Address_Id.HasValue && address == null) //Has existing address, and do not need to update old address.
                {
                    //Do nothing
                }
                else
                {
                    //Do nothing
                }

                _updateClientSuccess = await _unit.ClientRepository.UpdateAsync(client, _unit.Transaction);
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

                    if (query != null)
                    {
                        int _skipNum = (request.RowCount ?? 0) * ((request.PageNumber ?? 0) - 1);
                        var _list = query.ToList().Skip(_skipNum).Take(request.RowCount ?? 0);
                        int _returnNum = (_list == null) ? 0 : _list.Count();
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
            }
            catch (Exception)
            {
                throw;
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
                //_unit.Dispose();
            }

            if (_client != null)
                return 1;
            else
                return 0;
        }

        public async Task DeleteClientByIdAsync(int ClientId, IDbConnection _dbConnection)
        {
            Client client = null;
            Address address = null;
            try 
            {
                _unit.SetConnection(_dbConnection);               
                client = await _unit.ClientRepository.GetAsync(ClientId);
                if (client == null)
                {
                    return;
                }

                if (client.Address_Id.HasValue)
                {
                    address = await _unit.AddressRepository.GetAsync(client.Address_Id.Value);
                    await _unit.BeginAsync();
                    await _unit.AddressRepository.RemoveAsync(address, _unit.Transaction);
                }
                else
                {
                    await _unit.BeginAsync();
                }

                await _unit.ClientRepository.RemoveAsync(client, _unit.Transaction);
                await _unit.CommitAsync();
            }
            catch (Exception) 
            {
                _unit.Rollback();                
            } 
            finally 
            {
                _unit.Dispose();
            }
        }
    }
}
