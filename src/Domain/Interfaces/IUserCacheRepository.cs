using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserCacheRepository
{
    public Task<UserCacheEntity?> GetById(int id);
    public Task<UserCacheEntity?> GetByClientId(int id);
    public Task<int?> Create(UserCacheEntity user);
}
