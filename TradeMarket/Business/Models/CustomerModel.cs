using System;
using System.Collections.Generic;

namespace Business.Models
{
    public class CustomerModel
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Surname { set; get; }
        public DateTime BirthDate { set; get; }
        public int DiscountValue { set; get; }
        public ICollection<int> ReceiptsIds { set; get; }
    }
}