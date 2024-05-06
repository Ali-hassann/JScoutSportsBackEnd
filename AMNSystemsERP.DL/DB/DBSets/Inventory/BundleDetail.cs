using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Inventory
{
    public class BundleDetail
    {
        [Key]
        public long BundleDetailId { get; set; }
        public long ItemId { get; set; }
        public long BundleId { get; set; }
        public decimal Quantity { get; set; }

        public virtual Bundle Bundle { get; set; }
        public virtual Item Item { get; set; }
    }
}
