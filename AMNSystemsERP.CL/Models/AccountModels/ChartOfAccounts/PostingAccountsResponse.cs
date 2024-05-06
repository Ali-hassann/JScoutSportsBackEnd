namespace AMNSystemsERP.CL.Models.AccountModels.ChartOfAccounts
{
    public class PostingAccountsResponse : SubCategoriesResponse
    {
        public long PostingAccountsId { get; set; }
        public long? EmployeeId { get; set; }
        public string PostingAccountsName { get; set; }
        public bool IsActive { get; set; }
        public decimal? OpeningDebit { get; set; }
        public decimal? OpeningCredit { get; set; }
        public DateTime? OpeningDate { get; set; }
    }
}