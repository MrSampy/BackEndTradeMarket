using System.Collections.Generic;

namespace Business.Models
{
    public class ProductCategoryModel
    {
        public int Id { set; get; }
        public string CategoryName { set; get; }
        public ICollection<int> ProductIds { set; get; }

    }
}