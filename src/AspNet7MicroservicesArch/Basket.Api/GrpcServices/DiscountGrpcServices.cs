using Discount.Grpc.Protos;

namespace Basket.Api.GrpcServices
{
    public class DiscountGrpcServices
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _client;

        public DiscountGrpcServices(DiscountProtoService.DiscountProtoServiceClient client)
        {
            _client = client;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };

            return await _client.GetDiscountAsync(discountRequest);
        }
    }
}
