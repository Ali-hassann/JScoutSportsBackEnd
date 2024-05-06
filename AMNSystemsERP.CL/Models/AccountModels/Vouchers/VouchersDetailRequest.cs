namespace AMNSystemsERP.CL.Models.AccountModels.Vouchers
{
    public class VouchersDetailRequest
    {
        public long VouchersDetailId { get; set; }
        public string Narration { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public long VouchersMasterId { get; set; }
        public long PostingAccountsId { get; set; }
        public string PostingAccountsName { get; set; }
        public string ChequeNo { get; set; } = string.Empty;
        public bool ChequeStatus { get; set; }
        public DateTime? ChequeDate { get; set; }
    }
}