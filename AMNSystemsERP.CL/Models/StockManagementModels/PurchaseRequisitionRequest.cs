using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.StockManagementModels
{
	public class PurchaseRequisitionMasterRequest
	{
		public long PurchaseRequisitionMasterId { get; set; }

		public DateTime PurchaseRequisitionDate { get; set; }

		public string ReferenceNo { get; set; }

		public string Remarks { get; set; }

		public int Status { get; set; }

		public long ProjectsId { get; set; }

		public virtual List<PurchaseRequisitionDetailRequest> PurchaseRequisitionDetailRequest { get; set; }
	}

	public class PurchaseRequisitionDetailRequest
	{
		public long PurchaseRequisitionDetailId { get; set; }

		public long PurchaseRequisitionMasterId { get; set; }

		public long ItemId { get; set; }

		public string ItemName { get; set; }

		public string UnitName { get; set; }

		public string CategoryName { get; set; }

		public string TypeName { get; set; }

		public string BarCode { get; set; }

		public decimal Quantity { get; set; }
	}
}