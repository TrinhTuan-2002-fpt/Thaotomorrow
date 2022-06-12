using System.Collections.Generic;

namespace PerfumeShop.Models
{
    public class Roles
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Accounts>? Account { get; set; }
    }
}