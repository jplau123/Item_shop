using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface IShopService
{
    public Task<List<ShopResponse>> GetAll();
    public Task<List<ShopWithItemsResponse>> GetAllWithItems();
    public Task<ShopWithItemsResponse> GetByIdWithItems(int id);
    public Task<ShopResponse> GetById(int id);
    public Task<int> Add(ShopRequest item);
    public Task Update(ShopRequest item, int id);
    public Task Delete(int id);
}
