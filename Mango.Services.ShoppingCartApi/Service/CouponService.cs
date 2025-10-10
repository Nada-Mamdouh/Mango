using Mango.Services.ShoppingCartApi.Models.DTOs;
using Mango.Services.ShoppingCartApi.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartApi.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<CouponDTO> GetCoupon(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/coupon/GetByCouponCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync(); 
            var resp = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(resp?.Result));
            }
            return new CouponDTO();
        }
    }
}
