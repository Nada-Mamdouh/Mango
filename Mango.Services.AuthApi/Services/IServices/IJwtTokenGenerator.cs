using Mango.Services.AuthApi.Models;

namespace Mango.Services.AuthApi.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
