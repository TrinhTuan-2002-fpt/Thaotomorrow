using System.Collections.Generic;

namespace PerfumeShop.Models
{
    public class Fragrant
    {
        public int FragrantId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Products> products { get; set; }
    }
}