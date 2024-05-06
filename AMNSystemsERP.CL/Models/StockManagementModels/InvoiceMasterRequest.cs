using AMNSystemsERP.CL.Enums;
using AMNSystemsERP.CL.Enums.InventoryEnums;

namespace AMNSystemsERP.CL.Models.StockManagementModels
{
    public class InvoiceMasterRequest
    {
        public long InvoiceMasterId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long ParticularId { get; set; }
        public string ParticularName { get; set; }
        public string ReferenceNo { get; set; }
        public InventoryDocumentType DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public PaymentMode PaymentMode { get; set; }
        public string Remarks { get; set; }
        public int InvoiceStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidReceivedAmount { get; set; } 
        public decimal SaleTax { get; set; }
        public decimal BalanceAmount { get; set; }
        public long OutletId { get; set; }
        public long? OrderMasterId { get; set; }
        public string InvoiceSerialNo { get; set; }
        public long SerialIndex { get; set; }
        public virtual List<InvoiceDetailRequest> InvoiceDetailsRequest { get; set; }
    }
}
