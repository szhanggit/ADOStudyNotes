using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.MessageContract;

namespace Services.Models
{
    public class RenameMediaMessageV1 : MessageBody
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string TX2UserName { get; set; }
    }
}