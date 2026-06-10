using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public enum EMediaMessageActionType
    {
        Create = 1,
        Rename = 2,
        Replace = 3,
        Delete = 4
    }
    public class MediaMessageV1
    {
        public EMediaMessageActionType ActionType { get; set; }
        public int TenantId { get; set; }
        public string ActionBody { get; set; }
    }
}
