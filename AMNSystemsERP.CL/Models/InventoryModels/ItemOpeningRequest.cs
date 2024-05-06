namespace AMNSystemsERP.CL.Models.InventoryModels
{
    public class ItemOpeningRequest
    {
        public long ItemOpeningId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string ItemCategoryName { get; set; }
        public decimal OpeningQuantity { get; set; }
        public decimal OpeningPrice { get; set; }
        public long OutletId { get; set; }
    }
}