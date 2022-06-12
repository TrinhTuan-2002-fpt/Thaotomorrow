using System;
using System.Collections.Generic;

namespace PerfumeShop.Models
{
    public class Carts
    {
        public int CartId { get; set; }
        public DateTime Oderdate {get; set;}
        public DateTime Shipdate {get; set;}
        public string? Note {get; set;}

        public Customers? Customers { get; set; }
        public int CustomerId { get; set; }
        public Shippers? Shippers { get; set; }
        public int ShipperId { get; set; }
        public virtual ICollection<CartDetails>? CartDetails { get; set; }
    }
}