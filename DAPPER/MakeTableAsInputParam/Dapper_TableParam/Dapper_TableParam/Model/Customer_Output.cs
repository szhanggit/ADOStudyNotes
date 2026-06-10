using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper_TableParam.Model
{
    public class Customer_Output
    {
        private string _companyName = string.Empty;
        private string _contactTitle = string.Empty;
        private string _contactName = string.Empty;

        public string CompanyName {
            set { _companyName = value; }
            get { return _companyName; }
        }
        public string ContactTitle {
            set { _contactTitle = value; }
            get { return _contactTitle; }
        }  
        public string ContactName {
            set { _contactName = value; }
            get { return _contactName; }
        }
    }
}
