namespace PerfumeShop.Models
{
    public class Accounts
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Gender { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public int Status { get; set; }
        public Roles? Roles { get; set; }
        public int RoleId { get; set; }
    }
}
