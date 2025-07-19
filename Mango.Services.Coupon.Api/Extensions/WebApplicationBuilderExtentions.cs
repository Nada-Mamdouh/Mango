using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mango.Services.Coupon.Api.Extensions
{
    public static class WebApplicationBuilderExtentions
    {
        public static WebApplicationBuilder AddApplicationAuthentication(this WebApplicationBuilder builder)
        {
            var sectionSettings = builder.Configuration.GetSection("ApiSettings");
            var secret = sectionSettings.GetValue<string>("Secret");
            var issuer = sectionSettings.GetValue<string>("Issuer");
            var audience = sectionSettings.GetValue<string>("Audience");

            var key = Encoding.ASCII.GetBytes(secret);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidAudience = audience,
                };
            });

            return builder;
        }
    }
}
