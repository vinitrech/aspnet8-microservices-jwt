using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController(AppDbContext dbContext, IMapper mapper) : ControllerBase
    {
        private ResponseDto responseDto = new();

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                var coupons = dbContext.Coupons.ToList();

                responseDto.Result = mapper.Map<IEnumerable<CouponDto>>(coupons);
            }
            catch (Exception e)
            {
                responseDto.Success = false;
                responseDto.Message = e.Message;
            }

            return responseDto;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                var coupon = dbContext.Coupons.First(c => c.CouponId == id);

                responseDto.Result = mapper.Map<CouponDto>(coupon);
            }
            catch (Exception e)
            {
                responseDto.Success = false;
                responseDto.Message = e.Message;

            }

            return responseDto;
        }

        [HttpGet]
        [Route("{code:string}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                var coupon = dbContext.Coupons.FirstOrDefault(c => string.Equals(c.CouponCode!.ToLower(), code.ToLower()));

                if (coupon is null)
                {
                    responseDto.Success = false;
                    responseDto.Message = "Item not found";

                    return responseDto;
                }

                responseDto.Result = mapper.Map<CouponDto>(coupon);
            }
            catch (Exception e)
            {
                responseDto.Success = false;
                responseDto.Message = e.Message;
            }

            return responseDto;
        }

        [HttpPost]
        public ResponseDto Post(CouponDto couponDto)
        {
            try
            {
                var coupon = mapper.Map<Coupon>(couponDto);

                dbContext.Coupons.Add(coupon);
                dbContext.SaveChanges();

                responseDto.Result = mapper.Map<CouponDto>(coupon);
            }
            catch (Exception e)
            {
                responseDto.Success = false;
                responseDto.Message = e.Message;
            }

            return responseDto;
        }

        [HttpPut]
        public ResponseDto Put(CouponDto couponDto)
        {
            try
            {
                var coupon = mapper.Map<Coupon>(couponDto);

                dbContext.Coupons.Update(coupon);
                dbContext.SaveChanges();

                responseDto.Result = mapper.Map<CouponDto>(coupon);
            }
            catch (Exception e)
            {
                responseDto.Success = false;
                responseDto.Message = e.Message;
            }

            return responseDto;
        }

        [HttpDelete]
        public ResponseDto Put(int id)
        {
            try
            {
                var coupon = dbContext.Coupons.First(c => c.CouponId == id);

                dbContext.Coupons.Remove(coupon);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                responseDto.Success = false;
                responseDto.Message = e.Message;
            }

            return responseDto;
        }
    }
}
