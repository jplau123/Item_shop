using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IDbConnection _connection;

    public OrderRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int?> Add(OrderEntity order)
    {
        string sql = "INSERT INTO orders (user_id, item_id) VALUES (@userId, @itemId) RETURNING @id";

        var parameters = new
        {
            userId = order.UserId,
            itemId = order.ItemId,
        };

        return await _connection.QueryFirstOrDefaultAsync<int?>(sql, parameters);
    }
}
