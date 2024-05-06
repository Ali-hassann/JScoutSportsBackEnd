using System;

namespace AMNSystemsERP.CL.Models.AccountModels.Reports
{
    public class DailyVouchersPrintReportResponse
    {
        public long VouchersMasterId { get; set; }
        public string VoucherDate { get; set; }
        public string ReferenceNo { get; set; }
        public string VoucherTypeName { get; set; }
        public string Remarks { get; set; }
        public string Narration { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string PostingAccountsName { get; set; }
        public string OutletName { get; set; }
        public string OutletAddress { get; set; }
    }
}