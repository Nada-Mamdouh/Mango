﻿using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IProductService
    {
        //Task<ResponseDTO?> GetProductAsync(string code);
        Task<ResponseDTO?> GetProductByIdAsync(int id);
        Task<ResponseDTO?> GetAllProductsAsync();
        Task<ResponseDTO?> CreateProductAsync(ProductDTO productDTO);
        Task<ResponseDTO?> UpdateProductAsync(ProductDTO productDTO);
        Task<ResponseDTO?> DeleteProductAsync(int id);
    }
}
