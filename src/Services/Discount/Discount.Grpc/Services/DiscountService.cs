using Discount.Grpc.Extensions;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly ILogger<DiscountService> _logger;
        public DiscountService(
            ILogger<DiscountService> logger,
            IDiscountRepository repository)
        {

            _logger = logger;
            _repository = repository;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetDiscount(request.ProductName);

            if (coupon is null)
                throw new RpcException(
                    new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} not found")
                    );

            return coupon.ToCouponModel();
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.ToCoupon();

            await _repository.CreateDiscount(coupon);

            _logger.LogInformation("Discount is successfuly created. ProductName: {ProductName}", coupon.ProductName);

            return coupon.ToCouponModel();
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.ToCoupon();

            var boolean = await _repository.UpdateDiscount(coupon);

            if (boolean)
                _logger.LogInformation("Discount is sucessfully updated. ProductName : {ProductName}", coupon.ProductName);
            else
                _logger.LogError($"No coupon found with Id={coupon.Id}.");

            return coupon.ToCouponModel();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _repository.DeleteDiscount(request.ProductName);

            return new DeleteDiscountResponse
            {
                Success = deleted
            };
        }
    }
}
