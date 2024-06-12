using AMNSystemsERP.DL.DB.Base;
using AMNSystemsERP.DL.DB.DBSets.Inventory;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.StockManagement
{
    public class PurchaseRequisitionMaster : Entity
    {
        public PurchaseRequisitionMaster()
        {
            PurchaseRequisitionDetail = new HashSet<PurchaseRequisitionDetail>();
        }

        [Key]
        public long PurchaseRequisitionMasterId { get; set; }
        [Required]
        public DateTime PurchaseRequisitionDate { get; set; }
        public string ReferenceNo { get; set; }  
        public string Remarks { get; set; }
        public int Status { get; set; }

        public virtual ICollection<PurchaseRequisitionDetail> PurchaseRequisitionDetail { get; set; }
    }

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

		public virtual PurchaseRequisitionMaster PurchaseRequisitionMaster { get; set; }
		public virtual Item Item { get; set; }
	}
}
