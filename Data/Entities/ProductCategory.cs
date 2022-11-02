using System.Collections.Generic;

namespace Data.Entities
{
    public class ProductCategory:BaseEntity
    {
        public string CategoryName { set; get; }
        public ICollection<Product> Products { set; get; }
    }
}