using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using TXC.Proto.Client;

namespace ProtoConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create the Client
            // The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("http://localhost:9005");
            var client = new Client.ClientClient(channel);

            //var insertProductSkuBackupInfo = await CreateClient(client);
            //var createBXPClient = await CreateBXPClient(client);
            var updateClient = await UpdateClient(client);
            //var getClient = await GetClient(client);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

        }

        static async Task<string> CreateClient(Client.ClientClient client)
        {
            CreateClientResponse reply;

            // Create a call / request
            try
            {
                reply = await client.CreateClientAsync(new CreateClientRequest()
                {
                    TenantId = 9,
                    TenantName = "GL",
                    ClientName = "SevenEleven",
                    InvoiceRegisterNumber = "InvoiceRegisterNumber",
                    Status = 1,
                    SecurityAlgorithm = 3,
                    SecurityKey = "jdsaaaaaaajdsaaaaaaajdsaaaaaaajdsaaaaaaa",
                    NeedNotification = true,
                    CanIssue = true,
                    CountryId = 6,
                    StateOrProvinceId = 8,
                    CityId = 11,
                    AddressStatus = 1
                });
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw;
            }

            return reply.Message;
        }

        static async Task<string> CreateBXPClient(Client.ClientClient client)
        {
            CreateBXPClientResponse reply;

            try
            {
                reply = await client.CreateBXPClientAsync(new CreateBXPClientRequest()
                {
                    TenantId = 9,
                    TenantName = "GL",
                    ClientName = "SevenEleven",
                    InvoiceRegisterNumber = "InvoiceRegisterNumber",
                    CountryId = 6,
                    StateOrProvinceId = 8,
                    CityId = 11,
                });
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw;
            }

            return reply.Message;
        }

        static async Task<string> UpdateClient(Client.ClientClient client)
        {
            // Create a call / request
            var reply = await client.UpdateClientAsync(new UpdateClientRequest()
            {
                TenantId = 9,
                TenantName = "GL",
                ClientId = 10,
                ClientName = "SevenEleven",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                Status = 1,
                SecurityAlgorithm = 3,
                SecurityKey = "SecurityKey",
                NeedNotification = true,
                CanIssue = true,
            });

            return reply.Message;
        }

        static async Task<string> GetClient(Client.ClientClient client)
        {
            // Create a call / request
            ProtoBaseResponse reply;
            reply = await client.GetClientListAsync(new GetClientListRequest()
            {
                TenantId = 9,
                TenantName = "GL",
                SearchKeyword = "0000023"
            });

            return reply.Message;
        }
    }
}