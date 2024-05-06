using AMNSystemsERP.CL.Models.AccountModels.DashBoard;
using AMNSystemsERP.CL.Models.AccountModels.Reports;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Dashboard;

namespace AMNSystemsERP.BL.Repositories.Dashboard
{
    public interface IDashboardService
    {
        Task<DashBoardSummaryResponse> DashBoardDetail(DashBoardDetailRequest request);
        Task<List<AccountMonthWiseSummary>> DashboardMonthWiseProfitLossSummary(DashboardMonthWiseRequest request);
        Task<List<DashboardLatestVouchersRequest>> GetLatestPostVouchers(DashBoardDetailRequest request);
        Task<List<TrialBalancePrintReportResponse>> GetTrialBalanceData(long outletId);
        Task<List<EmployeeDashboardResponse>> GetEmployeeDashboardData(long outletId);
        Task<List<FinancialSummaryResponse>> GetAllowanceDashboardData(long outletId);
        Task<List<FinancialSummaryResponse>> GetLoanDashboardData(long outletId);
    }
}