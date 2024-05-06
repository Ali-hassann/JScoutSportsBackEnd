using AMNSystemsERP.DL.DB.DBSets.Inventory;
using Inventory.DL.DB.DBSets.StockManagement;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.StockManagement
{
    public class PurchaseOrderDetail
    {
        [Key]
        public long PurchaseOrderDetailId { get; set; }
        public long PurchaseOrderMasterId { get; set; }
        public long ItemId { get; set; }
        public string? BarCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }

        public virtual PurchaseOrderMaster PurchaseOrderMaster { get; set; }
        public virtual Item Item { get; set; }
    }
}