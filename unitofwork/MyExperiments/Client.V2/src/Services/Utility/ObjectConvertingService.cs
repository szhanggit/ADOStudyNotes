using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.MessageContract.Client;
using TXC.Proto.Client;
using static Domain.ENUMs.Enums;

namespace Services.Utility
{
    public interface IObjectConvertingService
    {
        ClientMessageV1 ConvertUpdateClientRequestToClientMessageV1(UpdateClientRequest _updateClientRequest);
        ClientMessageV1 ConvertCreateClientRequestToClientMessageV1(CreateClientRequest _createClientRequest, int? ClientId, string ClientCode);
        ClientMessageV1 CreateBXPClientRequestToClientMessageV1(CreateBXPClientRequest _createClientRequest, int? ClientId, string ClientCode, string securityKey);
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

        public ClientMessageV1 CreateBXPClientRequestToClientMessageV1(CreateBXPClientRequest _createClientRequest, int? ClientId, string ClientCode, string securityKey)
        {
            ClientMessageV1 _result = new ClientMessageV1();
            if (_createClientRequest == null || !ClientId.HasValue)
            {
                return _result;
            }
            else
            {
                _result.InvoiceTitle = _createClientRequest.InvoiceTitle;
                _result.SecurityKey = securityKey;
                _result.SecurityAlgorithm = (int)SecurityAlgorithmLength.DES;
                _result.Status = 1;
                _result.InvoiceRegisterNumber = _createClientRequest.InvoiceRegisterNumber;
                _result.IdentityCode = ClientCode;
                _result.ClientName = _createClientRequest.ClientName;
                _result.ClientId = ClientId ?? 0;
                _result.DetailAddressLine = _createClientRequest.DetailAddressLine;
                _result.District = _createClientRequest.District;
                _result.CityId = _createClientRequest.CityId;
                _result.StateOrProvinceId = _createClientRequest.StateOrProvinceId;
                _result.Postcode = _createClientRequest.Postcode;
                _result.CountryId = _createClientRequest.CountryId;
                _result.Longitude = _createClientRequest.Longitude;
                _result.Latitude = _createClientRequest.Latitude;
                _result.AddressStatus = 1;
                return _result;
            }
        }
    }
}
