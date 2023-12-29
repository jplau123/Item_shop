using Domain.Entities;

namespace Domain.Interfaces;

public interface IItemRepository
{
    public Task<List<ItemEntity>> GetAll();
    public Task<ItemEntity?> GetById(int id);
    public Task<int?> Add(ItemEntity item);
    public Task<int> Update(ItemEntity item);
    public Task<int> Delete(int id);
    public Task<int> GetCountByName(string name);
    public Task<int> UpdateShopId(int item_id, int shop_id);
}
