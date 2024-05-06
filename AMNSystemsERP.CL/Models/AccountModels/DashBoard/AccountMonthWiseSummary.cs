using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.AccountModels.DashBoard
{
    public class AccountMonthWiseSummary : CommonBaseModel
    {
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Profit { get; set; }
        public decimal Loss { get; set; }
        public string MonthName { get; set; }
        public int MonthNumber { get; set; }
    }
}