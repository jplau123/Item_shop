namespace ND_2023_12_19.Interfaces;

public interface IShopService
{
    public Task<decimal> Buy(int id, int quantity);
    public decimal ApplyDiscount(decimal price, int quantity);
}
