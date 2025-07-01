using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> CreateCouponsAsync(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = SD.CouponBaseApi + "/api/coupon",
                Data = couponDTO
            });
        }

        public async Task<ResponseDTO?> DeleteCouponsAsync(int id)
        {
            return await _baseService.SendAsync(
                new RequestDto()
                {
                    ApiType = ApiType.DELETE,
                    Url = SD.CouponBaseApi + "/api/coupon/" + id
                });
        }

        public async Task<ResponseDTO?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(
                new RequestDto()
                {
                    ApiType = ApiType.GET,
                    Url = SD.CouponBaseApi + "/api/coupon"
                });
        }

        public async Task<ResponseDTO?> GetCouponAsync(string code)
        {
            return await _baseService.SendAsync(
                new RequestDto()
                {
                    ApiType = ApiType.GET,
                    Url = SD.CouponBaseApi + "/api/coupon/GetByCouponCode/" + code
                });
        }

        public async Task<ResponseDTO?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(
                new RequestDto()
                {
                    ApiType = ApiType.GET,
                    Url = SD.CouponBaseApi + "/api/coupon/" + id
                });
        }

        public async Task<ResponseDTO?> UpdateCouponsAsync(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(
                new RequestDto()
                {
                    ApiType = ApiType.PUT,
                    Url = SD.CouponBaseApi + "/api/coupon/",
                    Data = couponDTO
                }
                );
        }
    }
}
