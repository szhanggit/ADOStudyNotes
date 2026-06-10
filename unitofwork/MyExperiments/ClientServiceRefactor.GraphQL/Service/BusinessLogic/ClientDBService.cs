using Domain.Entities;
using Domain.Models;
using Repository;
using System.Data;

namespace Service.BusinessLogic
{
    public interface IClientDBService
    {
        Task<string> GenerateClientIdentityAsync(int TenantId, IDbConnection _dbConnection);
        Task<int?> InsertClientAsync(Domain.Entities.Client client, Address address, IDbConnection _dbConnection);
        Task<bool> UpdateClientAsync(ClientModel _originalClient, Domain.Entities.Client client, Address address, IDbConnection _dbConnection);
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

        public async Task<int?> InsertClientAsync(Domain.Entities.Client client, Address address, IDbConnection _dbConnection)
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
            catch (Exception ex)
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

        public async Task<bool> UpdateClientAsync(ClientModel origin, Domain.Entities.Client client, Address address, IDbConnection _dbConnection)
        {
            bool _updateClientSuccess = false;
            bool _updateAddressSuccess = false;
            int _newAddressId = 0;
            try
            {
                _unit.SetConnection(_dbConnection);
                if (origin == null)
                {
                    return false;
                }
                client.Client_Id = origin.Id;
                client.Address_Id = origin.AddressId;

                await _unit.BeginAsync();
                if (!origin.AddressId.HasValue && address != null) //No existing address, and need to create new address.
                {
                    //Create new address
                    _newAddressId = await _unit.AddressRepository.AddAsync(address, _unit.Transaction);
                    client.Address_Id = _newAddressId;
                }
                else if (!origin.AddressId.HasValue && address == null) //No existing address, and do not need to create new address.
                {
                    //Do nothing to address
                }
                else if (origin.AddressId.HasValue && address != null) //Has existing address, and need to update old address.
                {
                    //Update address info
                    address.Address_Id = origin.AddressId ?? 0;
                    _updateAddressSuccess = await _unit.AddressRepository.UpdateAsync(address, _unit.Transaction);
                }
                else if (origin.AddressId.HasValue && address == null) //Has existing address, and do not need to update old address.
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
            catch (Exception ex)
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

        public async Task DeleteClientByIdAsync(int ClientId, IDbConnection _dbConnection)
        {
            Domain.Entities.Client client = null;
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
