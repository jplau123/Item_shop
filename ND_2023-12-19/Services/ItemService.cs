using AutoMapper;
using ND_2023_12_19.DTOs;
using ND_2023_12_19.Entities;
using ND_2023_12_19.Exceptions;
using ND_2023_12_19.Interfaces;

namespace ND_2023_12_19.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;

    public ItemService(
        IItemRepository itemRepository, 
        IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }

    public async Task<int> Add(ItemRequest request)
    {
        ItemEntity item = _mapper.Map<ItemEntity>(request);

        if (await _itemRepository.GetCountByName(request.Name) > 0)
            throw new BadRequestException("Item already exists.");

        return await _itemRepository.Add(item)
            ?? throw new Exception("Failed to add an item.");
    }

    public async Task Delete(int id)
    {
        if(await _itemRepository.Delete(id) == 0)
            throw new NotFoundException("Item not found.");
    }

    public async Task<List<ItemEntity>> GetAll()
    {
        return await _itemRepository.GetAll();
    }

    public async Task<ItemEntity> GetById(int id)
    {
        return await _itemRepository.GetById(id) 
            ?? throw new NotFoundException("Item not found.");
    }

    public async Task Update(ItemRequest request, int id)
    {
        ItemEntity item = _mapper.Map<ItemEntity>(request);
        item.Id = id;

        if (await _itemRepository.Update(item) == 0)
            throw new NotFoundException("Item not found.");
    }
}
