using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.MessageContract;
using TXC.Common.MessageContract.Client;
using TXC.Proto.Credit;

namespace Services.Core
{
    public interface ICommonClientService
    {
        Task<bool> SendCreateMessageAsync(int TenantId, string queueNameConfig, ClientMessageV1 message);
        Task<bool> SendUpdateMessageAsync(int TenantId, string queueNameConfig, ClientMessageV1 message);
        Task<TXC.Proto.Credit.ProtoBaseResponse> CreateClientWalletAsync(CreateClientWalletRequest createClientWalletRequest);
    }
    public class CommonClientService : ICommonClientService
    {
        private readonly IDapperOperation _dapperOperation;
        private readonly ITX2ServiceBusSender _txcServiceBusSender;
        private readonly CreditRpc.CreditRpcClient _creditRpcClient;
        public CommonClientService()
        {

        }

        public CommonClientService(
            ITX2ServiceBusSender txcServiceBusSender
            , CreditRpc.CreditRpcClient creditRpcClient)
        {
            _txcServiceBusSender = txcServiceBusSender;
            _creditRpcClient = creditRpcClient;
        }



        public async Task<bool> SendCreateMessageAsync(int TenantId, string queueNameConfig, ClientMessageV1 message)
        {
            try
            {
                return await _txcServiceBusSender.SendMessageAsync(TenantId, queueNameConfig, message, ESBMessageType.Client, (int)ActionType.Create, "TXCClient", 1);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendUpdateMessageAsync(int TenantId, string queueNameConfig, ClientMessageV1 message)
        {
            try
            {
                return await _txcServiceBusSender.SendMessageAsync(TenantId, queueNameConfig, message, ESBMessageType.Client, (int)ActionType.Update, "TXCClient", 1);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<TXC.Proto.Credit.ProtoBaseResponse> CreateClientWalletAsync(CreateClientWalletRequest createClientWalletRequest)
        {
            TXC.Proto.Credit.ProtoBaseResponse protoBaseResponseWallet = await _creditRpcClient.CreateClientWalletAsync(
                createClientWalletRequest, null, null, cancellationToken: default);
            return protoBaseResponseWallet;
        }

        public async Task<Tuple<bool, string>> CheckIfValidAddress(int? CityId, int? StateOrProvinceId, int? CountryId, IDbConnection _dbConnection)
        {
            CommandDefinition commandDefinition;
            Tuple<bool, string> result = null;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CountryId", CountryId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ProvinceId", StateOrProvinceId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CityId", CityId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ErrorCode", 0, DbType.Int32, ParameterDirection.Output);

            commandDefinition = new CommandDefinition("client.sp_sel_address_by_country", commandType: CommandType.StoredProcedure,
                                                    parameters: parameters, cancellationToken: default);

            await _dapperOperation.ProcessSql<ExecuteCommandWithReturn<int>, int>(_dbConnection, commandDefinition);
            int? errorCode = parameters.Get<int?>("@ErrorCode");

            if (!errorCode.HasValue)
            {
                result = new Tuple<bool, string>(true, string.Empty);
                return result;
            }

            if (errorCode == 1)
            {
                result = new Tuple<bool, string>(false, "Invalid country id.");
                return result;
            }
            else if (errorCode == 2)
            {
                result = new Tuple<bool, string>(false, "Invalid province id.");
                return result;
            }
            else if (errorCode == 3)
            {
                result = new Tuple<bool, string>(false, "Invalid city id.");
                return result;
            }
            else
            {
                result = new Tuple<bool, string>(true, string.Empty);
                return result;
            }
        }
    }
}