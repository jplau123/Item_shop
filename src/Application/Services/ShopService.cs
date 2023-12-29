using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public class ShopService : IShopService
{
    private readonly IShopRepository _shopRepository;
    private readonly IMapper _mapper;

    public ShopService(IShopRepository shopRepository, IMapper mapper)
    {
        _shopRepository = shopRepository;
        _mapper = mapper;
    }

    public async Task<int> Add(ShopRequest request)
    {
        ShopEntity shop = _mapper.Map<ShopEntity>(request);

        if (await _shopRepository.GetCountByName(request.Name) > 0)
            throw new BadRequestException("Shop already exists.");

        return await _shopRepository.Add(shop)
            ?? throw new Exception("Failed to add a shop.");
    }

    public async Task Delete(int id)
    {
        if (await _shopRepository.Delete(id) == 0)
            throw new NotFoundException("Shop not found.");
    }

    public async Task<List<ShopResponse>> GetAll()
    {
        List<ShopEntity> result = await _shopRepository.GetAll();

        List<ShopResponse> shops = _mapper.Map<List<ShopResponse>>(result);

        return shops;
    }

    public async Task<List<ShopWithItemsResponse>> GetAllWithItems()
    {
        List<ShopEntity> result = await _shopRepository.GetAllWithItems();

        List<ShopWithItemsResponse> shops = _mapper.Map<List<ShopWithItemsResponse>>(result);

        return shops;
    }

    public async Task<ShopResponse> GetById(int id)
    {
        ShopEntity result = await _shopRepository.GetById(id)
            ?? throw new NotFoundException("Shop not found.");

        ShopResponse shop = _mapper.Map<ShopResponse>(result);

        return shop;
    }
    
    public async Task<ShopWithItemsResponse> GetByIdWithItems(int id)
    {
        ShopEntity result = await _shopRepository.GetByIdWithItems(id)
            ?? throw new NotFoundException("Shop not found.");

        ShopWithItemsResponse shop = _mapper.Map<ShopWithItemsResponse>(result);

        return shop;
    }

    public async Task Update(ShopRequest request, int id)
    {
        ShopEntity shop = _mapper.Map<ShopEntity>(request);
        shop.Id = id;

        if (await _shopRepository.Update(shop) == 0)
            throw new NotFoundException("Shop not found.");
    }
}
