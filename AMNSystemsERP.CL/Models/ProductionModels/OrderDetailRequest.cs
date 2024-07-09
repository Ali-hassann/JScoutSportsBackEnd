namespace AMNSystemsERP.CL.Models.ProductionModels
{
    public class OrderDetailRequest
    {
        public long OrderDetailId { get; set; }
        public long OrderMasterId { get; set; }
        public long ProductId { get; set; }
        public int ProductSizeId { get; set; }
        public string ProductSizeName { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public string UnitName { get; set; }
    }
}