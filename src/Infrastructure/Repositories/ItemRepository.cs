using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly IDbConnection _connection;

    public ItemRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int?> Add(ItemEntity item)
    {
        string sql = "INSERT INTO items (name, price, quantity) VALUES (@itemName, @itemPrice, @itemQuantity) RETURNING @id";

        var parameters = new
        {
            itemName = item.Name,
            itemPrice = item.Price,
            itemQuantity = item.Quantity
        };

        return await _connection.QueryFirstOrDefaultAsync<int?>(sql, parameters);
    }

    public Task<int> GetCountByName(string name)
    {
        string sql = "SELECT COUNT(*) FROM items WHERE name = @itemName LIMIT 1";

        return _connection.QueryFirstAsync<int>(sql, new { itemName = name });
    }

    public async Task<int> Delete(int id)
    {
        string sql = "DELETE FROM items WHERE id = @itemId";

        return await _connection.ExecuteAsync(sql, new { itemId = id });
    }

    public async Task<List<ItemEntity>> GetAll()
    {
        string sql = "SELECT id, name, price, quantity FROM items";

        var result = await _connection.QueryAsync<ItemEntity>(sql);

        return result.ToList();
    }

    public Task<ItemEntity?> GetById(int id)
    {
        string sql = "SELECT id, name, price, quantity FROM items WHERE id = @itemId";

        return _connection.QueryFirstOrDefaultAsync<ItemEntity?>(sql, new { itemId = id });
    }

    public Task<int> Update(ItemEntity item)
    {
        string sql = "UPDATE items SET name = @itemName, price = @itemPrice, quantity = @itemQuantity WHERE id = @itemId";

        var parameters = new
        {
            itemName = item.Name,
            itemPrice = item.Price,
            itemQuantity = item.Quantity,
            id = item.Id
        };

        return _connection.ExecuteAsync(sql, parameters);
    }

    public Task<int> UpdateShopId(int item_id, int shop_id)
    {
        string sql = "UPDATE items SET shop_id = @shopId WHERE id = @itemId";

        var parameters = new
        {
            shopId = shop_id,
            itemId = item_id
        };

        return _connection.ExecuteAsync(sql, parameters);
    }
}
