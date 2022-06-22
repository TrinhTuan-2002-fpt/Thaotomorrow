using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PerfumeShop.Models
{
    public class Shippers
    {
        public int ShipperId { get; set; }
        public string Name { get; set; }
        [RegularExpression(@"^[0-9-+\s]+$",
            ErrorMessage = "Không Được Nhập Chữ")]
        public string PhoneNumber { get; set; }
        public int Status { get; set; }
        public virtual ICollection<Carts>? Carts { get; set; }
    }
}