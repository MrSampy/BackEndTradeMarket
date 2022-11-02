namespace Data.Entities
{
    public class ReceiptDetail:BaseEntity
    {
        public int ReceiptId { set; get; }
        public int ProductId { set; get; }
        public decimal DiscountUnitPrice { set; get; }
        public decimal UnitPrice { set; get; }
        public int Quantity { set; get; }
        public Receipt Receipt { set; get; }
        public Product Product { set; get; }
    }
}