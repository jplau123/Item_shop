namespace Domain.Entities;

public class UserCacheEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ClientId { get; set; }
}
