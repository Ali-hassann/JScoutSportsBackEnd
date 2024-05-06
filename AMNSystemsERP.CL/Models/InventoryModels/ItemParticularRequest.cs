namespace AMNSystemsERP.CL.Models.InventoryModels
{
    public class ItemParticularRequest
    {
        public long ItemParticularId { get; set; }
        public long ItemId { get; set; }
        public long ParticularId { get; set; }
        public decimal Price { get; set; }
        public long OutletId { get; set; }
        public string ItemName { get; set; }
        public string ItemCategoryName { get; set; }
        public string UnitName { get; set; }
        public decimal SalePrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public int TotalParticulars { get; set; }
        public string ParticularDetail { get; set; }
        public string ParticularName { get; set; }
        public string ContactNo { get; set; }
    }
}
