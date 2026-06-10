using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF2
{
    public interface IContextProvider
    {
        void SetConnection(string connectionString);
    }
}
