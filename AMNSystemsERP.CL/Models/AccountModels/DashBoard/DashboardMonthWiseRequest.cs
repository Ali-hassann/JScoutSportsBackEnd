using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.AccountModels.DashBoard
{
    public class DashboardMonthWiseRequest : CommonBaseModel
    {
        public int Year { get; set; }
        public int MonthId { get; set; }
        public bool IsActive { get; set; }
    }
}