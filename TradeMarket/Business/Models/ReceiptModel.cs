using System;
using System.Collections.Generic;
using Data.Entities;

namespace Business.Models
{
    public class ReceiptModel
    {
        public int Id { set; get; }
        public int CustomerId { set; get; }
        public DateTime OperationDate { set; get; }
        public bool IsCheckedOut { set; get; }
        public ICollection<int> ReceiptDetailsIds { set; get; }
        
    }
}