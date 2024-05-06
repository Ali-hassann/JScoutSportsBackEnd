using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Inventory
{
    public class Item : Entity
    {
        [Key]
        public long ItemId { get; set; }
        public long ItemCategoryId { get; set; }
        public int UnitId { get; set; }
        [MaxLength(80)]
        public string ItemName { get; set; } = string.Empty;
        public string PartNo { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal ReorderLevel { get; set; }
        public bool IsActive { get; set; }
        public long OutletId { get; set; }

        public virtual BundleDetail PackageDetail { get; set; }
    }
}