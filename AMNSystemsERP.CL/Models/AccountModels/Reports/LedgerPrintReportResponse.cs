namespace AMNSystemsERP.CL.Models.AccountModels.Reports
{
    public class LedgerPrintReportResponse : AccountsReportBase
    {
        public long VouchersMasterId { get; set; }
        public string VoucherDate { get; set; }
        public string ReferenceNo { get; set; }
        public string VoucherTypeName { get; set; }
        public string PostingAccountsId { get; set; }
        public string PostingAccountsName { get; set; }
        public string Narration { get; set; }
        public string Tag { get; set; }
        public decimal SumExpense { get; set; }
        public string Remarks { get; set; }
        public string SerialNumber { get; set; }
    }
}