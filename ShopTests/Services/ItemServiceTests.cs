
using AutoMapper;
using FluentAssertions;
using Moq;
using ND_2023_12_19.DTOs;
using ND_2023_12_19.Entities;
using ND_2023_12_19.Exceptions;
using ND_2023_12_19.Interfaces;
using ND_2023_12_19.Services;

namespace ShopTests.Services;

public class ItemServiceTests
{
    private readonly Mock<IItemRepository> _itemRepositoryMock;
    private readonly IMapper _mapper;
    private readonly IItemService _itemService;

    public ItemServiceTests()
    {
        _itemRepositoryMock = new Mock<IItemRepository>();
        _mapper = new Mapper(
            new MapperConfiguration(
                cfg => cfg.AddProfile<ND_2023_12_19.Profiles.AutoMapper>())
            );
        _itemService = new ItemService(_itemRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task Add_DuplicateItem_ThrowsBadRequestException()
    {
        var request = new ItemRequest
        {
            Name = "TestItem",
            Price = 1.0m,
            Quantity = 1,
        };

        _itemRepositoryMock.Setup(repo => repo.GetCountByName(It.Is<string>(name => name == request.Name))).ReturnsAsync(1);

        // Act and Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _itemService.Add(request));

        _itemRepositoryMock.Verify(repo => repo.GetCountByName(It.Is<string>(name => name == request.Name)), Times.Once);
    }

    [Fact]
    public async Task Add_ValidItem_ReturnsItemId()
    {
        // Arrange
        var request = new ItemRequest
        {
            Name = "TestItem",
            Price = 1.0m,
            Quantity = 1,
        };

        var expectedItemId = 1;

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
        int testId = 1;

        var expectedItemEntity = new ItemEntity
        {
            Id = testId,
            Name = "TestItem",
            Price = 1.0m,
            Quantity = 1,
        };

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
        int testId = 1000;

        _itemRepositoryMock.Setup(repo => repo.GetById(It.Is<int>(id => id == testId))).ReturnsAsync((ItemEntity?)null);

        // Act
        var act = () => _itemService.GetById(testId);
        
        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        // Verify
        _itemRepositoryMock.Verify(repo => repo.GetById(It.Is<int>(id => id == testId)), Times.Once);
    }

    [Fact]
    public async Task Delete_GivenValidId_ThrowsNotFoundException()
    {
        // Arrange
        int testId = 1000;

        _itemRepositoryMock.Setup(repo => repo.Delete(It.Is<int>(id => id == testId))).ReturnsAsync(0);

        // Act
        var act = () => _itemService.GetById(testId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        // Verify
        _itemRepositoryMock.Verify(repo => repo.GetById(It.Is<int>(id => id == testId)), Times.Once);
    }

    [Fact]
    public async Task Delete_ExistingItem_DeletesSuccessfully()
    {
        // Arrange
        int testId = 1;

        _itemRepositoryMock.Setup(repo => repo.Delete(It.Is<int>(id => id == testId))).ReturnsAsync(1);

        // Act
        await _itemService.Delete(testId);

        // Verify
        _itemRepositoryMock.Verify(repo => repo.Delete(It.Is<int>(id => id == testId)), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsAllItems()
    {
        // Arrange
        var expectedItems = new List<ItemEntity>
        {
            new ItemEntity { Id = 1, Name = "Item1", Price = 1.0m, Quantity = 0 },
            new ItemEntity { Id = 2, Name = "Item2", Price = 5.5m, Quantity = 10 },
            new ItemEntity { Id = 3, Name = "Item3", Price = 6.0m, Quantity = 5 }
        };

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
        int testId = 1;

        var itemRequest = new ItemRequest { 
            Name = "ExistingItem", 
            Price = 1.0m,
            Quantity = 1
        };

        var itemEntity = new ItemEntity { 
            Id = testId, 
            Name = "ExistingItem", 
            Price = 1.0m, 
            Quantity = 1
        };

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
        int nonExistingItemId = 999;

        var itemRequest = new ItemRequest { 
            Name = "UpdatedItem", 
            Price = 2.0m,
            Quantity = 2 
        };

        var nonExistingItemEntity = new ItemEntity
        {
            Id = nonExistingItemId,
            Name = "UpdatedItem",
            Price = 2.0m,
            Quantity = 2
        };

        _itemRepositoryMock.Setup(repo => repo.Update(It.Is<ItemEntity>(
            item => item.Id == nonExistingItemId &&
                    item.Name == nonExistingItemEntity.Name &&
                    item.Price == nonExistingItemEntity.Price &&
                    item.Quantity == nonExistingItemEntity.Quantity)))
            .ReturnsAsync(0);

        // Act and Assert
        var act = () => _itemService.Update(itemRequest, nonExistingItemId);
        await act.Should().ThrowAsync<NotFoundException>();

        // Verify
        _itemRepositoryMock.Verify(repo => repo.Update(It.Is<ItemEntity>(
            item => item.Id == nonExistingItemId &&
                    item.Name == nonExistingItemEntity.Name &&
                    item.Price == nonExistingItemEntity.Price &&
                    item.Quantity == nonExistingItemEntity.Quantity)
            ), Times.Once);
    }
}
