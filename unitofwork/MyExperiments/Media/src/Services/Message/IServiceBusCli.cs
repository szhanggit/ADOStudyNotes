using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Message
{
    public interface IServiceBusCli
    {
        Task SendMessage(object content, CancellationToken cancellationToken);
    }
}
