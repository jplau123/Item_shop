namespace Application.DTOs.Responses;

public class ShopWithItemsResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<ItemResponse> Items { get; set; } = [];
}
