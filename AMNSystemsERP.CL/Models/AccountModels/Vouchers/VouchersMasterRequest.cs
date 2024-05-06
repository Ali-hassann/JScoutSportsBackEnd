using AMNSystemsERP.CL.Enums.AccountEnums;
using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.AccountModels.Vouchers
{
    public class VouchersMasterRequest : VoucherMasterList
    {
        public long? InvoiceNo { get; set; }
        public int? InvoiceType { get; set; }
        public int FiscalYear { get; set; }
        public long? ProjectsId { get; set; }

        public List<VouchersDetailRequest> VouchersDetailRequest { get; set; } = new();
        public List<VoucherImagesRequest> VoucherImagesRequest { get; set; } = new();
    }

    public class VoucherMasterList 
    {
        public long VouchersMasterId { get; set; }
        public string ReferenceNo { get; set; }
        public VoucherType VoucherTypeId { get; set; }
        public string? TransactionType { get; set; }
        public string VoucherTypeName { get; set; }
        public DateTime VoucherDate { get; set; }
        public string Remarks { get; set; }
        public decimal TotalAmount { get; set; }
        public long OutletId { get; set; }
        public int TotalTags { get; set; }
        public int TotalImages { get; set; }
        public string CreatedBy { get; set; }
        public int VoucherStatus { get; set; }
        public string VoucherStatusName { get; set; }
        public string OutletName { get; set; }
        public string OutletImagePath { get; set; }
        public long SerialIndex { get; set; }
        public string SerialNumber { get; set; }
    }

    public class ExpenseVoucherRequest : IOutletBaseModel
    {
        public DateTime VoucherDate { get; set; }
        public string Remarks { get; set; }
        public decimal TotalAmount { get; set; }
        public long PostingAccountsId { get; set; }
        public long VouchersMasterId { get; set; }
        public long OutletId { get; set; }
        public long OrganizationId { get; set; }
        public int VoucherStatus { get; set; }
        public string OutletName { get; set; }
        public string OutletImagePath { get; set; }
    }
}