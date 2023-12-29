using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs.Requests;

namespace WebAPI.Controllers;

[Route("orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderRequest request)
    {
        int orderId = await _orderService.Create(request);

        return Created();
    }
}
