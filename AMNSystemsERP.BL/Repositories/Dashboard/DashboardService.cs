using AMNSystemsERP.BL.Repositories.Reports;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.AccountModels.DashBoard;
using AMNSystemsERP.CL.Models.AccountModels.Reports;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Dashboard;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;
using System.Data;

namespace AMNSystemsERP.BL.Repositories.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unit;
        private readonly IReportsService _reportsService;

        public DashboardService(IUnitOfWork unit, IReportsService reportsService)
        {
            _unit = unit;
            _reportsService = reportsService;
        }

        public async Task<DashBoardSummaryResponse> DashBoardDetail(DashBoardDetailRequest request)
        {
            try
            {
                var pOrganizationId = DBHelper.GenerateDapperParameter("ORGANIZATIONID", request.OrganizationId, DbType.Int64);
                var pOutletId = DBHelper.GenerateDapperParameter("OUTLETID", request.OutletId, DbType.Int64);
                var pFromDate = DBHelper.GenerateDapperParameter("FROMDATE", request.FromDate ?? "", DbType.String);
                var pToDate = DBHelper.GenerateDapperParameter("TODATE", request.ToDate ?? "", DbType.String);
                var pIsActive = DBHelper.GenerateDapperParameter("ISACTIVE", request.IsActive, DbType.Boolean);


                var dashBoardDetials = await _unit
                                             .DapperRepository
                                             .GetMultiResultsWithParentChildAndDetailAsync<object
                                                                                           , DashBoardDetailIncomeExpenseResponse
                                                                                           , DashBoardDetailCashBankCreditCardResponse>
                                                                                           (
                                                                                                "GET_DASHBOARD_DETAIL",
                                                                                                 DBHelper.GetDapperParms
                                                                                                 (
                                                                                                     pOrganizationId,
                                                                                                     pOutletId,
                                                                                                     pFromDate,
                                                                                                     pToDate,
                                                                                                     pIsActive
                                                                                                 ),
                                                                                                 CommandType.StoredProcedure
                                                                                           );

                /*Get maximum top 5 income and expense*/
                var query = $@"SELECT *FROM(SELECT top 5         
                                  v.OutletId        
                                  , v.PostingAccountsId        
                                  , coa.PostingAccountsName        
                                  , MAX(v.creditAmount) Amount        
                                  , 'Income' Type        
                                   FROM V_VOUCHERS v         
                                    INNER JOIN V_GET_CHARTOFACCOUNTS coa        
                                    ON v.PostingAccountsId=coa.PostingAccountsId        
                                     WHERE v.VoucherDate BETWEEN '{request.FromDate}' AND '{request.ToDate}'     
                               AND v.VoucherStatus=2        
                               AND COA.IsActive = {(request.IsActive == true ? 1 : 0)}      
                                      AND         
                                      (        
                                       {request.OutletId} = 0        
                                       OR        
                                       v.OutletId = {request.OutletId}        
                                      )        
                                     AND coa.MainHeadsId=4        
                                     Group By v.PostingAccountsId        
                                        , coa.PostingAccountsName         
                                        , v.OutletId        
                                     ORDER BY MAX(v.creditAmount) DESC) cheeta   
                              UNION ALL  
                              SELECT *FROM(SELECT top 5 
                                   v.OutletId        
                                   , v.PostingAccountsId        
                                   , coa.PostingAccountsName        
                                   , MAX(v.DebitAmount) Amount        
                                   , 'Expense' Type        
                                    FROM V_VOUCHERS v         
                                     INNER JOIN V_GET_CHARTOFACCOUNTS coa        
                                     ON v.PostingAccountsId=coa.PostingAccountsId        
                                      WHERE v.VoucherDate BETWEEN '{request.FromDate}' AND '{request.ToDate}'       
                                AND v.VoucherStatus=2            
                                AND COA.IsActive = {(request.IsActive == true ? 1 : 0)}      
                                       AND         
                                       (        
                                        {request.OutletId} = 0        
                                        OR        
                                        v.OutletId = {request.OutletId}        
                                       )        
                                      AND coa.MainHeadsId=5        
        
                                      Group By v.PostingAccountsId        
                                         , coa.PostingAccountsName   
                                         , v.OutletId        
                                      ORDER by MAX(v.DebitAmount) DESC) as munir";

                var maxIncomeExpenseDetails = await _unit
                                                    .DapperRepository
                                                    .GetListQueryAsync<DashBoardMaxIncomeExpenseResponse>(query);

                /******************************/

                DashBoardSummaryResponse summary = new();
                if (dashBoardDetials != null)
                {
                    summary.Income = dashBoardDetials?.Item2?.Sum(c => c.Income) ?? 0;
                    summary.Expense = dashBoardDetials?.Item2?.Sum(c => c.Expense) ?? 0;
                    summary.Cash = dashBoardDetials?.Item3?.Sum(c => c.Cash) ?? 0;
                    summary.Bank = dashBoardDetials?.Item3?.Sum(c => c.Bank) ?? 0;
                    summary.CreditCard = dashBoardDetials?.Item3?.Sum(c => c.CreditCard) ?? 0;

                    summary.SummaryDetail = new List<DashboardDetailSummaryOutletwise>();
                    dashBoardDetials?
                        .Item2
                        ?.Select(a => a.OutletId)
                        ?.ToList()
                        ?.ForEach(c =>
                        {
                            DashboardDetailSummaryOutletwise dashboardDetail = new();
                            dashboardDetail.Bank = dashBoardDetials.Item3.FirstOrDefault(x => x.OutletId == c).Bank;
                            dashboardDetail.Cash = dashBoardDetials.Item3.FirstOrDefault(x => x.OutletId == c).Cash;
                            dashboardDetail.CreditCard = dashBoardDetials.Item3.FirstOrDefault(x => x.OutletId == c).CreditCard;
                            dashboardDetail.Income = dashBoardDetials.Item2.FirstOrDefault(x => x.OutletId == c).Income;
                            dashboardDetail.Expense = dashBoardDetials.Item2.FirstOrDefault(x => x.OutletId == c).Expense;
                            dashboardDetail.OrganizationId = dashBoardDetials.Item2.FirstOrDefault(x => x.OutletId == c).OrganizationId;
                            dashboardDetail.OutletId = dashBoardDetials.Item2.FirstOrDefault(x => x.OutletId == c).OutletId;

                            summary.SummaryDetail.Add(dashboardDetail);
                        });

                    summary.MaxIncomeExpenseDetail = new List<DashBoardMaxIncomeExpenseResponse>();
                    if (maxIncomeExpenseDetails?.Count > 0)
                        summary.MaxIncomeExpenseDetail.AddRange(maxIncomeExpenseDetails);


                    return summary;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new DashBoardSummaryResponse();
        }
        public async Task<List<AccountMonthWiseSummary>> DashboardMonthWiseProfitLossSummary(DashboardMonthWiseRequest request)
        {
            try
            {
                var query = $@"SELECT f.MonthNumber,f.name MonthName,ISNULL(tb2.Income,0)Income,ISNULL(tb2.Expense,0)Expense            
                                , CASE when Income-Expense>0 then Income-Expense else 0 end as 'Profit'            
                                , CASE when Income-Expense<0 then Income-Expense else 0 end as 'Loss'            
                                  FROM  F_GET_MONTHS() f            
                                    LEFT JOIN             
                                     (SELECT MonthNumber ,SUM(Income) Income, SUM(Expense) Expense             
                                        FROM(SELECT             
                                         Month(v.VoucherDate) MonthNumber            
                                         , CASE WHEN coa.MainHeadsId=4 then SUM(v.CreditAmount) ELSE 0 ENd as Income              
                                         , CASE WHEN coa.MainHeadsId=5 then SUM(v.DEBITAMOUNT) ELSE 0 ENd as Expense                
                                           FROM V_VOUCHERS v ,V_GET_CHARTOFACCOUNTS  coa             
                                             WHERE v.postingAccountsId=coa.postingAccountsId            
                                            AND month(v.VoucherDate) in(SELECT monthNumber FROM F_GET_MONTHS())            
                                            AND Year(v.VoucherDate)= {request.Year}            
                                   AND v.VoucherStatus=2    
                                            AND coa.MainHeadsId in(4,5)           
                                   AND coa.IsActive= CASE WHEN {(request.IsActive == true ? 1 : 0)} = 1 THEN coa.IsActive ELSE 1  END        
                                            AND             
                                            (             
                                       {request.OutletId} = 0            
                                     OR            
                                     v.OutletId = {request.OutletId}            
                                            )            
                                             Group By month(v.voucherDate),coa.MainHeadsId) as tbl            
                                     Group By  MonthNumber) as tb2            
                                     ON f.Monthnumber=tb2.MonthNumber            
                                     ORDER BY f.Monthnumber  ";

                return await _unit.DapperRepository.GetListQueryAsync<AccountMonthWiseSummary>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DashboardLatestVouchersRequest>> GetLatestPostVouchers(DashBoardDetailRequest request)
        {
            try
            {
                var query = $@"SELECT TOP 10  
                                 V.VouchersMasterId  
                                 , V.OutletId  
                                 , V.InvoiceNo  
                                 , V.VoucherStatus  
                                 , V.TotalAmount  
                                 , VT.Name AS VouchersTypeName  
                                 , CONVERT(VARCHAR(15), V.VoucherDate,106) AS CreatedDate  
                                FROM VouchersMaster AS V  
                                INNER JOIN VoucherType AS VT  
                                 ON V.VoucherTypeId = VT.VoucherTypeId   
                                AND V.OutletId = {request.OutletId}  
                                AND V.VoucherDate   
                                 BETWEEN '{request.FromDate}'   
                                 AND '{request.ToDate}'  
                                ORDER BY   
                                 V.VoucherDate DESC";

                return await _unit.DapperRepository.GetListQueryAsync<DashboardLatestVouchersRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TrialBalancePrintReportResponse>> GetTrialBalanceData(long outletId)
        {
            try
            {
                var fromDate = DateHelper.GetDateFormat(new DateTime(2010, 02, 01), false, false, DateFormats.SqlDateFormat.ToString());
                var toDate = DateHelper.GetDateFormat(DateHelper.GetCurrentDate(), false, false, DateFormats.SqlDateFormat.ToString());
                var request = new ReportPrintTrialBalanceRequest()
                {
                    OutletId = outletId,
                    FromDate = fromDate,
                    ToDate = toDate,
                    IsActive = true,
                    PostingAccountsIds = new List<long>()
                };

                return (await _reportsService.PrintTrialBalance(request)).Data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmployeeDashboardResponse>> GetEmployeeDashboardData(long outletId)
        {
            try
            {
                var query = $@"SELECT 
	                                COUNT(EmployeeId) AS EmployeeCount 
	                                , DepartmentsId
	                                , DepartmentsName
	                                , SUM(SalaryAmount) AS SalarySum
                                FROM V_EMPLOYEE  
                                WHERE OutletId = {outletId}
                                GROUP BY DepartmentsId , DepartmentsName";

                return await _unit.DapperRepository.GetListQueryAsync<EmployeeDashboardResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FinancialSummaryResponse>> GetAllowanceDashboardData(long outletId)
        {
            try
            {
                var query = $@"SELECT 
	                                SUM(EA.Amount) AS Amount
	                                , AT.Name AS TypeName
                                FROM V_EMPLOYEE  AS E
                                INNER JOIN EmployeeAllowances AS EA
	                                ON E.EmployeeId = EA.EmployeeId
                                INNER JOIN AllowanceType AS AT
	                                ON AT.AllowanceTypeId = EA.AllowanceTypeId
                                WHERE E.OutletId = {outletId}
                                GROUP BY AT.Name";

                return await _unit.DapperRepository.GetListQueryAsync<FinancialSummaryResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FinancialSummaryResponse>> GetLoanDashboardData(long outletId)
        {
            try
            {
                var query = $@"SELECT                                                         
                                E.DepartmentsId
								, E.DepartmentsName
	                            , SUM(EL.Balance) AS Amount     
							FROM V_EMPLOYEE AS E
							INNER JOIN 
							(
								SELECT  
								EL.EmployeeId
	                            , MIN(ISNULL(LoanAmount-DeductAmount,0)) OVER(partition by EL.EmployeeId    
                                    ORDER BY EL.EmployeeId, LoanDate,EmployeeLoanId) AS Balance        
                            FROM EmployeeLoan AS EL
							WHERE LoanTypeId = 1
							) AS EL
								ON E.EmployeeId = EL.EmployeeId
                            where E.OutletId = {outletId}
							GROUP BY 
								E.DepartmentsId
								, E.DepartmentsName";

                return await _unit.DapperRepository.GetListQueryAsync<FinancialSummaryResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}