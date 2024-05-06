namespace AMNSystemsERP.CL.Models.InventoryModels
{
    public class BundleRequest
    {
        public long BundleId { get; set; }
        public string BundleName { get; set; }
        public string Description { get; set; }
        public long OutletId { get; set; }
        public int ItemCount { get; set; }

        public List<ItemRequest> Items { get; set; }
    }
}