using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.MessageContract;

namespace Services.Models
{
    public class CreateMediaMessageV1 : MessageBody
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Keyword { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Url { get; set; }
        public int MediaCategory { get; set; }
        public string TX2UserName { get; set; }
    }
}