using Mango.Web.Services.IServices;
using Mango.Web.Utility;

namespace Mango.Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        IHttpContextAccessor _httpContextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _httpContextAccessor = contextAccessor;
        }
        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(SD.TOKENCOOKIE, token);
        }
        public string? GetToken()
        {
            string? tokenVal = null;
            bool? hasToken = _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TOKENCOOKIE, out tokenVal);
            return hasToken is true ? tokenVal : null;
        }
        public void ClearToken()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(SD.TOKENCOOKIE);
        }

    }
}
