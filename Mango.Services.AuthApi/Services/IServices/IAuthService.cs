using Mango.Services.AuthApi.Models.DTOs;

namespace Mango.Services.AuthApi.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegisterRequestDto reqisterRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string Email, string roleName);
    }
}
