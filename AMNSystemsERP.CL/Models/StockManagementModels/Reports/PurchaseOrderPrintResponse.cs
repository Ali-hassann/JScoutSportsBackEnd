namespace AMNSystemsERP.CL.Models.StockManagementModels.Reports
{
    public class PurchaseOrderPrintResponse
    {
        public long PurchaseOrderMasterId { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public long VendorId { get; set; }
        public string VendorName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string ReferenceNo { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public long ProjectsId { get; set; }
        public decimal TotalAmount { get; set; }

        /********* detail section **************/
        public long PurchaseOrderDetailId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string BarCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }

    }
}
