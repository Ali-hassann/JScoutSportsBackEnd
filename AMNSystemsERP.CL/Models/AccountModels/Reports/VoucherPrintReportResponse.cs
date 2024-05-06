namespace AMNSystemsERP.CL.Models.AccountModels.Reports
{
    public class VoucherPrintReportResponse : AccountsReportBase
    {
        public long VouchersMasterId { get; set; }
        public string ReferenceNo { get; set; }
        public int VoucherTypeId { get; set; }
        public string VoucherTypeName { get; set; }
        public DateTime VoucherDate { get; set; }
        public string Remarks { get; set; }
        public long OutletId { get; set; }
        public long OrganizationId { get; set; }
        public string ChequeNo { get; set; }
        public bool ChequeStatus { get; set; }
        public DateTime ChequeDate { get; set; }
        public string VoucherStatusName { get; set; }
        public string Narration { get; set; }
        public long PostingAccountsId { get; set; }
        public string PostingAccountsName { get; set; }
        public string Tag { get; set; }
        public long SerialIndex { get; set; }
        public string SerialNumber { get; set; }
    }
}