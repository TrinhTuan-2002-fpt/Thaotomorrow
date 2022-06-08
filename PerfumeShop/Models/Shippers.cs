using System.Collections.Generic;

namespace PerfumeShop.Models
{
    public class Shippers
    {
        public int ShipperId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int Status { get; set; }
        public virtual ICollection<Carts> Carts { get; set; }
    }
}