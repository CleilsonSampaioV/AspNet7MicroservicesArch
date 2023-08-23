using Ordering.Domain.Entities;

namespace Ordering.Application.Contracts.Persistence;

public interface IOrderRepository : IRepositoryBase<Order>
{
    Task<IReadOnlyCollection<Order>> GetOrdersByUserNameAsync(string userName);
}
