using System.Collections;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Customer:BaseEntity
    {
        public int PersonId { set; get; }
        public int DiscountValue { set; get; }
        public Person Person { set; get; }
        public ICollection<Receipt> Receipts { get; set; }
    }
}