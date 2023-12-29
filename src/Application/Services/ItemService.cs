using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IShopRepository _shopRepository;
    private readonly IMapper _mapper;

    public ItemService(
        IItemRepository itemRepository,
        IShopRepository shopRepository,
        IMapper mapper)
    {
        _itemRepository = itemRepository;
        _shopRepository = shopRepository;
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

    public async Task AssignToShop(int item_id, int shop_id)
    {
        if (await _shopRepository.GetById(shop_id) is null)
            throw new NotFoundException("Shop does not exists.");

        if (await _itemRepository.UpdateShopId(item_id, shop_id) == 0)
            throw new NotFoundException("Item does not exists.");
    }

    public async Task Delete(int id)
    {
        if(await _itemRepository.Delete(id) == 0)
            throw new NotFoundException("Item not found.");
    }

    public async Task<List<ItemResponse>> GetAll()
    {
        List<ItemEntity> result = await _itemRepository.GetAll();

        List<ItemResponse> items = _mapper.Map<List<ItemResponse>>(result);

        return items;
    }

    public async Task<ItemResponse> GetById(int id)
    {
        var result = await _itemRepository.GetById(id) 
            ?? throw new NotFoundException("Item not found.");

        ItemResponse item = _mapper.Map<ItemResponse>(result);

        return item;
    }

    public async Task Update(ItemRequest request, int id)
    {
        ItemEntity item = _mapper.Map<ItemEntity>(request);
        item.Id = id;

        if (await _itemRepository.Update(item) == 0)
            throw new NotFoundException("Item not found.");
    }
}
