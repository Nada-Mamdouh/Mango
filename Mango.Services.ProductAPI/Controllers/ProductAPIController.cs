using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private IMapper _mapper;
        ResponseDTO ResponseDTO;
        public ProductAPIController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            ResponseDTO = new();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var prodList = await _dbContext.Products.ToListAsync();
            var products = _mapper.Map<List<ProductDTO>>(prodList);
            ResponseDTO.Result = products;
            return Ok(ResponseDTO);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var prod = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (prod == null)
            {
                ResponseDTO.IsSuccess = false;
                ResponseDTO.Message = "Product Not Found!";
                return NotFound(ResponseDTO);
            }
            var mappedProduct = _mapper.Map<ProductDTO>(prod);
            ResponseDTO.Result = mappedProduct;
            return Ok(ResponseDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDTO obj)
        {
            if (!ModelState.IsValid)
            {
                ResponseDTO.IsSuccess = false;
                ResponseDTO.Message = "Something went wrong while creating the product! Please modify your input!";
                return BadRequest(ResponseDTO);
            }
            var product = _mapper.Map<Product>(obj);
            await _dbContext.Products.AddAsync(product);
            var response = await _dbContext.SaveChangesAsync();
            if (response == 0)
            {
                ResponseDTO.IsSuccess = false;
                ResponseDTO.Message = "Failed to add Product!";
                return StatusCode(500, ResponseDTO);
            }
            ResponseDTO.IsSuccess = true;
            ResponseDTO.Message = "Product Created Successfully!";
            ResponseDTO.Result = product;
            return Ok(ResponseDTO);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProductDTO obj)
        {
            if (!ModelState.IsValid)
            {
                ResponseDTO.IsSuccess = false;
                ResponseDTO.Message = "Something went wrong while updating the product! Incomplete request data!";
                return BadRequest(ResponseDTO);
            }
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == obj.ProductId);
            if (product is null)
            {
                ResponseDTO.IsSuccess = false;
                ResponseDTO.Message = "This product was not Found!";
                return NotFound(ResponseDTO);
            }

            _mapper.Map(obj, product);
            var response = await _dbContext.SaveChangesAsync();
            if (response == 0)
            {
                ResponseDTO.IsSuccess = false;
                ResponseDTO.Message = "Failed to update Product!";
                return StatusCode(500, ResponseDTO);
            }
            ResponseDTO.IsSuccess = true;
            ResponseDTO.Message = "Product Updated Successfully!";
            ResponseDTO.Result = obj;
            return Ok(ResponseDTO);

        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var prod = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (prod is null)
            {
                ResponseDTO.IsSuccess = false;
                ResponseDTO.Message = "Product was not found!";
                return NotFound(ResponseDTO);
            }
            _dbContext.Products.Remove(prod);
            var response = await _dbContext.SaveChangesAsync();
            if (response == 0)
            {
                ResponseDTO.IsSuccess = false;
                ResponseDTO.Message = "Something went wrong while deleting product!";
                return StatusCode(500, ResponseDTO);
            }
            ResponseDTO.IsSuccess = true;
            ResponseDTO.Message = "Product deleted successfully!";
            return Ok(ResponseDTO);
        }

    }
}
