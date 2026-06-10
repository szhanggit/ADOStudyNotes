using Domain.Dto;

namespace ServiceMedia.Api.Authentication
{
    public interface IJwtAuth
    {
        string Authentication(UserDto userDto);
        string CreateSuperAdminAuthentication();
    }
}
