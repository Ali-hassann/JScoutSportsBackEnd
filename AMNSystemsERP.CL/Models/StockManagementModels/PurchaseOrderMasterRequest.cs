namespace AMNSystemsERP.CL.Models.StockManagementModels
{
    public class PurchaseOrderMasterRequest
    {
        public long PurchaseOrderMasterId { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public long ParticularId { get; set; }
        public string ParticularName { get; set; }
        public string? ReferenceNo { get; set; }
        public string? Remarks { get; set; }
        public int? Status { get; set; }
        public long? OrderMasterId { get; set; }
        public decimal TotalAmount { get; set; }
        public long OutletId { get; set; }
        public virtual List<PurchaseOrderDetailRequest> PurchaseOrderDetailRequest { get; set; }
    }
}