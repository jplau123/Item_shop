using Microsoft.AspNetCore.Mvc;
using ND_2023_12_19.Clients;

namespace ND_2023_12_19.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private JsonPlaceholderClient _client;

    public UserController(JsonPlaceholderClient client)
    {
        _client = client;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return Ok(await _client.GetUsers());
    }
}
