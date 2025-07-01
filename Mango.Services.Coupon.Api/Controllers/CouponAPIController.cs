using AutoMapper;
using Mango.Services.Coupon.Api.Data;
using Mango.Services.Coupon.Api.Models;
using Mango.Services.Coupon.Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.Coupon.Api.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private IMapper _mapper;
        ResponseDTO response;

        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            response = new ResponseDTO();
        }
        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<CouponEntity> objList = _db.Coupons.ToList();
                response.Result = _mapper.Map<IEnumerable<CouponDTO>>(objList);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        [HttpGet]
        [Route("{id:int}")]
        public ResponseDTO Get(int id)
        {
            try
            {
                CouponEntity obj = _db.Coupons.First(x => x.CouponId == id);
                response.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        [HttpGet]
        [Route("GetByCouponCode/{code}")]
        public ResponseDTO Get(string code)
        {
            try
            {
                CouponEntity obj = _db.Coupons.First(x => x.CouponCode.ToLower() == code.ToLower());
                response.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        [HttpPost]
        public ResponseDTO Post([FromBody] CouponDTO couponDTO)
        {
            try
            {
                CouponEntity mappedObj = _mapper.Map<CouponEntity>(couponDTO);
                _db.Coupons.Add(mappedObj);
                _db.SaveChanges();
                response.Result = _mapper.Map<CouponDTO>(mappedObj);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        [HttpPut]
        public ResponseDTO Put([FromBody] CouponDTO couponDTO)
        {
            try
            {
                CouponEntity mappedObj = _mapper.Map<CouponEntity>(couponDTO);
                _db.Coupons.Update(mappedObj);
                _db.SaveChanges();
                response.Result = _mapper.Map<CouponDTO>(mappedObj);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDTO Delete(int id)
        {
            try
            {
                CouponEntity mappedObj = _db.Coupons.First(x => x.CouponId == id);
                _db.Coupons.Remove(mappedObj);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

    }
}
