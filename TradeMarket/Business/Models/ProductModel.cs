using System.Collections.Generic;

namespace Business.Models
{
    public class ProductModel
    {
        public int Id { set; get; }
        public int ProductCategoryId { set; get; }
        public string CategoryName { set; get; }
        public string ProductName { set; get; }
        public decimal Price { set; get; }
        public ICollection<int> ReceiptDetailIds { set; get; }
        
    }
}