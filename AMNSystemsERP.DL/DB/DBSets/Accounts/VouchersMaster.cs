using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Accounts
{
    public class VouchersMaster : Entity
    {
        public VouchersMaster()
        {
            VouchersDetail = new HashSet<VouchersDetail>();
            VoucherImages = new HashSet<VoucherImages>();
        }

        [Key]
        public long VouchersMasterId { get; set; }
        public string ReferenceNo { get; set; }
        public int VoucherTypeId { get; set; }
        public string? TransactionType { get; set; }
        public DateTime VoucherDate { get; set; }
        public string Remarks { get; set; }
        public long? InvoiceNo { get; set; }
        public int? InvoiceType { get; set; }
        public int FiscalYear { get; set; }
        public decimal TotalAmount { get; set; }
        public long? ProjectsId { get; set; }
        public long OutletId { get; set; }
        public int VoucherStatus { get; set; }
        public long SerialIndex { get; set; }
        public string SerialNumber { get; set; }

        public virtual ICollection<VouchersDetail> VouchersDetail { get; set; }
        public virtual ICollection<VoucherImages> VoucherImages { get; set; }
    }
}