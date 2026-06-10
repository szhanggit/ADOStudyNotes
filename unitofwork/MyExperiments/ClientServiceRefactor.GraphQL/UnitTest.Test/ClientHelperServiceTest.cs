using Service.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Test
{
    public class ClientHelperServiceTest
    {
        private IClientHelperService _clientHelperService;
        public ClientHelperServiceTest()
        {
            _clientHelperService = new ClientHelperService();
        }

        [Fact]
        public async Task GetSkipNum_PageNumber1RowCount1_ShallReturnZero()
        {
            int? PageNumber = 1;
            int? RowCount = 1;
            int skip = _clientHelperService.GetSkipNum(PageNumber, RowCount);

            Assert.Equal(0, skip);
        }

        [Fact]
        public async Task GetSkipNum_PageNumber2RowCount10_ShallReturn20()
        {
            int? PageNumber = 2;
            int? RowCount = 10;
            int skip = _clientHelperService.GetSkipNum(PageNumber, RowCount);

            Assert.Equal(10, skip);
        }

        [Fact]
        public async Task GetSkipNum_PageNumberNullRowCountNull_ShallReturnZero()
        {
            int? PageNumber = null;
            int? RowCount = null;
            int skip = _clientHelperService.GetSkipNum(PageNumber, RowCount);

            Assert.Equal(0, skip);
        }
    }
}
