using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.AccountModels.ChartOfAccounts
{
    public class HeadCategoriesRequest : CommonBaseModel
    {
        public int HeadCategoriesId { get; set; }
        public string Name { get; set; }
        public string MainHeadsName { get; set; }
        public long MainHeadsId { get; set; }
    }
}