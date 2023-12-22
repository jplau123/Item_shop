using Microsoft.AspNetCore.Mvc;
using ND_2023_12_19.DTOs;
using ND_2023_12_19.Interfaces;

namespace ND_2023_12_19.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;
    private readonly IShopService _shopService;

    public ItemController(IItemService itemService, IShopService shopService)
    {
        _itemService = itemService;
        _shopService = shopService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _itemService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _itemService.GetById(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ItemRequest request)
    {
        int itemId = await _itemService.Add(request);
        return Created($"https://localhost:7136/api/Item/{itemId}", request);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit([FromBody] ItemRequest request, int id)
    {
        await _itemService.Update(request, id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _itemService.Delete(id);

        return NoContent();
    }

    [HttpPost("buy")]
    public async Task<IActionResult> Buy(int id, int quantity)
    {
        decimal finalPrice = await _shopService.Buy(id, quantity);
        return Ok(finalPrice);
    }
}
