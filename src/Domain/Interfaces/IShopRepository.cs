
using Domain.Entities;

namespace Domain.Interfaces;

public interface IShopRepository
{
    public Task<List<ShopEntity>> GetAll();
    public Task<ShopEntity?> GetById(int id);
    public Task<int?> Add(ShopEntity shop);
    public Task<int> Update(ShopEntity shop);
    public Task<int> Delete(int id);
    public Task<int> GetCountByName(string name);
    public Task<List<ShopEntity>> GetAllWithItems();
    public Task<ShopEntity?> GetByIdWithItems(int id);
}
