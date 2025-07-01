using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface ICouponService
    {
        Task<ResponseDTO?> GetCouponAsync(string code);
        Task<ResponseDTO?> GetCouponByIdAsync(int id);
        Task<ResponseDTO?> GetAllCouponsAsync();
        Task<ResponseDTO?> CreateCouponsAsync(CouponDTO couponDTO);
        Task<ResponseDTO?> UpdateCouponsAsync(CouponDTO couponDTO);
        Task<ResponseDTO?> DeleteCouponsAsync(int id);
    }
}
