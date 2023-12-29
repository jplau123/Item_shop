using AutoMapper;
using Application.DTOs;
using Domain.Interfaces;
using System.Net;
using Domain.Exceptions;
using Application.Interfaces;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Domain.Entities;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IJsonPlaceholderClient _client;
    private readonly IUserCacheRepository _userCacheRepository;
    private readonly IMapper _mapper;

    public UserService(
        IJsonPlaceholderClient client, 
        IMapper mapper, 
        IUserCacheRepository userCacheRepository)
    {
        _client = client;
        _mapper = mapper;
        _userCacheRepository = userCacheRepository;
    }

    public async Task<List<UserResponse>> GetAll()
    {
        JsonPlaceholderResult<List<UserEntity>> result = await _client.GetUsers();

        HandleExceptions(result);

        return _mapper.Map<List<UserResponse>>(result.Data);
    }

    public async Task<UserResponse> GetById(int id)
    {
        var cacheResult = await _userCacheRepository.GetByClientId(id);

        if (cacheResult is not null)
        {
            return new UserResponse
            {
                Id = cacheResult.ClientId,
                Name = cacheResult.Name,
            };
        }

        JsonPlaceholderResult<UserEntity> result = await _client.GetUserById(id);
        HandleExceptions(result);

        try
        {
            var userCacheEntity = new UserCacheEntity
            {
                ClientId = result.Data!.Id,
                Name = result.Data!.Name
            };

            var cachedId = await _userCacheRepository.Create(userCacheEntity)
                ?? throw new CacheException("Failed to cache the user.");
        }
        catch (CacheException)
        {
            // TODO Log the exception
            // ...

            throw;
        }

        return _mapper.Map<UserResponse>(result.Data);
    }

    public async Task<UserResponse> Create(UserRequest request)
    {
        UserEntity user = _mapper.Map<UserEntity>(request);

        JsonPlaceholderResult<UserEntity> clietResult = await _client.CreateUser(user);

        HandleExceptions(clietResult);

        var cacheResult = await _userCacheRepository.GetByClientId(clietResult.Data!.Id);

        if(cacheResult is null)
        {
            try
            {
                var userCacheEntity = new UserCacheEntity
                {
                    ClientId = clietResult.Data!.Id,
                    Name = clietResult.Data!.Name
                };

                var cachedId = await _userCacheRepository.Create(userCacheEntity)
                    ?? throw new CacheException("Failed to cache the user.");
            }
            catch (CacheException)
            {
                // TODO Log the exception
                // ...

                throw;
            }
        }

        return _mapper.Map<UserResponse>(clietResult.Data);
    }

    private static void HandleExceptions<T>(JsonPlaceholderResult<T> jsonPlaceholderResult) where T : class
    {
        if (jsonPlaceholderResult.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException(jsonPlaceholderResult.ErrorMessage);
        }
        else if (jsonPlaceholderResult.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new BadRequestException(jsonPlaceholderResult.ErrorMessage);
        }
        else if ((int)jsonPlaceholderResult.StatusCode >= 400)
        {
            throw new Exception(jsonPlaceholderResult.ErrorMessage);
        }
    }
}
