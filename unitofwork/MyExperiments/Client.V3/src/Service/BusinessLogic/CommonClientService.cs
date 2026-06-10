using TXC.Common.Data;
using TXC.Common.MessageContract;
using TXC.Common.MessageContract.Client;
using TXC.Proto.Credit;

namespace Service.BusinessLogic
{
    public interface ICommonClientService
    {
        Task<bool> SendCreateMessageAsync(int TenantId, string queueNameConfig, ClientMessageV1 message);
        Task<bool> SendUpdateMessageAsync(int TenantId, string queueNameConfig, ClientMessageV1 message);
        Task<TXC.Proto.Credit.ProtoBaseResponse> CreateClientWalletAsync(CreateClientWalletRequest createClientWalletRequest);
        Task<bool> SendCreateBXPMessageAsync(int TenantId, string queueNameConfig, ClientMessageV1 message);
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

        public async Task<bool> SendCreateBXPMessageAsync(int TenantId, string queueNameConfig, ClientMessageV1 message)
        {
            try
            {
                return await _txcServiceBusSender.SendMessageAsync(TenantId, queueNameConfig, message, ESBMessageType.Client, (int)ActionType.Create, "TXCBXPClient", 1);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
