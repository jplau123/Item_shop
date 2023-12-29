
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using System.Net;
using Domain.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Application.Interfaces;
using Application.Services;

namespace Application.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IJsonPlaceholderClient> _jsonPlaceholderClientMock;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _jsonPlaceholderClientMock = new Mock<IJsonPlaceholderClient>();
        _mapper = new Mapper(
            new MapperConfiguration(
                cfg => cfg.AddProfile<Application.Profiles.AutoMapper>())
            );
        _fixture = new Fixture();
        _userService = new UserService(_jsonPlaceholderClientMock.Object, _mapper);
    }

    [Fact]
    public async Task GetUserById_WhenUserExists_ShouldReturnUserResponse()
    {
        // Arrange
        var id = 123;
        var clientResponse = new JsonPlaceholderResult<UserEntity>
        {
            StatusCode = HttpStatusCode.OK,
            Data = new UserEntity { Id = id, Name = "John" }
        };

        _jsonPlaceholderClientMock.Setup(c => c.GetUserById(It.IsAny<int>())).ReturnsAsync(clientResponse);

        // Act
        var result = await _userService.GetUserById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(clientResponse.Data);
    }
}
