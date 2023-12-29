using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repositories;

public class UserCacheRepository : IUserCacheRepository
{
    private readonly IDbConnection _connection;

    public UserCacheRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int?> Create(UserCacheEntity user)
    {
        string sql = "INSERT INTO users (name, client_id) VALUES (@userName, @clientId) RETURNING @id";

        return await _connection.QueryFirstOrDefaultAsync<int?>(sql, new { userName = user.Name, clientId = user.ClientId});
    }

    public async Task<UserCacheEntity?> GetById(int id)
    {
        string sql = "SELECT id, client_id AS clientId, name FROM users WHERE id = @userId";

        return await _connection.QueryFirstOrDefaultAsync<UserCacheEntity?>(sql, new { userId = id });
    }
    
    public async Task<UserCacheEntity?> GetByClientId(int id)
    {
        string sql = "SELECT id, client_id AS clientId, name FROM users WHERE client_id = @userClientId";

        return await _connection.QueryFirstOrDefaultAsync<UserCacheEntity?>(sql, new { userClientId = id });
    }
}
