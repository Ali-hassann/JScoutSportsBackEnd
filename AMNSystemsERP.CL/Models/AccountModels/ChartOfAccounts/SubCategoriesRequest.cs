using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.AccountModels.ChartOfAccounts
{
    public class SubCategoriesRequest : CommonBaseModel
    {
        public long SubCategoriesId { get; set; }
        public string Name { get; set; }
        public int HeadCategoriesId { get; set; }
    }
}