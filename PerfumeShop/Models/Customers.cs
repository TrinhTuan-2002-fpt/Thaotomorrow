using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PerfumeShop.Models
{
    public class Customers
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        [RegularExpression(@"^(.+)@(.+)$",
            ErrorMessage = "Hãy nhập email đúng định dạng")]
        public string Email { get; set; }
        [RegularExpression(@"^[a-z0-9.@#$%&]{6,12}$",
            ErrorMessage = "Hãy Pass đúng định dạng sau: " +
                           "\n Chứa ít nhất 6 kí tự, tối đa 12 kí tự" +
                           "\n Không có chữ hoa.")]
        public string Password { get; set; }
        public bool? Gender { get; set; }
        [RegularExpression(@"^[0-9-+\s]+$",
            ErrorMessage = "Không Được Nhập Chữ")]
        public string PhoneNumber { get; set; }
        public int? Status { get; set; }
        public ICollection<Carts>? Carts { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
    }
}