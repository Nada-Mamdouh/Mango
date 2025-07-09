using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        IAuthService _authService;
        ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }
        [HttpGet]
        public IActionResult Register()
        {
            //RegisterRequestDto request = new();
            var roleList = new List<SelectListItem>() {
                new SelectListItem{Text = "Admin", Value = SD.ADMINROLE},
                new SelectListItem{Text = "Customer", Value=SD.CUSTOMERROLE}
            };
            ViewBag.RoleList = roleList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto obj)
        {
            ResponseDTO result = await _authService.RegisterAsync(obj);
            ResponseDTO roleResponse;
            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = SD.CUSTOMERROLE;
                }
                roleResponse = await _authService.AssignRoleAsync(obj);
                if (roleResponse != null && roleResponse.IsSuccess)
                {
                    TempData["Success"] = "Registeration done successfully!";
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    TempData["Error"] = roleResponse?.Message;
                }
            }
            var roleList = new List<SelectListItem>() {
                new SelectListItem{Text = "Admin", Value = SD.ADMINROLE},
                new SelectListItem{Text = "Customer", Value=SD.CUSTOMERROLE}
            };
            ViewBag.RoleList = roleList;
            return View(obj);

        }
        public IActionResult Login()
        {
            LoginRequestDto request = new();
            return View(request);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            var responseDto = await _authService.LoginAsync(obj);
            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                await SignInUser(loginResponse);
                _tokenProvider.SetToken(loginResponse!.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //ModelState.AddModelError("CommonError", responseDto.Message);
                TempData["Error"] = responseDto?.Message;
                return View(obj);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }
        private async Task SignInUser(LoginResponseDto model)
        {
            //1- jwt handler
            var handler = new JwtSecurityTokenHandler();
            //2- read token 
            var jwt = handler.ReadJwtToken(model.Token);
            //3- add claims
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
            //identity.AddClaim

            //4- form principal
            var principal = new ClaimsPrincipal(identity);
            //5- perform sign in 
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        }
    }
}
