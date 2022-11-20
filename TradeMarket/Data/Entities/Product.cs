using System.Collections.Generic;

namespace Data.Entities
{
    public class Product:BaseEntity
    {
        public int ProductCategoryId { set; get; }
        public string ProductName { set; get; }
        public decimal Price { set; get; }
        public ProductCategory Category { set; get; }
        public ICollection<ReceiptDetail> ReceiptDetails { get; set; }
        
    }
}