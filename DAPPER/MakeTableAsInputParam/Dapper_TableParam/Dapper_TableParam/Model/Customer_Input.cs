using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper_TableParam.Model
{
    public class Customer_Input
    {
        private string _customerId = string.Empty;
        private string _contactName = string.Empty;
        List<Customer_Input> _selectedCustomer = null;

        public string CustomerId
        {
            set { _customerId = value; }
            get { return _customerId;  }
        }

        public string ContactName
        {
            set { _contactName = value; }
            get { return _contactName; }
        }

        public List<Customer_Input> GetData()
        {
            _selectedCustomer = new List<Customer_Input> {
                new Customer_Input() { CustomerId = "ALFKI", ContactName = "Maria Anders" },
                new Customer_Input() { CustomerId = "ANATR", ContactName = "Ana Trujillo" },
                new Customer_Input() { CustomerId = "ANTON", ContactName = "Antonio Moreno" },
                new Customer_Input() { CustomerId = "AROUT", ContactName = "Thomas Hardy" },
                new Customer_Input() { CustomerId = "BERGS", ContactName = "Christina Berglund" }};

            return _selectedCustomer;
        }
    }
}
