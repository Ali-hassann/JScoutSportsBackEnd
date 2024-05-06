using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.StockManagement
{
    public class InvoiceMaster : Entity
    {
        public InvoiceMaster()
        {
            InvoiceDetail = new HashSet<InvoiceDetail>();
        }

        [Key]
        public long InvoiceMasterId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long ParticularId { get; set; }
        public string ReferenceNo { get; set; }    /*Reference Id of Purchase Order, Purchase invoice, Sale invoice, Purchase Return and sale return */
        public int DocumentTypeId { get; set; }     /*Enum id of PurchaseInvoice, SaleInvoice etc*/
        public string Remarks { get; set; }
        public int InvoiceStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public long OutletId { get; set; }
        public int PaymentMode { get; set; }
        public decimal PaidReceivedAmount { get; set; }
        public decimal SaleTax { get; set; }
        public string InvoiceSerialNo { get; set; }
        public long SerialIndex { get; set; }
        public long? OrderMasterId { get; set; }

        public virtual ICollection<InvoiceDetail> InvoiceDetail { get; set; }
    }
}
