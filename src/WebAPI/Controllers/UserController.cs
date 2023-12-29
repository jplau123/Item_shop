using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _userService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _userService.GetById(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserRequest userRequest)
    {
        var result = await _userService.Create(userRequest);

        return Created($"https://jsonplaceholder.typicode.com/users/{result.Id}", result);
    }
}
