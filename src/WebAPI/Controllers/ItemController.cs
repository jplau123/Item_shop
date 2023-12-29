using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs.Requests;

namespace WebAPI.Controllers;

[Route("items")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
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
        return Created($"https://localhost:7136/items/{itemId}", request);
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

    [HttpPut("{item_id}/assign-to-shop")]
    public async Task<IActionResult> AssignToShop(int item_id, [FromBody] int shop_id)
    {
        await _itemService.AssignToShop(item_id, shop_id);

        return NoContent();
    }
}
