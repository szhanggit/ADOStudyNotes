using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Message
{
    public sealed class ServiceBusCli : IServiceBusCli
    {
        readonly ServiceBusClient cli;
        readonly ServiceBusSender sender;
        public ServiceBusCli(IConfiguration config)
        {
            cli = new ServiceBusClient(config.GetValue<string>("ServiceBus:ConnectionString"));
            sender = cli.CreateSender(config.GetValue<string>("ServiceBus:QueueName"));
        }

        public async Task SendMessage(object content, CancellationToken cancellationToken)
        {
            try
            {
                await sender.SendMessageAsync(new ServiceBusMessage(JsonConvert.SerializeObject(content)),cancellationToken);
            }
            catch
            {
                throw;
            }
            finally
            {
                await cli.DisposeAsync();
                await sender.DisposeAsync();
            }
        }

    }
}
