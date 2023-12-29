using Application.DTOs.Responses;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public class UserCacheService : IUserCacheService
{
    private readonly IUserCacheRepository _userRepository;
    private readonly IMapper _mapper;

    public UserCacheService(IUserCacheRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<int> SaveUser(UserCacheEntity user)
    {
        return await _userRepository.Create(user)
                ?? throw new CacheException("Failed to cache the user.");
    }

    public async Task<UserCacheResponse> GetById(int id)
    {
        var result = await _userRepository.GetById(id)
            ?? throw new NotFoundException();

        return _mapper.Map<UserCacheResponse>(result);
    }

    public async Task<UserCacheResponse> GetByClientId(int id)
    {
        var result = await _userRepository.GetByClientId(id)
            ?? throw new NotFoundException();

        return _mapper.Map<UserCacheResponse>(result);
    }
}
