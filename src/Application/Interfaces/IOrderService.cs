using Application.DTOs.Requests;

namespace Application.Interfaces;

public interface IOrderService
{
    public Task<int> Create(OrderRequest request);
}
