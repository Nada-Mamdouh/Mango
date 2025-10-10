using AutoMapper;
using Mango.Services.ShoppingCartApi.Models;
using Mango.Services.ShoppingCartApi.Models.DTOs;

namespace Mango.Services.ShoppingCartApi
{
    public class MappingConfigs
    {
        public static MapperConfiguration RegisterMap()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeaderDTO, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailDTO, CartDetails>().ReverseMap();
            });
            return mapperConfiguration;
        }
    }
}
