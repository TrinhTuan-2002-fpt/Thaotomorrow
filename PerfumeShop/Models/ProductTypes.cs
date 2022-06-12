using System.Collections.Generic;

namespace PerfumeShop.Models
{
    public class ProductTypes
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Products>? products { get; set; }
    }
}