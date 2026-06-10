using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [ExcludeFromCodeCoverageAttribute]
    public class ClientAddress
    {
        private Client _client;
        private Address _address;
        public ClientAddress()
        {
            _client = new Client();
            _address = new Address();
        }
        public Client Client { get { return _client;  } set { _client = value; } }
        public Address Address { get { return _address; } set { _address = value; } }
    }
}
