using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.AccountModels.ChartOfAccounts
{
    public class SubCategoriesResponse : CommonBaseModel
    {
        public int SubCategoriesId { get; set; }
        public int MainHeadsId { get; set; }
        public long HeadCategoriesId { get; set; }
        public string MainHeadsName { get; set; }
        public string HeadCategoriesName { get; set; }
        public string SubCategoriesName { get; set; }
        public string Name { get; set; }
    }
}