using AutoMapper;
using Mango.Services.Coupon.Api.Models;
using Mango.Services.Coupon.Api.Models.DTOs;

namespace Mango.Services.Coupon.Api
{
    public class MappingConfigs
    {
        public static MapperConfiguration RegisterMaps()
        {
            MapperConfiguration mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDTO, CouponEntity>();
                config.CreateMap<CouponEntity, CouponDTO>();
            });
            return mapperConfig;
        }
    }
}
