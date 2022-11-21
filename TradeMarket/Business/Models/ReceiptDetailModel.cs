namespace Business.Models
{
    public class ReceiptDetailModel
    {
        public int Id { set; get; }
        public int ReceiptId { set; get; }
        public int ProductId { set; get; }
        public decimal DiscountUnitPrice { set; get; }
        public decimal UnitPrice { set; get; }
        public int Quantity { set; get; }
    }
}