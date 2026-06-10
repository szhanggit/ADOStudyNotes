using AutoMapper;
using Dapper;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Services.Core;
using Services.gRPCServices;
using Services.Utility.Telemetry;
using Services.Validators.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.CacheManagement;
using TXC.Common.Data;
using TXC.Common.Data.TenantDbConnection;
using TXC.Common.Domain;
using TXC.Common.MessageContract;
using TXC.Proto.Client;
using Xunit;

namespace UnitTest.Test.Validators
{
    public class CreateBXPClientValidatorTest : AbstractValidator<CreateBXPClientRequest>
    {

        [Fact]
        public async Task Should_Success()
        {
            var telemetry = new Mock<ITelemetryLogTrace<CreateBXPClientService>>();
            var tenantDbConnection = new Mock<ITenantDbConnection>();
            var dapperOperation = new Mock<IDapperOperation>();
            var iTX2ServiceBusSender = new Mock<ITX2ServiceBusSender>();
            var iTenantConfigHelper = new Mock<ITenantConfigHelper>();
            var checkAddressByCityDel = new Mock<CheckAddressByCityDel>();

            var iMapper = new Mock<IMapper>();
            var dbconn = new Mock<IDbConnection>();

            var tenantDbResponse = Response.Success("success", dbconn.Object);

            tenantDbConnection.Setup
                (
                    f => f.GetTenantDbConnection(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())

                ).ReturnsAsync(tenantDbResponse);

            dapperOperation.Setup(
                f => f.ProcessSql<ExecuteCommand, int>(It.IsAny<IDbConnection>(), It.IsAny<CommandDefinition>()))
                .ReturnsAsync(1);

            dapperOperation.Setup(
               f => f.ProcessSql<ExecuteCommandWithReturn<string>, string>(It.IsAny<IDbConnection>(), It.IsAny<CommandDefinition>()))
               .ReturnsAsync("C001");

            var CreateBXPClientService = new CreateBXPClientService(tenantDbConnection.Object, dapperOperation.Object, iTX2ServiceBusSender.Object, iTenantConfigHelper.Object, iMapper.Object);

            var request = new CreateBXPClientRequest
            {
                Longitude=1,
                CityId=3,
                ClientName="Test",
                CountryId=1,
                DetailAddressLine="Test",
                District = "Test",
                InvoiceRegisterNumber= "Test",
                InvoiceTitle="Test",
                Latitude=1,
                Postcode="Test",
                StateOrProvinceId=2,
                TenantId=2,
                TenantName = "IN"

            };

            var result = await CreateBXPClientService.CreateBXPClient(request);

            Assert.True(result.Success);
        }


        [Fact]
            public void Should_NotHave_Returned_Erro_for_Required_String()
            {
                var model = new CreateBXPClientRequest
                {
                    CityId = 1,
                    ClientName = "Test",
                    CountryId = 1,
                    DetailAddressLine = "Test",
                    District = "Test",
                    InvoiceRegisterNumber = "Test",
                    InvoiceTitle = "Test",
                    Latitude = 123,
                    Longitude = 123,
                    Postcode = "4000123",
                    StateOrProvinceId = 123

                };

                var validator = new CreateBXPClientValidator();
                var result = validator.TestValidate(model);
                result.ShouldNotHaveValidationErrorFor(p => p.DetailAddressLine);
                result.ShouldNotHaveValidationErrorFor(p => p.ClientName);
                result.ShouldNotHaveValidationErrorFor(p => p.InvoiceRegisterNumber);

            }

            [Fact]
            public void Should_NotHave_Returned_Erro_for_CityID_Nonzero()
            {
                var model = new CreateBXPClientRequest
                {
                    CityId = 1,
                    ClientName = "Test",
                    CountryId = 1,
                    DetailAddressLine = "Test",
                    District = "Test",
                    InvoiceRegisterNumber = "",
                    InvoiceTitle = "Test",
                    Latitude = 123,
                    Longitude = 123,
                    Postcode = "4000123",
                    StateOrProvinceId = 123

                };

                var validator = new CreateBXPClientValidator();
                var result = validator.TestValidate(model);
                result.ShouldNotHaveValidationErrorFor(p => p.CityId);
            }


            [Fact]
            public void Should_Have_Returned_Erro_for_CountryId_Nnozero()
            {
                var model = new CreateBXPClientRequest
                {
                    CityId = 1,
                    ClientName = "Test",
                    CountryId = 2,
                    DetailAddressLine = "Test",
                    District = "Test",
                    InvoiceRegisterNumber = "",
                    InvoiceTitle = "Test",
                    Latitude = 123,
                    Longitude = 123,
                    Postcode = "4000123",
                    StateOrProvinceId = 123

                };

                var validator = new CreateBXPClientValidator();
                var result = validator.TestValidate(model);
                result.ShouldNotHaveValidationErrorFor(p => p.CountryId);
            }

            [Fact]
            public void Should_NotHave_Returned_Erro_for_LatLog_Nonzero()
            {
                var model = new CreateBXPClientRequest
                {
                    CityId = 1,
                    ClientName = "Test",
                    CountryId = 1,
                    DetailAddressLine = "Test",
                    District = "Test",
                    InvoiceRegisterNumber = "",
                    InvoiceTitle = "Test",
                    Latitude = 1,
                    Longitude = 1,
                    Postcode = "4000123",
                    StateOrProvinceId = 123

                };

                var validator = new CreateBXPClientValidator();
                var result = validator.TestValidate(model);
                result.ShouldNotHaveValidationErrorFor(p => p.Latitude);
                result.ShouldNotHaveValidationErrorFor(p => p.Longitude);

            }
        }
    }
