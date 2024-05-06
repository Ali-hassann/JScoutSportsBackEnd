using AMNSystemsERP.DL.DB.Base;
using AMNSystemsERP.DL.DB.DBSets.Inventory;
using AMNSystemsERP.DL.DB.DBSets.StockManagement;
using System.ComponentModel.DataAnnotations;

namespace Inventory.DL.DB.DBSets.StockManagement
{
    public class PurchaseOrderMaster : Entity
    {
        public PurchaseOrderMaster()
        {
            PurchaseOrderDetail = new HashSet<PurchaseOrderDetail>();
        }

        [Key]
        public long PurchaseOrderMasterId { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public long ParticularId { get; set; }
        public string? ReferenceNo { get; set; }
        public string? Remarks { get; set; }
        public int? Status { get; set; }
        public long? OrderMasterId { get; set; }
        public long OutletId { get; set; }
        public decimal TotalAmount { get; set; }

        public virtual Particular Particular { get; set; }
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }
    }
}