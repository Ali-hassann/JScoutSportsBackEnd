using AMNSystemsERP.BL.Repositories.Dashboard;
using AMNSystemsERP.CL.Models.AccountModels.DashBoard;
using AMNSystemsERP.CL.Models.AccountModels.Reports;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/Dashboard")]
    public class DashboardController : ApiController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpPost]
        [Route("DashBoardDetail")]
        public async Task<DashBoardSummaryResponse> DashBoardDetail([FromBody] DashBoardDetailRequest request)
        {
            try
            {
                if (request != null)
                {
                    if (request.OrganizationId > 0
                        && !string.IsNullOrEmpty(request.FromDate)
                    && !string.IsNullOrEmpty(request.ToDate))
                    {
                        return await _dashboardService.DashBoardDetail(request);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("DashboardMonthWiseProfitLossSummary")]
        public async Task<List<AccountMonthWiseSummary>> DashboardMonthWiseProfitLossSummary([FromBody] DashboardMonthWiseRequest request)
        {
            try
            {
                if (request != null)
                {
                    if (request.OrganizationId > 0 && request.Year > 0)
                    {
                        return await _dashboardService.DashboardMonthWiseProfitLossSummary(request);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }


        [HttpPost]
        [Route("GetLatestPostVouchers")]
        public async Task<List<DashboardLatestVouchersRequest>> GetLatestPostVouchers([FromBody] DashBoardDetailRequest request)
        {
            try
            {
                if (request.OrganizationId > 0
                    && request.OutletId > 0
                    && !string.IsNullOrEmpty(request.FromDate)
                    && !string.IsNullOrEmpty(request.ToDate))
                {
                    return await _dashboardService.GetLatestPostVouchers(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetTrialBalanceData")]
        public async Task<List<TrialBalancePrintReportResponse>> GetTrialBalanceData( long outletId)
        {
            try
            {
                if ( outletId > 0)
                {
                    return await _dashboardService.GetTrialBalanceData(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetEmployeeDashboardData")]
        public async Task<List<EmployeeDashboardResponse>> GetEmployeeDashboardData(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _dashboardService.GetEmployeeDashboardData(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetAllowanceDashboardData")]
        public async Task<List<FinancialSummaryResponse>> GetAllowanceDashboardData(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _dashboardService.GetAllowanceDashboardData(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        
        [HttpGet]
        [Route("GetLoanDashboardData")]
        public async Task<List<FinancialSummaryResponse>> GetLoanDashboardData(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _dashboardService.GetLoanDashboardData(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
    }
}