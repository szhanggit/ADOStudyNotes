using Microsoft.Extensions.Configuration;
using Moq;
using Services.Utility.GraphQLClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Test.Utility.GraphQLClient
{
    [ExcludeFromCodeCoverageAttribute]
    public class MediaGraphQLClient
    {
        [Fact]
        public async Task NullConfig()
        {
            var mockConf = new Mock<IConfiguration>();

            var cli = new MediaGrapQLClient(mockConf.Object);
            var res = cli.GetGraphQLClient(9);

            Assert.Null(res);
        }
    }
}
