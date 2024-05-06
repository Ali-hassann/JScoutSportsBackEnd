using AMNSystemsERP.CL.Models.Commons.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMNSystemsERP.CL.Models.AccountModels.DashBoard
{
    public class DashBoardDetailIncomeExpenseResponse : CommonBaseModel
    {
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }
    public class DashBoardDetailCashBankCreditCardResponse : CommonBaseModel
    {
        public decimal Cash { get; set; }
        public decimal Bank { get; set; }
        public decimal CreditCard { get; set; }
    }

    public class DashBoardSummaryResponse
    {
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Cash { get; set; }
        public decimal Bank { get; set; }
        public decimal CreditCard { get; set; }

        public virtual List<DashboardDetailSummaryOutletwise> SummaryDetail { get; set; }
        public virtual List<DashBoardMaxIncomeExpenseResponse> MaxIncomeExpenseDetail { get; set; }

    }

    public class DashboardDetailSummaryOutletwise : DashBoardDetailCashBankCreditCardResponse
    {
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }
    public class DashBoardMaxIncomeExpenseResponse : CommonBaseModel
    {
        public long PostingAccountsId { get; set; }
        public string PostingAccountsName { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }

    }
}
