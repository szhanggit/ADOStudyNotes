using TXC.Common.Services.Wrappers;

namespace Services.Command.Client
{
    public class GenerateClientIdentityCodeCommand : IRequestWrapper<string>
    {
        public int TenantID { get; set; }
        public string SequenceName { get; set; }
        public bool IsFixReturnLength { get; set; }
        public byte ReturnLength { get; set; }
        public char PaddingCharacter { get; set; }
    }
}
