using System.Diagnostics.CodeAnalysis;
using TXC.Common.Services.Wrappers;

namespace Services.Command.ImageMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class RenameImageMediaCommand : IRequestWrapper<int>
    {
        public int MediaId { get; set; }
        public string Keyword { get; set; }
    }
}
