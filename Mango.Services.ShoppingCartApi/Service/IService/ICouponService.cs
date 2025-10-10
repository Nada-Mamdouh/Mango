using Mango.Services.ShoppingCartApi.Models.DTOs;

namespace Mango.Services.ShoppingCartApi.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDTO> GetCoupon(string couponCode);
    }
}
