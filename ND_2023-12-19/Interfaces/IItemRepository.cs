using ND_2023_12_19.Entities;

namespace ND_2023_12_19.Interfaces;

public interface IItemRepository
{
    public Task<List<ItemEntity>> GetAll();
    public Task<ItemEntity?> GetById(int id);
    public Task<int?> Add(ItemEntity item);
    public Task<int> Update(ItemEntity item);
    public Task<int> Delete(int id);
    public Task<int> GetCountByName(string name);
}
