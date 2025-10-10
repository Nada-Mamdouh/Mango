using AutoMapper;
using Mango.Services.ShoppingCartApi.Data;
using Mango.Services.ShoppingCartApi.Models;
using Mango.Services.ShoppingCartApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartApi.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private ResponseDTO _response;
        private IMapper _mapper;
        private readonly AppDbContext _db;
        public CartAPIController(AppDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
        }
        [HttpPost("CartUpsert")]
        public async Task<ResponseDTO> CartUpsert(CartDTO cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeader.AsNoTracking().FirstOrDefaultAsync(ch => ch.UserId == cartDto.CartHeader.UserId);
                //Case1: the cart is empty, add header & details
                if (cartHeaderFromDb == null) {
                    var cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeader.Add(cartHeader);
                    await _db.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //Case2: if header is not null, check details has the same product
                    var cardDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        cd => cd.ProductId == cartDto.CartDetails.First().ProductId
                        && cd.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cardDetailsFromDb == null)
                    {
                        //Case2 cont.: same product doesn't exist - create card details
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //Case3: same product exist, update count
                        cartDto.CartDetails.First().Count += cardDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cardDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailId = cardDetailsFromDb.CartDetailId;

                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
