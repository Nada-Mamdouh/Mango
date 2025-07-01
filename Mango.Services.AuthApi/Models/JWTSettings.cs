namespace Mango.Services.AuthApi.Models
{
    public class JWTSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Client { get; set; } = string.Empty;
    }
}
