using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, IReadOnlyCollection<OrdersVm>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken ct)
        {
            var orderList = await _orderRepository.GetOrdersByUserNameAsync(request.UserName);
            return _mapper.Map<IReadOnlyCollection<OrdersVm>>(orderList);
        }
    }
}
