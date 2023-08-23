using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetPreconfiguredOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
        }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>
            {
                new Order {
                    UserName = "UserAdm", 
                    FirstName = "João", 
                    LastName = "José", 
                    EmailAddress = "Joao@gmail.com", 
                    AddressLine = "Rua A", 
                    Country = "Fortaleza", 
                    TotalPrice = 350,
                    CreatedBy = "Adm",
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = "Adm",
                    LastModifiedDate = DateTime.Now,
                }
            };
    }
}
