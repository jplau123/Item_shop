using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface IItemService
{
    public Task<List<ItemResponse>> GetAll();
    public Task<ItemResponse> GetById(int id);
    public Task<int> Add(ItemRequest item);
    public Task Update(ItemRequest item, int id);
    public Task Delete(int id);
    public Task AssignToShop(int item_id, int shop_id);
}
