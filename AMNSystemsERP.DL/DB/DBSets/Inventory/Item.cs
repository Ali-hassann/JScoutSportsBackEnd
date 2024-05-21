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
        public string ItemName { get; set; }
        public string? PartNo { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? GSM { get; set; }
        public int RackNo { get; set; }
        public string? RowNo { get; set; }
        public string? BinNo { get; set; }
        public string? LineNumber { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal ReorderLevel { get; set; }
        public bool IsActive { get; set; }
        public long OutletId { get; set; }

        public virtual BundleDetail PackageDetail { get; set; }
    }
}