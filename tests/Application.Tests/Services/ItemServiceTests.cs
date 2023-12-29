
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Services;
using AutoFixture;
using AutoFixture.Xunit2;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Application.Tests.Services;

public class ItemServiceTests
{
    private readonly Mock<IItemRepository> _itemRepositoryMock;
    private readonly IMapper _mapper;
    private readonly IItemService _itemService;
    private readonly Fixture _fixture;

    public ItemServiceTests()
    {
        _itemRepositoryMock = new Mock<IItemRepository>();
        _mapper = new Mapper(
            new MapperConfiguration(
                cfg => cfg.AddProfile<Application.Profiles.AutoMapper>())
            );
        _itemService = new ItemService(_itemRepositoryMock.Object, _mapper);
        _fixture = new Fixture();
    }

    [Theory]
    [AutoData]
    public async Task Add_DuplicateItem_ThrowsBadRequestException(ItemRequest request)
    {
        _itemRepositoryMock.Setup(repo => repo.GetCountByName(request.Name)).ReturnsAsync(1);

        // Act and Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _itemService.Add(request));

        _itemRepositoryMock.Verify(repo => repo.GetCountByName(request.Name), Times.Once);
    }

    [Theory]
    [AutoData]
    public async Task Add_ValidItem_ReturnsItemId(ItemRequest request)
    {
        var expectedItemId = _fixture.Create<int>();

        _itemRepositoryMock.Setup(repo => repo.Add(It.Is<ItemEntity>(
            item => item.Name == request.Name &&
                    item.Price == request.Price &&
                    item.Quantity == request.Quantity)))
            .ReturnsAsync(expectedItemId);

        _itemRepositoryMock.Setup(repo => repo.GetCountByName(request.Name)).ReturnsAsync(0);

        // Act
        var result = await _itemService.Add(request);

        // Assert
        result.Should().Be(expectedItemId);

        _itemRepositoryMock.Verify(repo => repo.Add(It.Is<ItemEntity>(
            item => item.Name == request.Name &&
                    item.Price == request.Price &&
                    item.Quantity == request.Quantity)), Times.Once);
        _itemRepositoryMock.Verify(repo => repo.GetCountByName(request.Name), Times.Once);
    }

    [Fact]
    public async Task GetById_GivenValidId_ReturnsItemEntity()
    {
        // Arrange
        int testId = _fixture.Create<int>();

        var expectedItemEntity = _fixture.Build<ItemEntity>().With(item => item.Id, testId).Create();

        _itemRepositoryMock.Setup(repo => repo.GetById(testId)).ReturnsAsync(expectedItemEntity);

        // Act
        var result = await _itemService.GetById(testId);

        result.Should().BeEquivalentTo(expectedItemEntity);

        // Assert
        _itemRepositoryMock.Verify(repo => repo.GetById(testId), Times.Once);
    }

    [Fact]
    public async Task GetById_GivenValidId_ThrowsNotFoundException()
    {
        // Arrange
        int testId = _fixture.Create<int>();

        _itemRepositoryMock.Setup(repo => repo.GetById(testId)).ReturnsAsync((ItemEntity?)null);

        // Act
        var act = () => _itemService.GetById(testId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        // Verify
        _itemRepositoryMock.Verify(repo => repo.GetById(testId), Times.Once);
    }

    [Fact]
    public async Task Delete_GivenValidId_ThrowsNotFoundException()
    {
        // Arrange
        int testId = _fixture.Create<int>();

        _itemRepositoryMock.Setup(repo => repo.Delete(testId)).ReturnsAsync(0);

        // Act
        var act = () => _itemService.GetById(testId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        // Verify
        _itemRepositoryMock.Verify(repo => repo.GetById(testId), Times.Once);
    }

    [Fact]
    public async Task Delete_ExistingItem_DeletesSuccessfully()
    {
        // Arrange
        int testId = _fixture.Create<int>();

        _itemRepositoryMock.Setup(repo => repo.Delete(testId)).ReturnsAsync(1);

        // Act
        await _itemService.Delete(testId);

        // Verify
        _itemRepositoryMock.Verify(repo => repo.Delete(testId), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsAllItems()
    {
        // Arrange
        List<ItemEntity> expectedItems = _fixture.CreateMany<ItemEntity>(count: 5).ToList();

        _itemRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(expectedItems);

        // Act
        var result = await _itemService.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedItems);

        // Verify
        _itemRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyListWhenNoItems()
    {
        // Arrange
        _itemRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync([]);

        // Act
        var result = await _itemService.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        // Verify
        _itemRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
    }

    [Fact]
    public async Task Update_ValidItem_UpdatesSuccessfully()
    {
        // Arrange
        int testId = _fixture.Create<int>();

        var itemRequest = _fixture.Create<ItemRequest>();

        var itemEntity = _fixture.Build<ItemEntity>()
            .With(item => item.Id, testId)
            .With(item => item.Name, itemRequest.Name)
            .With(item => item.Price, itemRequest.Price)
            .With(item => item.Quantity, itemRequest.Quantity)
            .Create();

        _itemRepositoryMock.Setup(repo => repo.Update(It.Is<ItemEntity>(
            item => item.Id == testId &&
                    item.Name == itemEntity.Name &&
                    item.Price == itemEntity.Price &&
                    item.Quantity == itemEntity.Quantity))).ReturnsAsync(1);

        // Act
        await _itemService.Update(itemRequest, testId);

        // Verify
        _itemRepositoryMock.Verify(repo => repo.Update(It.Is<ItemEntity>(
            item => item.Id == testId &&
                    item.Name == itemEntity.Name &&
                    item.Price == itemEntity.Price &&
                    item.Quantity == itemEntity.Quantity)
            ), Times.Once);
    }

    [Fact]
    public async Task Update_NonExistingItem_ThrowsNotFoundException()
    {
        // Arrange
        int testId = _fixture.Create<int>();

        var itemRequest = _fixture.Create<ItemRequest>();

        var itemEntity = _fixture.Build<ItemEntity>()
            .With(item => item.Id, testId)
            .With(item => item.Name, itemRequest.Name)
            .With(item => item.Price, itemRequest.Price)
            .With(item => item.Quantity, itemRequest.Quantity)
            .Create();

        _itemRepositoryMock.Setup(repo => repo.Update(It.Is<ItemEntity>(
            item => item.Id == testId &&
                    item.Name == itemEntity.Name &&
                    item.Price == itemEntity.Price &&
                    item.Quantity == itemEntity.Quantity)))
            .ReturnsAsync(0);

        // Act and Assert
        var act = () => _itemService.Update(itemRequest, testId);
        await act.Should().ThrowAsync<NotFoundException>();

        // Verify
        _itemRepositoryMock.Verify(repo => repo.Update(It.Is<ItemEntity>(
            item => item.Id == testId &&
                    item.Name == itemEntity.Name &&
                    item.Price == itemEntity.Price &&
                    item.Quantity == itemEntity.Quantity)
            ), Times.Once);
    }
}
