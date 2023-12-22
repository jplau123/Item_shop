namespace ND_2023_12_19.DTOs;

public class ItemRequest
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; } = decimal.Zero;
    public int Quantity { get; set; } = 0;
}
