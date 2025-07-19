using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;

namespace Mango.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> AssignRoleAsync(RegisterRequestDto request)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = request,
                Url = SD.AuthBaseApi + "/api/auth/AssignRole"
            }, withTokenEnabled: false);
        }

        public async Task<ResponseDTO?> LoginAsync(LoginRequestDto request)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = request,
                Url = SD.AuthBaseApi + "/api/auth/login"
            }, withTokenEnabled: false);
        }

        public async Task<ResponseDTO?> RegisterAsync(RegisterRequestDto request)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = request,
                Url = SD.AuthBaseApi + "/api/auth/register"
            }, withTokenEnabled: false);
        }
    }
}
