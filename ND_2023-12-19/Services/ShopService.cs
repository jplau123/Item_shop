using ND_2023_12_19.Entities;
using ND_2023_12_19.Exceptions;
using ND_2023_12_19.Interfaces;

namespace ND_2023_12_19.Services;

public class ShopService : IShopService
{
    private readonly IItemService _itemService;
    private readonly IItemRepository _itemRepository;

    public ShopService(
        IItemService itemService, 
        IItemRepository itemRespository)
    {
        _itemService = itemService;
        _itemRepository = itemRespository;
    }

    public decimal ApplyDiscount(decimal price, int quantity)
    {
        if(quantity >= 10)
        {
            return quantity * price * 0.90M;
        } else if(quantity >= 20) {
            return quantity * price * 0.80M;
        }
        return quantity * price;
    }

    public async Task<decimal> Buy(int id, int quantity)
    {
        ItemEntity item = await _itemService.GetById(id);

        return ApplyDiscount(item.Price, quantity);
    }
}
