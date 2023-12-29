using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ND_2023_12_19.Controllers;

[Route("shops")]
[ApiController]
public class ShopController : ControllerBase
{
    private readonly IShopService _shopService;

    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _shopService.GetAll());
    }

    [HttpGet("with-items")]
    public async Task<IActionResult> GetAllWithItems()
    {
        return Ok(await _shopService.GetAllWithItems());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _shopService.GetById(id));
    }

    [HttpGet("{id}/with-items")]
    public async Task<IActionResult> GetWithItems(int id)
    {
        return Ok(await _shopService.GetByIdWithItems(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ShopRequest request)
    {
        int itemId = await _shopService.Add(request);
        return Created($"https://localhost:7136/shops/{itemId}", request);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit([FromBody] ShopRequest request, int id)
    {
        await _shopService.Update(request, id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _shopService.Delete(id);
        return NoContent();
    }
}
