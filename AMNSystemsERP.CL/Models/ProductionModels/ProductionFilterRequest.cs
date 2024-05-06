namespace AMNSystemsERP.CL.Models.ProductionModels
{
    public class ProductionFilterRequest
    {
        public long OrderMasterId { get; set; }
        public int ProductId { get; set; }
        public int ProductColorId { get; set; }
        public List<int> ProductColorIds { get; set; }
        public int ProductSizeId { get; set; }
        public List<int> ProductSizeIds { get; set; }
        public long OutletId { get; set; }
    }
}
