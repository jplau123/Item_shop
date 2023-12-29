
using Domain.Entities;

namespace Application.DTOs.Responses;

public class ShopResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
