namespace AMNSystemsERP.CL.Models.AccountModels.ChartOfAccounts
{
    public class PostingAccountsRequest
    {
        public long PostingAccountsId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public long SubCategoriesId { get; set; }
        public decimal OpeningDebit { get; set; }
        public decimal OpeningCredit { get; set; }
        public DateTime OpeningDate { get; set; }
        public long OutletId { get; set; }
        public long EmployeeId { get; set; }
    }
}