using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.AccountModels.Reports
{
    public class ReportPrintBaseRequest : IOutletBaseModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public long OutletId { get; set; }
        public long OrganizationId { get; set; }
        public bool IsActive { get; set; }
        public bool IsPosted { get; set; }
        public string OutletName { get; set; }
        public string Address { get; set; }
        public string OutletImagePath { get; set; }
        public List<long> PostingAccountsIds { get; set; }
        public long PostingAccountsId { get; set; }
        public long SubCategoriesId { get; set; }
        public long OwnerEquityId { get; set; }

    }

    public class ReportPrintLedgerRequest : ReportPrintBaseRequest
    {
        public List<long> PostingAccountsIds { get; set; }
    }

    public class ReportPrintSubCategoryRequest : ReportPrintBaseRequest
    {
        public long SubCategoriesId { get; set; }
        public bool IncludeZeroValue { get; set; }
    }

    public class ReportPrintHeadCategoryRequest : ReportPrintBaseRequest
    {
        public long HeadCategoriesId { get; set; }
        public bool IncludeDetail { get; set; }
        public bool IncludeZeroValue { get; set; }
    }

    public class ReportPrintMainHeadRequest : ReportPrintBaseRequest
    {
        public long MainHeadId { get; set; }
        public bool IncludeDetail { get; set; }
        public bool IncludeZeroValue { get; set; }
    }

    public class ReportPrintTrialBalanceRequest : ReportPrintBaseRequest
    {
        public bool IncludeZeroValue { get; set; }
    }

    public class ReportPrintDailyVouchersRequest : ReportPrintBaseRequest
    {
        public int VoucherTypeId { get; set; }
        public int VoucherStatus { get; set; }
    }

    public class DashBoardDetailRequest : ReportPrintBaseRequest
    {

    }

}