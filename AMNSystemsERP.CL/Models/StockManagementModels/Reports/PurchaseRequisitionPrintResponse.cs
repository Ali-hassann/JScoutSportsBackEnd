namespace AMNSystemsERP.CL.Models.StockManagementModels.Reports
{
    public class PurchaseRequisitionPrintResponse
    {
        public long PurchaseRequisitionMasterId { get; set; }
        public DateTime PurchaseRequisitionDate { get; set; }
        public string ReferenceNo { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public long ProjectsId { get; set; }

        /********* detail section **************/
        public long PurchaseRequisitionDetailId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string BarCode { get; set; }
        public decimal Quantity { get; set; }

    }
}
