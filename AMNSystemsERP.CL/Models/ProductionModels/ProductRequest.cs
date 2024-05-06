namespace AMNSystemsERP.CL.Models.ProductionModels
{
    public class ProductRequest
    {
        public long ProductId { get; set; }
        public long ProductCategoryId { get; set; }
        public string ProductName { get; set; }
        public string? PartNo { get; set; }
        public string? Color { get; set; }
        public long UnitId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public bool IsActive { get; set; }
        public long OutletId { get; set; }
        public string UnitName { get; set; }
        public string ProductCategoryName { get; set; }
    }
}