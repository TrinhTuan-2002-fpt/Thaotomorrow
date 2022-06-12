using System.Collections.Generic;

namespace PerfumeShop.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string City { get; set; }
        public virtual ICollection<Customers>? Customers { get; set; }
    }
}