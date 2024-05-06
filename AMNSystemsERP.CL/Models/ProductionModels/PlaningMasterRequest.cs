using AMNSystemsERP.CL.Models.InventoryModels;

namespace AMNSystemsERP.CL.Models.ProductionModels
{
    public class PlaningMasterRequest
    {
        public long PlaningMasterId { get; set; }
        public long ProductId { get; set; }
        public int ProductSizeId { get; set; }
        public long OrderMasterId { get; set; }
        public decimal Amount { get; set; }
        public long OutletId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public string OrderName { get; set; }

        public List<PlaningDetailRequest> PlaningDetailRequest { get; set; }
        public List<ItemRequest> Items { get; set; }
    }
}