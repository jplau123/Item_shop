
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs.Requests;

public class ShopRequest
{
    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
}
