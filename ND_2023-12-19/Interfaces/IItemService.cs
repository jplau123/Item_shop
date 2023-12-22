using ND_2023_12_19.DTOs;
using ND_2023_12_19.Entities;

namespace ND_2023_12_19.Interfaces;

public interface IItemService
{
    public Task<List<ItemEntity>> GetAll();
    public Task<ItemEntity> GetById(int id);
    public Task<int> Add(ItemRequest item);
    public Task Update(ItemRequest item, int id);
    public Task Delete(int id);
}
