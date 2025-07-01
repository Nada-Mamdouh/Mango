using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IAuthService
    {
        Task<ResponseDTO?> LoginAsync(LoginRequestDto request);
        Task<ResponseDTO?> RegisterAsync(RegisterRequestDto request);
        Task<ResponseDTO?> AssignRoleAsync(RegisterRequestDto request);
    }
}
