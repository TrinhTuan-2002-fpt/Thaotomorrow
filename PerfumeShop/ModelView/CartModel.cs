namespace PerfumeShop.ModelView;

public class CartModel
{
    public int Id { get; set; }
    public double Total { get; set; }
    public int CustomerId { get; set; }
    public IEnumerable<CartItemModel>? Items { get; set; }
}

public class CartItemModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Max { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Image { get; set; } = string.Empty;
    public double Total { get; set; }
}