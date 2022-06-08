using System.Collections.Generic;

namespace PerfumeShop.Models
{
    public class Customers
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Gender { get; set; }
        public string PhoneNumber { get; set; }
        public int Status { get; set; }
        public ICollection<Carts> Carts { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}