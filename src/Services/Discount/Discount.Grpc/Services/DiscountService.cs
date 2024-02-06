using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository repository;
        private readonly ILogger<DiscountService> logger;
        private readonly IMapper mapper;

        public DiscountService(IDiscountRepository reopsitory, ILogger<DiscountService> logger, IMapper mapper)
        {
            this.repository = reopsitory ?? throw new ArgumentNullException(nameof(reopsitory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        { 
            var coupon = await repository.GetDiscount(request.ProductName);

            if(null == coupon)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName}"));
            return mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupon>(request.Coupon);

            await repository.CreateDiscount(coupon);

            logger.LogInformation("Discount is successfully created for ProductName: {0}", coupon.ProductName);

            return mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = await repository.GetDiscount(request.Coupon.ProductName);

            if (null == coupon)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.Coupon.ProductName}"));

            await repository.UpdateDiscount(coupon);

            return request.Coupon;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await repository.GetDiscount(request.ProductName);

            if (null == coupon)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName}"));
            var deleted = await repository.DeleteDiscount(request.ProductName);
            return new DeleteDiscountResponse
            {
                Success = deleted
            };
        }
    }
}
