namespace PerfumeShop.Models
{
    public class CartDetails
    {
        public int Amount { get; set; }
        public double Payment { get; set; }
        public int Status { get; set; }
        public int CartId { get; set; }
        public Carts Carts { get; set; }
        public int ProductId { get; set; }
        public Products Products { get; set; }
    }
}