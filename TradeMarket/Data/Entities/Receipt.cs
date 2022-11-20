using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Receipt:BaseEntity
    {
        public int CustomerId { set; get; }
        public DateTime OperationDate { set; get; }
        public bool IsCheckedOut { set; get; }
        public Customer Customer { set; get; }
        public ICollection<ReceiptDetail> ReceiptDetails { get; set; }
    }
}