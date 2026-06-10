using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.MessageContract.Client;
using TXC.Proto.Client;

namespace Services.Utility
{
    public interface IObjectConvertingService
    {
        ClientMessageV1 ConvertUpdateClientRequestToClientMessageV1(UpdateClientRequest _updateClientRequest);
        ClientMessageV1 ConvertCreateClientRequestToClientMessageV1(CreateClientRequest _createClientRequest, int? ClientId, string ClientCode);
    }
    public class ObjectConvertingService : IObjectConvertingService
    {
        private readonly IMapper _mapper;
        public ObjectConvertingService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ClientMessageV1 ConvertUpdateClientRequestToClientMessageV1(UpdateClientRequest _updateClientRequest)
        {
            ClientMessageV1 _result = new ClientMessageV1();
            if (_updateClientRequest == null)
            {
                return _result;
            }
            else
            {
                _result = _mapper.Map<ClientMessageV1>(_updateClientRequest);
                return _result;
            }
        }

        public ClientMessageV1 ConvertCreateClientRequestToClientMessageV1(CreateClientRequest _createClientRequest, int? ClientId, string ClientCode)
        {
            ClientMessageV1 _result = new ClientMessageV1();
            if (_createClientRequest == null || !ClientId.HasValue)
            {
                return _result;
            }
            else
            {
                _result = _mapper.Map<ClientMessageV1>(_createClientRequest);
                _result.ClientId = ClientId ?? 0;
                _result.IdentityCode = ClientCode;
                return _result;
            }
        }
    }
}
