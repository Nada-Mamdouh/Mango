using Mango.Services.Auth.Api.Data;
using Mango.Services.AuthApi.Models;
using Mango.Services.AuthApi.Models.DTOs;
using Mango.Services.AuthApi.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        //These classes are provided by identity to set all data associated with users 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //defined
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator tokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenGenerator = tokenGenerator;
        }


        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            //1- Get user by user name
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            //2- check if the password matches
            var isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || !isValid)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }
            //3- If user was found, generate JWT Token
            var token = _tokenGenerator.GenerateToken(user);
            //4- Return correct answer
            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Token = token
            };
            return loginResponseDto;

        }

        public async Task<string> Register(RegisterRequestDto reqisterRequestDto)
        {
            ApplicationUser applicationUser = new()
            {
                UserName = reqisterRequestDto.Email,
                Name = reqisterRequestDto.Name,
                Email = reqisterRequestDto.Email,
                NormalizedEmail = reqisterRequestDto.Email.ToUpper(),
                PhoneNumber = reqisterRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, reqisterRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToBeReturned = _db.ApplicationUsers.First(u => u.UserName == reqisterRequestDto.Email);
                    UserDto userdto = new UserDto()
                    {
                        Id = userToBeReturned.Id,
                        Name = userToBeReturned.Name,
                        Email = userToBeReturned.Email,
                        PhoneNumber = userToBeReturned.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    //Returning Identity descriptive error
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex) { }
            return "Error encountered!";
        }

        public async Task<bool> AssignRole(string Email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == Email.ToLower());
            if (user != null)
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }
    }
}
