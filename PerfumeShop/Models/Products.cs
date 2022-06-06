using System.Collections.Generic;

namespace PerfumeShop.Models
{
    public class Products
    {
        public int ProdcutId { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string Insence { get; set; }
        public string Img { get; set; }
        public double Price { get; set; }
        public double? Discount { get; set; }
        public int Amount { get; set; }
        public string Madein { get; set; }
        public string DebutYear { get; set; }
        public string? Note { get; set; }
        public int? Status { get; set; }
        public ProductTypes ProductType { get; set; }
        public int TypeId { get; set; }
        public Fragrant Fragrant { get; set; }
        public int FragrantId { get; set; }
        public virtual ICollection<CartDetails> CartDetails { get; set; }
    }
}