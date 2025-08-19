using Mango.Services.AuthApi.Models.DTOs;
using Mango.Services.AuthApi.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDTO response;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            response = new();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            string errorMessage = await _authService.Register(request);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                response.IsSuccess = false;
                response.Message = errorMessage;
                return BadRequest(response);
            }
            return Ok(response);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var loginResponse = await _authService.Login(request);
            if (loginResponse.User == null)
            {
                response.IsSuccess = false;
                response.Message = "Username or Password is incorrect!";
                return BadRequest(response);
            }
            response.Result = loginResponse;
            //Persist token in cookie here
            return Ok(response);
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterRequestDto request)
        {
            bool assignRoleResponse = await _authService.AssignRole(request.Email, request.Role.ToUpper());
            if (!assignRoleResponse)
            {
                response.IsSuccess = false;
                response.Message = "Error Encountered while assigning role";
                return BadRequest(response);
            }
            return Ok(response);

        }
    }
}
