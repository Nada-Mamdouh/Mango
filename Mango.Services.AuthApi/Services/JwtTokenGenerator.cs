using Mango.Services.AuthApi.Models;
using Mango.Services.AuthApi.Services.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mango.Services.AuthApi.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JWTSettings _jwtOptions;
        public JwtTokenGenerator(IOptions<JWTSettings> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            //1- token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            //2- encode the key
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            //3- claim list
            var claimList = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Name,  applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email)
            };
            //4- Add roles to claims
            claimList.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            //5- token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //6- create token 
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //7- write it in response
            return tokenHandler.WriteToken(token);
        }
    }
}
