namespace AMNSystemsERP.CL.Models.ProductionModels
{
    public class ProductOpeningRequest
    {
        public long ProductOpeningId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal OpeningQuantity { get; set; }
        public decimal OpeningPrice { get; set; }
        public long OutletId { get; set; }
    }
}