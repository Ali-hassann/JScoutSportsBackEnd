using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Accounts
{
    public class VouchersDetail
    {
        [Key]
        public long VouchersDetailId { get; set; }
        public string Narration { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public long VouchersMasterId { get; set; }
        public long PostingAccountsId { get; set; }
        public string ChequeNo { get; set; } = string.Empty;
        public bool ChequeStatus { get; set; }
        public DateTime? ChequeDate { get; set; }

        public virtual VouchersMaster VouchersMaster { get; set; }
    }
}