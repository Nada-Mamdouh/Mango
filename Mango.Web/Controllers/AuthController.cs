using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CommonError", responseDto.Message);
                return View(obj);
            }
        }
        public IActionResult Logout()
        {
            return View();
        }
    }
}
