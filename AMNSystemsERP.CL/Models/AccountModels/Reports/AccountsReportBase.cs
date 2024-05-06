namespace AMNSystemsERP.CL.Models.AccountModels.Reports
{
    public class AccountsReportBase
    {
        public decimal OpeningBalance { get; set; }
        public decimal BfAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal ClosingBalance { get; set; } // This is running balance
    }
}