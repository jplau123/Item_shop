using Application.DTOs.Responses;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserCacheService
{
    public Task<int> SaveUser(UserCacheEntity user);
    public Task<UserCacheResponse> GetById(int id);
    public Task<UserCacheResponse> GetByClientId(int id);
}
