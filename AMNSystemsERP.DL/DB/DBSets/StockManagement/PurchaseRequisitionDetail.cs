using AMNSystemsERP.DL.DB.DBSets.Inventory;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.StockManagement
{

    public class PurchaseRequisitionDetail
    {
        [Key]
        public long PurchaseRequisitionDetailId { get; set; }
        
        [Required]
        public long PurchaseRequisitionMasterId { get; set; }

        [Required]
        public long ItemId { get; set; }
        public string BarCode { get; set; }

        [Required]
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }

        public virtual PurchaseRequisitionMaster PurchaseRequisitionMaster { get; set; }
        public virtual Item Item { get; set; }
    }
}