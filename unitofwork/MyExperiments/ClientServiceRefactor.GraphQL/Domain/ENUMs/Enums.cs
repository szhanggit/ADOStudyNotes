using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum SecurityAlgorithm
    {
        DES = 1,
        AES = 2
    }

    public enum SecurityAlgorithmLength
    {
        DES = 15,
        AES = 31
    }
}
