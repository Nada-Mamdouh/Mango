using Mango.Services.ShoppingCartApi.Models.DTOs;

namespace Mango.Services.ShoppingCartApi.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
    }
}
