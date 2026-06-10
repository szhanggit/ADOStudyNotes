using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOAccess
{
    public class EfException : Exception
    {
        public EfException()
        {
        }

        public EfException(string message)
            : base(message)
        {
        }

        public EfException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
