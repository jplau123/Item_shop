using Domain.Entities;

namespace Domain.Interfaces;

public interface IOrderRepository
{
    public Task<int?> Add(OrderEntity order);
}
