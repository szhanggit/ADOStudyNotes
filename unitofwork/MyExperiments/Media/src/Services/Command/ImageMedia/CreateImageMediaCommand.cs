using Domain.EnumList;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using TXC.Common.Services.Wrappers;

namespace Services.Command.ImageMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class CreateImageMediaCommand : IRequestWrapper<int>
    {
        public ImageCategory Type {get;set;}
        public IFormFile Image { get; set; }
    }
}
