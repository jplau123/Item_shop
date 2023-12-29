using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface IUserService
{
    public Task<List<UserResponse>> GetAll();
    public Task<UserResponse> GetById(int id);
    public Task<UserResponse> Create(UserRequest request);
}
