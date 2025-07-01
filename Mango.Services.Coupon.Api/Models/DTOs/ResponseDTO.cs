namespace Mango.Services.Coupon.Api.Models.DTOs
{
    /// <summary>
    /// This is a common response returned from all api end points
    /// It's considered best practice, especially for consumers of these end points.
    /// </summary>
    public class ResponseDTO
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
