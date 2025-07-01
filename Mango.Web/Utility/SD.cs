namespace Mango.Web.Utility
{
    /// <summary>
    /// Static details utility class
    /// </summary>
    public static class SD
    {
        public static string CouponBaseApi { get; set; }
        public static string AuthBaseApi { get; set; }
        public const string ADMINROLE = "Admin";
        public const string CUSTOMERROLE = "Customer";
        public const string TOKENCOOKIE = "JWTToken";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
