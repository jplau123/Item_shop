using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repositories;

public class ShopRepository : IShopRepository
{
    private readonly IDbConnection _connection;

    public ShopRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int?> Add(ShopEntity shop)
    {
        string sql = "INSERT INTO shops (name, address) VALUES (@shopName, @shopAddress) RETURNING @id";

        var parameters = new
        {
            shopName = shop.Name,
            shopAddress = shop.Address,
        };

        return await _connection.QueryFirstOrDefaultAsync<int?>(sql, parameters);
    }

    public async Task<int> Delete(int id)
    {
        string sql = "DELETE FROM shops WHERE id = @shopId";

        return await _connection.ExecuteAsync(sql, new { shopId = id });
    }

    public async Task<List<ShopEntity>> GetAll()
    {
        string sql = "SELECT id, name, address FROM shops";

        var result = await _connection.QueryAsync<ShopEntity>(sql);

        return result.ToList();
    }

    public async Task<List<ShopEntity>> GetAllWithItems()
    {
        string sql = "SELECT s.id, s.name, s.address, i.id, i.name, i.price, i.quantity FROM shops AS s LEFT JOIN items AS i ON s.id = i.shop_id";

        var result = await _connection.QueryAsync<ShopEntity, ItemEntity, ShopEntity>(sql, (shop, item) => {
            if (item is not null)
                shop.Items.Add(item);
            return shop;
        });

        var groupedResults = result.GroupBy(shop => shop.Id).Select(group =>
        {
            var shop = group.First();
            shop.Items = group.SelectMany(g => g.Items).ToList();
            return shop;
        });

        return groupedResults.ToList();
    }

    public async Task<ShopEntity?> GetById(int id)
    {
        string sql = "SELECT id, name, address FROM shops WHERE id = @shopId";

        return await _connection.QueryFirstOrDefaultAsync<ShopEntity?>(sql, new { shopId = id });
    }

    public async Task<ShopEntity?> GetByIdWithItems(int id)
    {
        string sql = "SELECT s.id, s.name, s.address, i.id, i.name, i.price, i.quantity FROM shops AS s LEFT JOIN items AS i ON s.id = i.shop_id WHERE s.id = @shopId";

        var result = await _connection.QueryAsync<ShopEntity, ItemEntity, ShopEntity>(sql, (shop, item) => {
            if (item is not null)
                shop.Items.Add(item);
            return shop;
        }, new { shopId = id });

        var groupedResults = result.GroupBy(shop => shop.Id).Select(group =>
        {
            var shop = group.First();
            shop.Items = group.SelectMany(g => g.Items).ToList();
            return shop;
        });

        return groupedResults.FirstOrDefault();
    }

    public Task<int> GetCountByName(string name)
    {
        string sql = "SELECT COUNT(*) FROM shops WHERE name = @shopName LIMIT 1";

        return _connection.QueryFirstAsync<int>(sql, new { shopName = name });
    }

    public Task<int> Update(ShopEntity shop)
    {
        string sql = "UPDATE shops SET name = @shopName, address = @shopAddress WHERE id = @shopId";

        var parameters = new
        {
            shopId = shop.Id,
            shopName = shop.Name,
            shopAddress = shop.Address,
        };

        return _connection.ExecuteAsync(sql, parameters);
    }
}
