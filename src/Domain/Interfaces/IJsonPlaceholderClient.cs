using Application.DTOs;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IJsonPlaceholderClient
{
    public Task<JsonPlaceholderResult<List<UserEntity>>> GetUsers();
    public Task<JsonPlaceholderResult<UserEntity>> GetUserById(int id);
    public Task<JsonPlaceholderResult<UserEntity>> CreateUser(UserEntity user);
}
