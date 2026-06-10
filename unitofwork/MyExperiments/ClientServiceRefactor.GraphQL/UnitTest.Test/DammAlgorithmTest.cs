using Service.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Test
{
    public class DammAlgorithmTest
    {
        private IDammAlgorithm _dammAlgorithm;
        public DammAlgorithmTest()
        {
            _dammAlgorithm = new DammAlgorithm();
        }

        [Fact]
        public async Task CalculateCheckSum_23_ShallReturn2()
        {
            int result = _dammAlgorithm.CalculateCheckSum("23");
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task CalculateCheckSum_230000_ShallReturn3()
        {
            int result = _dammAlgorithm.CalculateCheckSum("230000");
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task CalculateCheckSumWithInt_ShallReturn7()
        {
            CalculateCheckSumDel _cal = _dammAlgorithm.CalculateCheckSum;
            int result = _dammAlgorithm.CalculateCheckSum(3, _cal);
            Assert.Equal(7, result);
        }

        [Fact]
        public async Task CalculateCheckSumWithLong_ShallReturn4()
        {
            CalculateCheckSumDel _cal = _dammAlgorithm.CalculateCheckSum;
            int result = _dammAlgorithm.CalculateCheckSum(30000, _cal);
            Assert.Equal(4, result);
        }

        [Fact]
        public async Task GenerateCheckSum_ShallReturn300004()
        {
            CalculateCheckSumDel _cal = _dammAlgorithm.CalculateCheckSum;
            string result = _dammAlgorithm.GenerateCheckSum("30000", _cal);
            Assert.Equal("300004", result);
        }

        [Fact]
        public async Task GenerateCheckSumWithInt_ShallReturn37()
        {
            CalculateCheckSumDel _cal = _dammAlgorithm.CalculateCheckSum;
            CalculateCheckSumWithIntDel _cal2 = _dammAlgorithm.CalculateCheckSum;
            int result = _dammAlgorithm.GenerateCheckSum(3, _cal, _cal2);
            Assert.Equal(37, result);
        }

        [Fact]
        public async Task GenerateCheckSumWithLong_ShallReturn300004()
        {
            CalculateCheckSumDel _cal = _dammAlgorithm.CalculateCheckSum;
            CalculateCheckSumWithLongDel _cal2 = _dammAlgorithm.CalculateCheckSum;
            long result = _dammAlgorithm.GenerateCheckSum(30000, _cal, _cal2);
            Assert.Equal(300004, result);
        }

        [Fact]
        public async Task ValidateWithString_ShallReturnFalse()
        {
            CalculateCheckSumDel _cal = _dammAlgorithm.CalculateCheckSum;
            bool result = _dammAlgorithm.Validate("3", _cal);
            Assert.Equal(false, result);
        }

        [Fact]
        public async Task ValidateWithInt_ShallReturnFalse()
        {
            CalculateCheckSumDel _cal = _dammAlgorithm.CalculateCheckSum;
            CalculateCheckSumWithIntDel _cal2 = _dammAlgorithm.CalculateCheckSum;
            bool result = _dammAlgorithm.Validate(3, _cal, _cal2);
            Assert.Equal(false, result);
        }

        [Fact]
        public async Task ValidateWithLong_ShallReturnFalse()
        {
            CalculateCheckSumDel _cal = _dammAlgorithm.CalculateCheckSum;
            CalculateCheckSumWithIntDel _cal2 = _dammAlgorithm.CalculateCheckSum;
            bool result = _dammAlgorithm.Validate(30000, _cal, _cal2);
            Assert.Equal(false, result);
        }
    }
}
