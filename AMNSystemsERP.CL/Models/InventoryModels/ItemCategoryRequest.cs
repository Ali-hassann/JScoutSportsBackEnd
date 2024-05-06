namespace AMNSystemsERP.CL.Models.InventoryModels
{
    public class ItemCategoryRequest
    {
        public long ItemCategoryId { get; set; }
        public long ItemTypeId { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemCategoryName { get; set; }
        public long OutletId { get; set; }
    }
}
