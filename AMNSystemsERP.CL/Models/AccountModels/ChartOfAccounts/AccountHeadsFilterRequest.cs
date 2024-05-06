using AMNSystemsERP.CL.Models.Commons.Base;
using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.AccountModels.ChartOfAccounts
{
    public class AccountHeadsFilterRequest : CommonBaseModel
    {
        public long HeadCategoriesId { get; set; }
        public long SubCategoriesId { get; set; }
        public long PostingAccountsId { get; set; }
    }
}