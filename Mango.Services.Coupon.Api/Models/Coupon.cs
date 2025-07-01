using System.ComponentModel.DataAnnotations;

namespace Mango.Services.Coupon.Api.Models
{
    public class CouponEntity
    {
        [Key]
        public int CouponId { get; set; }
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
