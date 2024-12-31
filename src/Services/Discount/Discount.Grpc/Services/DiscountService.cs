using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService 
    (DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        //TODO: getDiscount form database
        var coupon = await dbContext
                    .Coupons
                    .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        if (coupon == null)
            coupon = new Coupon { ProductName = "No discount", Amount = 0, Description = "No Discount Desc" };

        logger.LogInformation("Discount is  retrieved for ProductName: {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

        var coupleModel = coupon.Adapt<CouponModel>();
        return coupleModel;
    }

    public override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        return base.CreateDiscount(request, context);
    }

    public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        return base.UpdateDiscount(request, context);
    }

    public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        return base.DeleteDiscount(request, context);
    }
}
