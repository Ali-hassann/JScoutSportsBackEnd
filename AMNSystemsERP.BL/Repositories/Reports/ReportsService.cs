using System.Data;
using AMNSystemsERP.CL.Models.AccountModels.Reports;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.StockManagementModels;
using AMNSystemsERP.CL.Models.StockManagementModels.Reports;
using AMNSystemsERP.CL.Models.RDLCModels;
using Microsoft.Extensions.Options;
using AMNSystemsERP.CL.Enums.Reports;
using AMNSystemsERP.CL.Services.CurrentLogin;
using AMNSystemsERP.CL.Models.InventoryModels;
using Microsoft.Extensions.Configuration;
using AMNSystemsERP.CL.Models.ProductionModels;

namespace AMNSystemsERP.BL.Repositories.Reports
{
    public class ReportsService : IReportsService
    {
        private readonly IUnitOfWork _unit;
        private readonly ICurrentLoginService _currentLogin;
        private readonly IOptions<ReportsConfigs> _rdlcConfigs;
        private readonly IConfiguration _config;

        // For RDLC Reports
        //

        public ReportsService(IUnitOfWork unit
            , IOptions<ReportsConfigs> rdlcConfigs
            , IConfiguration config
            , ICurrentLoginService currentLogin)
        {
            _unit = unit;
            _rdlcConfigs = rdlcConfigs;
            _config = config;
            _currentLogin = currentLogin;
        }

        // ----------------------------------------------------------------------------
        // ------------------ Rdlc Reports Section Start ------------------------------
        // ----------------------------------------------------------------------------
        #region Accounts Report 

        // ------------------ Vouchers Section Start ----------------------------------

        public async Task<ReportDataParms<VoucherPrintReportResponse>> PrintVoucher(long vouchersMasterId, bool isAdvanceVoucher)
        {
            try
            {
                var query = $@"SELECT               
                                 v.VouchersMasterId              
                                 , v.ReferenceNo              
                                 , v.VoucherTypeId              
                                 , v.VoucherDate       
                                 , v.ChequeNo      
                                 , v.ChequeDate      
                                 , v.ChequeStatus      
                                 , v.Remarks              
                                 , v.OutletId                     
                                 , v.Narration           
                                 , v.DebitAmount              
                                 , v.CreditAmount              
                                 , v.SerialNumber              
                                 , v.PostingAccountsId
                                 , PA.Name AS PostingAccountsName              
                                 , VT.Description AS VoucherTypeName              
                                , vs.StatusName as VoucherStatusName    
                                FROM V_VOUCHERS_PRINT v             
                                INNER JOIN PostingAccounts AS PA              
                                 ON PA.PostingAccountsId = v.PostingAccountsId            
                                INNER JOIN VoucherType AS VT            
                                 ON VT.VoucherTypeId = v.VoucherTypeId     
                                 INNER JOIN voucherstatus vs    
                                ON v.VoucherStatus=vs.StatusId
                                WHERE v.VouchersMasterId = {vouchersMasterId}            
                                order by v.creditAmount";

                var reportData = await _unit.DapperRepository.GetListQueryAsync<VoucherPrintReportResponse>(query);
                if (reportData?.Count > 0)
                {
                    var reportConfig = new RdlcReportConfiguration();

                    if (isAdvanceVoucher)
                    {
                        reportConfig = _rdlcConfigs
                                       ?.Value
                                       ?.Configs
                                       ?.Find(x => x.ReportName == ReportName.Accounts.VoucherAdvancePrint.ToString()) ?? new RdlcReportConfiguration();
                    }
                    else
                    {
                        // Calling and getting path of RDLC Report
                        reportConfig = _rdlcConfigs
                                           ?.Value
                                           ?.Configs
                                           ?.Find(x => x.ReportName == ReportName.Accounts.VoucherPrint.ToString()) ?? new RdlcReportConfiguration();
                    }
                    var reportParams = new Dictionary<string, string>();
                    reportParams.Add("pmrOutletName", _currentLogin?.OutletName ?? "-");
                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    reportParams.Add("pmrOutletImage", baseUrl + _currentLogin?.OutletImage ?? "-");
                    reportParams.Add("pmrAddress", _currentLogin?.Address ?? "-");

                    var dataParms = new ReportDataParms<VoucherPrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = "VoucherPrint-" + vouchersMasterId
                    };
                    return dataParms;
                    //
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<LedgerPrintReportResponse>> PrintAccountsLedger(ReportPrintLedgerRequest request)
        {
            try
            {
                var query = $@"  SELECT  bf.*    
                                    , ISNULL(bf.PostingAccountsId,0) AS PostingAccountsId
                                    , FORMAT(detail.VoucherDate,'dd MMM yyyy') AS VoucherDate  
                                    , detail.SerialNumber
                                    , detail.ReferenceNo  
                                    , detail.Remarks     
                                    , detail.Narration
                                        --, detail.ProjectName  
                                    , isnull(detail.DebitAmount,0) as DebitAmount  
                                    , isnull(detail.CreditAmount,0) as CreditAmount  
                                      , bf.OpeningBalance+SUM(ISNULL(detail.DebitAmount-detail.CreditAmount,0)) OVER(partition by bf.PostingAccountsId   
		                                              ORDER BY bf.PostingAccountsId, detail.VoucherDate,detail.VouchersDetailId) AS ClosingBalance                                             
                                    FROM
                                        (
                                            /*** BF *****/
                                            SELECT *FROM      
	                                            (
		                                        SELECT p.PostingAccountsId
			                                        , Name as PostingAccountsName
			                                        , (OpeningDebit-OpeningCredit)+ISNULL(bf.BalanceAmount,0) as OpeningBalance
			                                        FROM PostingAccounts as P                    
			                                            LEFT JOIN
				                                        (      
				                                            SELECT v.PostingAccountsId
						                                         , SUM(DebitAmount-CreditAmount) as BalanceAmount
						                                            FROM V_Vouchers v      
						                                            WHERE v.VoucherDate <'{request.FromDate}' 
						                                            AND PostingAccountsId={request.PostingAccountsId}
						                                            GROUP by v.PostingAccountsId
				                                        ) as bf      
						                                        ON p.PostingAccountsId=bf.PostingAccountsId
						                                        WHERE p.PostingAccountsId={request.PostingAccountsId}
	                                            ) as tbl 

			                                )as bf
                                        LEFT JOIN    
                                            (    
                                                SELECT    
                                                 v.VouchersDetailId  
                                                , v.SerialNumber
                                                , v.VoucherDate  
                                                , v.PostingAccountsId  
                                                , v.ReferenceNo  
                                                , v.Remarks                                          
                                                -- , v.ProjectName  
                                                , v.DebitAmount  
                                                , v.CreditAmount  
                                                , v.Narration
                                                FROM V_Vouchers v    
                                            WHERE v.VoucherDate BETWEEN '{request.FromDate}' AND '{request.ToDate}'   
                                                            AND PostingAccountsId={request.PostingAccountsId}
                                                ) 
						                        as detail      
                                                ON bf.PostingAccountsId=detail.PostingAccountsId ";


                var reportData = await _unit.DapperRepository.GetListQueryAsync<LedgerPrintReportResponse>(query);

                if (reportData?.Count > 0)
                {
                    // Setting Params For Report
                    var reportParams = new Dictionary<string, string>();
                    var date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);
                    reportParams.Add("pmrDate", DateHelper.GetReportFormatDate(request.FromDate, request.ToDate));
                    reportParams.Add("pmrOutletName", _currentLogin.OutletName);
                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    reportParams.Add("pmrOutletImage", baseUrl + _currentLogin?.OutletImage ?? "-");
                    reportParams.Add("pmrAddress", _currentLogin.Address);

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Accounts.LedgerPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<LedgerPrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"LedgerPrint-{date}"
                    };
                    return dataParms;
                    //
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<LedgerPrintReportResponse>> GeneralExpensePrint(ReportPrintLedgerRequest request)
        {
            try
            {
                List<LedgerPrintReportResponse> reportData = await GetAccountsLedger(request);
                if (reportData?.Count > 0)
                {
                    var reportParams = new Dictionary<string, string>();
                    var date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);
                    reportParams.Add("pmrDate", date);
                    reportParams.Add("pmrOutletName", _currentLogin.OutletName ?? "");
                    reportParams.Add("pmrAddress", _currentLogin.Address ?? "");

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Accounts.LedgerExpensePrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<LedgerPrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"ExpensePrint-{date}"
                    };
                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<List<LedgerPrintReportResponse>> GetAccountsLedger(ReportPrintLedgerRequest request)
        {
            // Preparing Data For Ledger Print Report
            var pOutletId = DBHelper.GenerateDapperParameter("OUTLETID", request.OutletId, DbType.Int64);
            var table = CommonHelper.ListToDataTable(request.PostingAccountsIds);
            var pFromDate = DBHelper.GenerateDapperParameter("DATEFROM", request.FromDate ?? "", DbType.String);
            var pToDate = DBHelper.GenerateDapperParameter("DATETO", request.ToDate ?? "", DbType.String);
            var pPostingAccountsIds = DBHelper.GenerateDapperParameter("ACCOUNTIDS", table, DbType.Object);

            var reportData = await _unit
                                   .DapperRepository
                                   .GetManyWithStoreProcedureAsync<LedgerPrintReportResponse>("GET_ACCOUNTS_LEDGER",
                                                                                               DBHelper.GetDapperParms
                                                                                               (
                                                                                                   pOutletId,
                                                                                                   pFromDate,
                                                                                                   pToDate,
                                                                                                   pPostingAccountsIds
                                                                                               ));
            var sumDebit = reportData.Sum(c => c.DebitAmount);
            reportData.ForEach(data =>
            {
                data.SumExpense = sumDebit;
            });

            //
            return reportData;
        }

        public async Task<ReportDataParms<TrialBalancePrintReportResponse>> PrintSubCategoryLedger(ReportPrintTrialBalanceRequest request)
        {
            try
            {
                var reportData = await Function_TrialBalance(request);

                var subCategoriesData = reportData.Where(c => c.SubCategoriesId == request.SubCategoriesId).ToList();

                if (subCategoriesData?.Count > 0)
                {
                    var subCatName = subCategoriesData.Select(c => c.SubCategoriesName).FirstOrDefault();
                    // Setting Params For Report
                    var reportParams = new Dictionary<string, string>();
                    var date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);

                    reportParams.Add("pmrDate", date ?? "");
                    reportParams.Add("pmrTitle", subCatName ?? "");
                    reportParams.Add("pmrOutletName", _currentLogin.OutletName ?? "");
                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    reportParams.Add("pmrOutletImage", baseUrl + _currentLogin?.OutletImage ?? "-");
                    reportParams.Add("pmrAddress", _currentLogin.Address ?? "");

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Accounts.SubCategoryPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<TrialBalancePrintReportResponse>()
                    {
                        Data = subCategoriesData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"SubCategoryPrint-{date}"
                    };
                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<HeadCategoryPrintReportResponse>> PrintHeadCategoryLedger(ReportPrintHeadCategoryRequest request)
        {
            try
            {
                // Preparing Data For Head Category Print Report                
                var pOrganizationId = DBHelper.GenerateDapperParameter("ORGANIZATIONID", request.OrganizationId, DbType.Int64);
                var pOutletId = DBHelper.GenerateDapperParameter("OUTLETID", request.OutletId, DbType.Int64);
                var pHeadCategoriesId = DBHelper.GenerateDapperParameter("HEADCATEGORIESID", request.HeadCategoriesId, DbType.Int64);
                var pFromDate = DBHelper.GenerateDapperParameter("FROMDATE", request.FromDate ?? "", DbType.String);
                var pToDate = DBHelper.GenerateDapperParameter("TODATE", request.ToDate ?? "", DbType.String);
                var pIsActive = DBHelper.GenerateDapperParameter("IsActive", request.IsActive, DbType.Boolean);

                var reportData = await _unit
                                       .DapperRepository
                                       .GetMultiResultsWithStoreProcedureAsync<ParametersRequest,
                                                                               HeadCategoryPrintReportResponse>("REPORT_GET_HEADCATEGORY",
                                                                                                                 DBHelper.GetDapperParms
                                                                                                                 (
                                                                                                                     pOrganizationId,
                                                                                                                     pOutletId,
                                                                                                                     pHeadCategoriesId,
                                                                                                                     pFromDate,
                                                                                                                     pToDate,
                                                                                                                     pIsActive
                                                                                                                 ),
                                                                                                             CommandType.StoredProcedure);
                if (reportData?.Item2?.Count > 0)
                {


                    // Setting Params For Report
                    var reportParams = new Dictionary<string, string>();
                    var date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);
                    reportParams.Add("pmrDate", date);
                    reportParams.Add("pmrOutletName", _currentLogin.OutletName ?? "");
                    reportParams.Add("pmrAddress", _currentLogin.Address ?? "");

                    reportParams.Add("pmrTitle", reportData?.Item1?.Title ?? "");
                    //
                    if (!request.IncludeZeroValue)
                    {
                        reportData?.Item2?.RemoveAll(c => c.ClosingBalance == 0);
                    }

                    var reportConfig = _rdlcConfigs
                                    ?.Value
                                    ?.Configs
                                    ?.Find(x => x.ReportName == ReportName.Accounts.SumSubCategoriesByHeadCategoryIdPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<HeadCategoryPrintReportResponse>()
                    {
                        Data = reportData?.Item2,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"HeadCategorySummaryPrint-{date}"
                    };
                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<DetailPrintReportResponse>> PrintHeadCategoryDetailLedger(ReportPrintHeadCategoryRequest request)
        {
            try
            {
                // Preparing Data For Head Category Print Report                
                var pOrganizationId = DBHelper.GenerateDapperParameter("ORGANIZATIONID", request.OrganizationId, DbType.Int64);
                var pOutletId = DBHelper.GenerateDapperParameter("OUTLETID", request.OutletId, DbType.Int64);
                var pHeadCategoriesId = DBHelper.GenerateDapperParameter("HEADCATEGORIESID", request.HeadCategoriesId, DbType.Int64);
                var pFromDate = DBHelper.GenerateDapperParameter("FROMDATE", request.FromDate ?? "", DbType.String);
                var pToDate = DBHelper.GenerateDapperParameter("TODATE", request.ToDate ?? "", DbType.String);
                var pIsActive = DBHelper.GenerateDapperParameter("IsActive", request.IsActive, DbType.Boolean);

                var reportData = await _unit
                                       .DapperRepository
                                       .GetMultiResultsWithStoreProcedureAsync<ParametersRequest,
                                                                               DetailPrintReportResponse>("REPORT_GET_HEADCATEGORY_DETAIL",
                                                                                                           DBHelper.GetDapperParms
                                                                                                           (
                                                                                                               pOrganizationId,
                                                                                                               pOutletId,
                                                                                                               pHeadCategoriesId,
                                                                                                               pFromDate,
                                                                                                               pToDate,
                                                                                                               pIsActive
                                                                                                           ),
                                                                                                           CommandType.StoredProcedure);
                if (reportData?.Item2?.Count > 0)
                {


                    // Setting Params For Report
                    var reportParams = new Dictionary<string, string>();
                    var date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);
                    reportParams.Add("pmrDate", date);
                    reportParams.Add("pmrOutletName", _currentLogin.OutletName ?? "");
                    reportParams.Add("pmrAddress", _currentLogin.Address ?? "");

                    reportParams.Add("pmrTitle", reportData?.Item1?.Title ?? "");
                    //
                    if (!request.IncludeZeroValue)
                    {
                        reportData?.Item2?.RemoveAll(c => c.ClosingBalance == 0);
                    }

                    var reportConfig = _rdlcConfigs
                                    ?.Value
                                    ?.Configs
                                    ?.Find(x => x.ReportName == ReportName.Accounts.HeadCategoryDetailPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<DetailPrintReportResponse>()
                    {
                        Data = reportData?.Item2,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"HeadCategoryDetailPrint-{date}"
                    };
                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<MainHeadPrintReportResponse>> PrintMainHeadLedger(ReportPrintMainHeadRequest request)
        {
            try
            {
                // Preparing Data For Main Head Print Report                
                var pOrganizationId = DBHelper.GenerateDapperParameter("ORGANIZATIONID", request.OrganizationId, DbType.Int64);
                var pOutletId = DBHelper.GenerateDapperParameter("OUTLETID", request.OutletId, DbType.Int64);
                var pMainHeadId = DBHelper.GenerateDapperParameter("MAINHEADID", request.MainHeadId, DbType.Int64);
                var pFromDate = DBHelper.GenerateDapperParameter("FROMDATE", request.FromDate ?? "", DbType.String);
                var pToDate = DBHelper.GenerateDapperParameter("TODATE", request.ToDate ?? "", DbType.String);
                var pIsActive = DBHelper.GenerateDapperParameter("ISACTIVE", request.IsActive, DbType.Boolean);

                // Setting Params For Report
                var reportParams = new Dictionary<string, string>();
                var date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);
                //

                var reportData = await _unit
                                        .DapperRepository
                                        .GetMultiResultsWithStoreProcedureAsync<ParametersRequest,
                                                                                MainHeadPrintReportResponse>("REPORT_GET_MAINHEAD",
                                                                                                              DBHelper.GetDapperParms
                                                                                                              (
                                                                                                                  pOrganizationId,
                                                                                                                  pOutletId,
                                                                                                                  pMainHeadId,
                                                                                                                  pFromDate,
                                                                                                                  pToDate,
                                                                                                                  pIsActive
                                                                                                              ),
                                                                                                CommandType.StoredProcedure);
                if (reportData?.Item2?.Count > 0)
                {


                    reportParams.Add("pmrOutletName", _currentLogin.OutletName ?? "");
                    reportParams.Add("pmrAddress", _currentLogin.Address ?? "");

                    // Setting Params For Report                        
                    reportParams.Add("pmrTitle", reportData.Item1.Title);
                    //
                    if (!request.IncludeZeroValue)
                    {
                        reportData.Item2.RemoveAll(c => c.ClosingBalance == 0);
                    }

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Accounts.SumHeadCategoriesByMainHeadIdPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<MainHeadPrintReportResponse>()
                    {
                        Data = reportData.Item2,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"MainHeadSummaryPrint-{date}"
                    };
                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<DetailPrintReportResponse>> PrintMainHeadDetailLedger(ReportPrintMainHeadRequest request)
        {
            try
            {
                // Preparing Data For Main Head Print Report                
                var pOrganizationId = DBHelper.GenerateDapperParameter("ORGANIZATIONID", request.OrganizationId, DbType.Int64);
                var pOutletId = DBHelper.GenerateDapperParameter("OUTLETID", request.OutletId, DbType.Int64);
                var pMainHeadId = DBHelper.GenerateDapperParameter("MAINHEADID", request.MainHeadId, DbType.Int64);
                var pFromDate = DBHelper.GenerateDapperParameter("FROMDATE", request.FromDate ?? "", DbType.String);
                var pToDate = DBHelper.GenerateDapperParameter("TODATE", request.ToDate ?? "", DbType.String);
                var pIsActive = DBHelper.GenerateDapperParameter("ISACTIVE", request.IsActive, DbType.Boolean);

                // Setting Params For Report
                var reportParams = new Dictionary<string, string>();
                var date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);
                reportParams.Add("pmrDate", date);

                //

                var reportData = await _unit
                                       .DapperRepository
                                       .GetMultiResultsWithStoreProcedureAsync<ParametersRequest,
                                                                               DetailPrintReportResponse>("REPORT_GET_MAINHEAD_DETAIL",
                                                                                                           DBHelper.GetDapperParms
                                                                                                           (
                                                                                                               pOrganizationId,
                                                                                                               pOutletId,
                                                                                                               pMainHeadId,
                                                                                                               pFromDate,
                                                                                                               pToDate,
                                                                                                               pIsActive
                                                                                                           ),
                                                                                                           CommandType.StoredProcedure);
                if (reportData?.Item2?.Count > 0)
                {


                    reportParams.Add("pmrOutletName", _currentLogin.OutletName ?? "");
                    reportParams.Add("pmrAddress", _currentLogin.Address ?? "");

                    // Setting Params For Report                        
                    reportParams.Add("pmrTitle", reportData.Item1.Title);
                    //
                    if (!request.IncludeZeroValue)
                    {
                        reportData.Item2.RemoveAll(c => c.ClosingBalance == 0);
                    }

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Accounts.MainHeadDetailPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<DetailPrintReportResponse>()
                    {
                        Data = reportData.Item2,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"MainHeadDetailPrint-{date}"
                    };
                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<ChartOfAccountsPrintReportResponse>> PrintChartOfAccoutns(long OrganizationId, long OutletId, string outletName, string address)
        {
            try
            {
                var query = $@"SELECT     
                                 mh.MainHeadsId    
                                 , mh.Name AS MainHeadsName      
                                 , hc.HeadCategoriesId    
                                 , ISNULL(hc.Name,'') AS HeadCategoriesName      
                                 , ISNULL(sc.SubCategoriesId,0) SubCategoriesId    
                                 , ISNULL(sc.Name,'') AS SubCategoriesName      
                                 , ISNULL(pa.PostingAccountsId,0) PostingAccountsId    
                                 , ISNULL(pa.Name, '') AS PostingAccountsName      
                                 , ISNULL(pa.OpeningDebit,0) OpeningDebit    
                                 , ISNULL(pa.OpeningCredit,0) OpeningCredit
                                FROM MainHeads AS MH      
                                LEFT JOIN HeadCategories hc     
                                 ON hc.MainHeadsId = mh.MainHeadsId      
                                LEFT JOIN SubCategories AS SC    
                                 ON HC.HeadCategoriesId = sc.HeadCategoriesId      
                                LEFT JOIN PostingAccounts AS pa     
                                 ON SC.SubCategoriesId = pa.SubCategoriesId";

                var reportData = await _unit.DapperRepository.GetListQueryAsync<ChartOfAccountsPrintReportResponse>(query);

                if (reportData?.Count > 0)
                {
                    // Setting Params For Report
                    var reportParams = new Dictionary<string, string>();
                    reportParams?.Add("pmrOutletName", _currentLogin.OutletName ?? "");
                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    reportParams.Add("pmrOutletImage", baseUrl + _currentLogin?.OutletImage ?? "-");
                    reportParams?.Add("pmrAddress", _currentLogin.Address ?? "");

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Accounts.ChartOfAccountsPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<ChartOfAccountsPrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = "ChartOfAccountPrint-" + DateTime.Now.Date.ToString("dd-MMM-yyyy")
                    };

                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<PayableReceiveablePrintReportResponse>> PrintPayableReceiveable(long OrganizationId, long OutletId, string outletName, string address)
        {
            try
            {
                var query = $@"";

                var reportData = await _unit.DapperRepository.GetListQueryAsync<PayableReceiveablePrintReportResponse>(query);

                if (reportData?.Count > 0)
                {
                    // Setting Params For Report
                    var reportParams = new Dictionary<string, string>();
                    reportParams.Add("pmrOutletName", _currentLogin.OutletName ?? "");
                    reportParams.Add("pmrAddress", _currentLogin.Address ?? "");
                    //

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Accounts.PayableReceiveablePrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<PayableReceiveablePrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = "Payable_ReceiveablePrint-" + DateTime.Now.Date.ToString("dd-MMM-yyyy")
                    };

                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<DailyVouchersPrintReportResponse>> PrintDailyVouchers(ReportPrintDailyVouchersRequest request)
        {
            try
            {
                var query = $@"SELECT                                    
                                 V.VouchersMasterId                    
                                 , V.voucherDate                    
                                 , V.ReferenceNo                    
                                 , VT.Name AS VoucherTypeName              
                                 , PostingAccountsName              
                                 , Remarks                    
                                 , V.Narration                    
                                 , DebitAmount                    
                                 , CreditAmount   
                                 , O.OutletName  
                                 , O.Address AS OutletAddress  
                                FROM V_Vouchers AS V  
                                INNER JOIN Outlet AS O  
                                 ON O.OutletId = V.OutletId  
                                 AND O.IsActive = 1  
                                INNER JOIN VoucherType AS VT     
                                ON V.VOUCHERTYPEID = VT.VoucherTypeId                    
                                WHERE V.VoucherDate BETWEEN     
                                '{request.FromDate}' AND '{request.ToDate}'             
                                AND                   
                                (                  
                                 {request.VoucherTypeId} = 0                  
                                 OR                  
                                 V.VoucherTypeId = {request.VoucherTypeId}                  
                                )                                     
                                AND V.OutletId = {request.OutletId}                
                                AND V.IsActive = CASE WHEN {(request.IsActive == true ? 1 : 0)} = 1 THEN V.IsActive ELSE 1  END                
                                ORDER BY     
                                 V.VoucherDate    
                                 , V.VouchersMasterId    
                                 , V.CreditAmount";

                var reportData = await _unit.DapperRepository.GetListQueryAsync<DailyVouchersPrintReportResponse>(query);

                if (reportData?.Count > 0)
                {
                    var reportParams = new Dictionary<string, string>();
                    var date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);
                    reportParams.Add("pmrDate", date);
                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    reportParams.Add("pmrOutletImage", baseUrl + _currentLogin?.OutletImage ?? "-");
                    reportParams.Add("pmrOutlet_Address", _currentLogin.OutletName + " " + _currentLogin.Address ?? "");

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Accounts.DailyVouchersPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<DailyVouchersPrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"DailyVouchersPrint-{date}"
                    };

                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<DailyVouchersPrintReportResponse>> PrintDailyPaymentReceipt(ReportPrintDailyVouchersRequest request)
        {
            try
            {
                var query = $@"SELECT *FROM           
                                    (SELECT v.VouchersMasterId          
                                       , v.voucherDate          
                                       , v.ReferenceNo          
                                       , vt.Name VoucherTypeName          
                                       , Remarks           
                                       , v.Narration                  
                                       , DebitAmount as TotalAmount            
                                       , PostingAccountsName  
                                       , O.OutletName  
                                     , O.Address AS OutletAddress  
                                       FROM V_Vouchers v  
                                       INNER JOIN Outlet AS O  
                                     ON O.OutletId = V.OutletId   
                                     AND O.IsActive = 1  
                                       INNER JOIN VoucherType vt                  
                                       ON v.VOUCHERTYPEID=vt.VoucherTypeId                  
                                       WHERE v.VoucherDate                  
                                       BETWEEN '{request.FromDate}' AND '{request.ToDate}'         
                                       AND v.VoucherStatus=2      
                                       AND v.VoucherTypeId=1-- IN(CASE @VOUCHERTYPEID WHEN 2 THEN 0 ELSE 1 END)              
                                       AND v.OutletId= {request.OutletId}          
                                       AND DebitAmount>0          
                                       AND V.IsActive = CASE WHEN {(request.IsActive == true ? 1 : 0)} = 1 THEN V.IsActive ELSE 1  END          
        
                                    UNION ALL          
                                    SELECT       
                                     v.VouchersMasterId           
                                     , v.voucherDate          
                                     , v.ReferenceNo           
                                     , vt.Name VoucherTypeName          
                                     , Remarks           
                                     , v.Narration                  
                                     , CreditAmount  as TotalAmount          
                                     , PostingAccountsName  
                                     , O.OutletName  
                                     , O.Address AS OutletAddress  
                                    FROM V_Vouchers v  
                                    INNER JOIN Outlet AS O  
                                     ON O.OutletId = V.OutletId  
                                     AND O.IsActive = 1  
                                    INNER JOIN VoucherType vt                  
                                    ON v.VOUCHERTYPEID=vt.VoucherTypeId                  
                                    WHERE v.VoucherDate                  
                                    BETWEEN '{request.FromDate}' AND '{request.ToDate}'                  
                                    AND v.VoucherTypeId=2-- IN(CASE @VOUCHERTYPEID WHEN 1 THEN 0 ELSE 2 END)                
                                    AND v.OutletId= {request.OutletId}          
                                    AND CreditAmount>0        
                                    AND V.IsActive = CASE WHEN {(request.IsActive == true ? 1 : 0)} = 1 THEN V.IsActive ELSE 1  END) tbl       
                                       ORDER BY   
                                     VoucherTypeName  
                                     , VoucherDate";

                var reportData = await _unit.DapperRepository.GetListQueryAsync<DailyVouchersPrintReportResponse>(query);

                if (reportData?.Count > 0)
                {
                    var sumDebit = reportData.Where(c => c.VoucherTypeName == "Payment")?.Sum(c => c.TotalAmount) ?? 0;
                    var sumCredit = reportData.Where(c => c.VoucherTypeName == "Receipt")?.Sum(c => c.TotalAmount) ?? 0;

                    reportData.ForEach(data =>
                    {
                        data.DebitAmount = sumDebit;
                        data.CreditAmount = sumCredit;
                    });

                    var reportParams = new Dictionary<string, string>();
                    var date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);
                    reportParams.Add("pmrDate", date);
                    reportParams.Add("pmrOutlet_Address", _currentLogin.OutletName ?? "" + " " + _currentLogin.Address ?? "");

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Accounts.DailyVouchersPaymentReceiptPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<DailyVouchersPrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"DailyPayment_Or_ReceiptPrint-{date}"
                    };

                    return dataParms;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<DailyVouchersPrintReportResponse>> PrintVoucherList(ReportPrintDailyVouchersRequest request)
        {
            try
            {
                var query = $@"SELECT v.VouchersMasterId              
                                  , v.voucherDate              
                                  , v.ReferenceNo              
                                  , vt.Name VoucherTypeName              
                                  , Remarks               
                                  , v.Narration                      
                                  , DebitAmount          
                                  , CreditAmount        
                                  , PostingAccountsName  
                                  , O.OutletName  
                                 , O.Address AS OutletAddress  
                                FROM V_Vouchers v  
                                INNER JOIN Outlet AS O  
                                 ON O.OutletId = V.OutletId  
                                 AND O.IsActive = 1  
                                INNER JOIN VoucherType vt ON v.VOUCHERTYPEID=vt.VoucherTypeId                      
                                WHERE O.OutletId = {request.OutletId}                      
                                   AND                       
                                   (                      
                                   {request.VoucherTypeId} = 0                      
                                   OR                      
                                   v.VoucherTypeId = {request.VoucherTypeId}                      
                                   )                                  
                                   AND VoucherDate BETWEEN '{request.FromDate}' AND '{request.ToDate}'            
                                   ORDER BY v.VoucherDate";

                var reportData = await _unit.DapperRepository.GetListQueryAsync<DailyVouchersPrintReportResponse>(query);

                if (reportData?.Count > 0)
                {
                    var reportParams = new Dictionary<string, string>();
                    var date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);
                    reportParams.Add("pmrDate", date);
                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    reportParams.Add("pmrOutletImage", baseUrl + _currentLogin?.OutletImage ?? "-");
                    reportParams.Add("pmrOutlet_Address", _currentLogin.OutletName + " " + _currentLogin.Address ?? "");

                    var downloadFileName = "VoucherListPrint-" + date;

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Accounts.VoucherListPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<DailyVouchersPrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"VoucherListPrint-{date}"
                    };

                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<TrialBalancePrintReportResponse>> PrintTrialBalance(ReportPrintTrialBalanceRequest request)
        {
            try
            {

                var reportData = await Function_TrialBalance(request);

                if (reportData?.Count > 0)
                {
                    var reportParams = new Dictionary<string, string>();
                    var date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);
                    reportParams.Add("pmrDate", date);
                    reportParams.Add("pmrOutletName", _currentLogin.OutletName ?? "abs");
                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    reportParams.Add("pmrOutletImage", baseUrl + _currentLogin?.OutletImage ?? "-");
                    reportParams.Add("pmrAddress", _currentLogin.Address ?? "dkhjd");

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Accounts.TrialBalancePrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<TrialBalancePrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = "TrialBalancePrint"
                    };

                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<TrialBalancePrintReportResponse>> PrintBalanceSheet(ReportPrintTrialBalanceRequest request)
        {
            try
            {
                var reportData = await Function_TrialBalance(request);

                var balanceSheet = reportData
                                       .Where(c => c.MainHeadsId == 1
                                       || c.MainHeadsId == 2
                                       || c.MainHeadsId == 3).ToList();

                var totalIncome = -1 * reportData.Where(c => c.MainHeadsId == 4).Sum(c => c.ClosingBalance);
                var totalExpense = reportData.Where(c => c.MainHeadsId == 5).Sum(c => c.ClosingBalance);
                var pl = totalIncome - totalExpense;

                var owenerEquity = reportData.FirstOrDefault(c => c.PostingAccountsId == request.OwnerEquityId); // parameter
                if (owenerEquity != null)
                {
                    balanceSheet.Add(new TrialBalancePrintReportResponse
                    {
                        MainHeadsId = owenerEquity.MainHeadsId,   // Owener Equity
                        MainHeadsName = owenerEquity.MainHeadsName,
                        HeadCategoriesId = owenerEquity.HeadCategoriesId,
                        SubCategoriesId = owenerEquity.SubCategoriesId,
                        PostingAccountsId = owenerEquity.PostingAccountsId,
                        PostingAccountsName = "P & L",
                        DebitAmount = pl > 0 ? pl : 0,
                        CreditAmount = pl < 0 ? pl : 0,
                        ClosingBalance = pl
                    });
                }

                /**************************/

                if (balanceSheet?.Count > 0)
                {
                    // Setting Params For Report
                    var date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);

                    var reportParams = new Dictionary<string, string>();
                    reportParams.Add("pmrOutletName", _currentLogin.OutletName ?? "");
                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    reportParams.Add("pmrOutletImage", baseUrl + _currentLogin?.OutletImage ?? "-");
                    reportParams.Add("pmrAddress", _currentLogin.Address ?? "");
                    reportParams.Add("pmrDate", date ?? "");


                    var downloadFileName = "BalanceSheetPrint-" + date;

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Accounts.BalanceSheetPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<TrialBalancePrintReportResponse>()
                    {
                        Data = balanceSheet,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"BalanceSheetPrint-{date}"
                    };

                    return dataParms;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<TrialBalancePrintReportResponse>> PrintProfitAndLoss(ReportPrintTrialBalanceRequest request)
        {
            try
            {
                var reportData = await Function_TrialBalance(request);
                var plData = reportData
                         .Where(c => c.MainHeadsId == 4
                                || c.MainHeadsId == 5).ToList();

                if (plData?.Count > 0)
                {
                    var date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);

                    var reportParams = new Dictionary<string, string>();
                    reportParams.Add("pmrOutletName", _currentLogin.OutletName ?? "");
                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    reportParams.Add("pmrOutletImage", baseUrl + _currentLogin?.OutletImage ?? "-");
                    reportParams.Add("pmrAddress", _currentLogin.Address ?? "");
                    reportParams.Add("pmrDate", date ?? "");


                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Accounts.ProfitAndLossPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<TrialBalancePrintReportResponse>()
                    {
                        Data = plData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"Profit_and_LossPrint-{date}"
                    };

                    return dataParms;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region  Stock Reports Print

        public async Task<ReportDataParms<ItemRequest>> PrintItems()
        {
            try
            {
                string query = $@"SELECT itm.*
                                    ,c.ItemCategoryName,c.ItemTypeId, t.ItemTypeName, UnitName
                                            FROM Item itm
                                            INNER JOIN ItemCategory c ON itm.ItemCategoryId = c.ItemCategoryId
                                            INNER JOIN ItemType t ON c.ItemTypeId = t.ItemTypeId
                                            INNER JOIN Unit u ON itm.UnitId= u.UnitId ";

                var reportData = await _unit
                                       .DapperRepository
                                       .GetListQueryAsync<ItemRequest>(query);

                var reportParams = new Dictionary<string, string>();

                if (reportData?.Count > 0)
                {
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.StockManagement.ItemsPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<ItemRequest>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"Items Print"
                    };

                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<InvoicePrintReportResponse>> PrintInvoice(long invoiceMasterId)
        {
            try
            {
                var pId = DBHelper.GenerateDapperParameter("INVOICEMASTERID", invoiceMasterId, DbType.Int64);

                string query = $@"SELECT *
                                    FROM V_InvoicePrint
                                    WHERE InvoiceMasterId = {invoiceMasterId}";

                var reportData = await _unit
                                       .DapperRepository
                                       .GetListQueryAsync<InvoicePrintReportResponse>(query);

                var reportParams = new Dictionary<string, string>();
                var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                reportParams.Add("prmOutletName", _currentLogin.OutletName ?? "Outlet Name");
                reportParams.Add("pmrOutletImage", baseUrl + _currentLogin?.OutletImage ?? "-");
                reportParams.Add("pmrBaseUrl", baseUrl ?? "-");

                if (reportData?.Count > 0)
                {
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.StockManagement.InvoicePrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<InvoicePrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"InvoicePrint-{invoiceMasterId}"
                    };

                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<InvoicePrintReportResponse>> PrintPurchaseOrder(long purchaseOrderMasterId)
        {
            try
            {
                string query = $@"SELECT *
                                    FROM V_PurchaseOrderPrint
                                    WHERE PurchaseOrderMasterId = {purchaseOrderMasterId}";

                var reportData = await _unit
                                       .DapperRepository
                                       .GetListQueryAsync<InvoicePrintReportResponse>(query);

                var reportParams = new Dictionary<string, string>();
                var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                reportParams.Add("prmOutletName", _currentLogin.OutletName ?? "Outlet Name");
                reportParams.Add("pmrOutletImage", _currentLogin?.OutletImage ?? "-");
                reportParams.Add("pmrBaseUrl", baseUrl ?? "-");
                if (reportData?.Count > 0)
                {
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.StockManagement.PurchaseOrderPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<InvoicePrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"PurchaseOrderPrint-{purchaseOrderMasterId}"
                    };

                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }


        public async Task<ReportDataParms<InvoicePrintReportResponse>> PrintDailyTransactions(InvoiceParameterRequest request)
        {
            try
            {

                string date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);

                var query = $@"SELECT *FROM V_InvoicePrint
                               WHERE DocumentTypeId ={request.DocumentTypeId}
                                AND InvoiceDate BETWEEN  {"'" + request.FromDate + "'"} AND {"'" + request.ToDate + "'"}";


                var reportData = await _unit
                                       .DapperRepository
                                       .GetListQueryAsync<InvoicePrintReportResponse>(query);

                if (reportData?.Count > 0)
                {

                    var reportParams = new Dictionary<string, string>();

                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    reportParams.Add("date", date);
                    reportParams.Add("prmOutletName", _currentLogin.OutletName ?? "Outlet Name");
                    reportParams.Add("pmrOutletImage", _currentLogin?.OutletImage ?? "-");
                    reportParams.Add("pmrBaseUrl", baseUrl ?? "-");
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.StockManagement.DailyTransactionsPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<InvoicePrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"DailyTransactionsPrint"
                    };

                    return dataParms;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<InvoicePrintReportResponse>> PrintItemsLedger(InvoiceParameterRequest request)
        {
            try
            {
                var query = $@"SELECT  opening.*        
                                        , ISNULL(detail.InvoiceDetailId,0) AS Id        
                                        , ISNULL(detail.InvoiceMasterId,0)AS InvoiceMasterId        
                                        , ISNULL(detail.InvoiceDate,GETDATE())as InvoiceDate        
                                        , ISNULL(ParticularId,0) as ParticularId      
                                        , ParticularName        
                                        , ISNULL(DocumentTypeId,0) as DocumentTypeId       
                                        , detail.ReferenceNo  
                                        , detail.Remarks    
                                        , InvoiceSerialNo
                                        , ISNULL(detail.Quantity,0)as Quantity
                                        , ISNULL(detail.Price,0)as Price
                                        , ISNULL(detail.Amount,0)as Amount              
		                                    , opening.OpeningQuantity+SUM(ISNULL(CASE TransactionType WHEN 1 THEN detail.Quantity 
															WHEN 2 THEN -detail.Quantity ELSE 0 END,0)) OVER(partition by opening.ItemId         
															ORDER BY opening.ItemId, detail.InvoiceDate,detail.InvoiceDetailId) AS RunningTotal   

			                                    , OPENING.OPENINGAMOUNT+SUM(ISNULL(CASE TransactionType WHEN 1 THEN detail.Amount 
																WHEN 2 THEN -detail.Amount ELSE 0 END,0)) OVER(PARTITION BY OPENING.ItemId         
																ORDER BY OPENING.ItemId, DETAIL.INVOICEDATE,DETAIL.InvoiceDetailId) AS RunningAmount
	                                                        ,TransactionType			
                                                                FROM
	                                                            (
		                                                            SELECT *FROM       
			                                                            (
			                                                            SELECT p.ItemId
				                                                        ,ItemName   
				                                                        ,PartNo
				                                                        ,ISNULL(OpeningQuantity,0)+ISNULL(bf.bfQuantity,0) as OpeningQuantity        
				                                                        ,ISNULL(OpeningAmount,0)+ISNULL(bf.bfAmount,0) as OpeningAmount        
				                                                            FROM Item as P                      
			                                                            LEFT JOIN(        
				                                                            SELECT ItemId		/** Opening*******/  
				                                                            , SUM(OpeningQuantity) as OpeningQuantity  
				                                                            , SUM(OpeningQuantity*OpeningPrice) as OpeningAmount  
					                                                        FROM ItemOpening       
				                                                            {(request.ItemIds?.Length > 0 ? $"WHERE ItemId IN({request.ItemIds})" : "")}
				                                                            GROUP by ItemId
				                                                        ) as opening   
				                                                        ON p.ItemId=opening.ItemId  
			                                                            LEFT JOIN(					/** BF *******/ 
					                                                            SELECT v.ItemId  
					                                                            , SUM(Quantity) as bfQuantity  
					                                                            , SUM(Amount)as bfAmount        
					                                                        FROM V_InvoicePrint v            
					                                                 WHERE v.InvoiceDate < '{request.FromDate}'
					                                                 {(request.ItemIds?.Length > 0 ? $"AND ItemId IN({request.ItemIds})" : "")}
					                                                 GROUP by v.ItemId 
				                                                ) as bf        
			                                                    ON p.ItemId=bf.ItemId    
			                                                     {(request.ItemIds?.Length > 0 ? $"WHERE p.ItemId IN({request.ItemIds})" : "")}
			                                                    ) as broughtForward        
	                                                                )as opening        
                                                                    LEFT JOIN        
                                                                    (        
	                                                                SELECT        
		                                                                v.InvoiceDetailId        
		                                                                , v.ItemId        
		                                                                , v.InvoiceMasterId        
		                                                                , v.InvoiceDate        
		                                                                , v.ReferenceNo, v.Remarks                                              
		                                                                , v.Quantity
		                                                                , v.Price
		                                                                , v.Amount
		                                                                , v.DocumentTypeId      
		                                                                , v.ParticularId      
		                                                                , v.ParticularName    
		                                                                , TransactionType
                                                                        , InvoiceSerialNo
		                                                                FROM V_InvoicePrint v          
			                                                WHERE v.InvoiceDate BETWEEN '{request.FromDate}'
                                                            AND '{request.ToDate}'
			                                                    {(request.ItemIds?.Length > 0 ? $"AND ItemId IN({request.ItemIds})" : "")}
                                                        ) as detail          
                                                            ON opening.ItemId=detail.ItemId
                                                            ORDER BY                                             
                                                            opening.ItemId,detail.InvoiceDate,detail.InvoiceDetailId ASC  ";

                var reportData = await _unit
                                       .DapperRepository
                                       .GetListQueryAsync<InvoicePrintReportResponse>(query);


                if (reportData?.Count > 0)
                {

                    var reportParams = new Dictionary<string, string>();

                    string date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);
                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    reportParams.Add("date", date);
                    reportParams.Add("prmOutletName", _currentLogin.OutletName ?? "Outlet Name");
                    reportParams.Add("pmrOutletImage", _currentLogin?.OutletImage ?? "-");
                    reportParams.Add("pmrBaseUrl", baseUrl ?? "-");
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.StockManagement.ItemsLedgerPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<InvoicePrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"ItemsLedgerPrint-{date}"
                    };

                    return dataParms;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<InvoicePrintReportResponse>> PrintPartyLedger(InvoiceParameterRequest request)
        {
            try
            {
                var query = $@"SELECT ItemId,ParticularId
                                            ,InvoiceMasterId
                                            ,InvoiceSerialNo
                                            ,DocumentTypeId
                                            ,InvoiceDate
                                            ,ReferenceNo
                                            ,ItemName
		                                    ,UnitName
                                            ,ParticularType
                                            ,ParticularName
                                            ,Quantity
		                                    ,Price  
                                            ,Amount
                                            FROM V_InvoicePrint 
                                            WHERE InvoiceDate BETWEEN '{request.FromDate}'
                                                            AND '{request.ToDate}'
                                            AND ParticularId ={request.ParticularId}";

                var reportData = await _unit
                                      .DapperRepository
                                      .GetListQueryAsync<InvoicePrintReportResponse>(query);

                if (reportData?.Count > 0)
                {
                    var reportParams = new Dictionary<string, string>();
                    string date = DateHelper.GetReportFormatDate(request.FromDate, request.ToDate);
                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    reportParams.Add("prmDate", date);
                    reportParams.Add("prmOutletName", _currentLogin.OutletName ?? "Outlet Name");
                    reportParams.Add("prmOutletAddress", request.Address ?? "");

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.StockManagement.PartyLedgerPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<InvoicePrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"VendorsLedgerPrint-{date}"
                    };

                    return dataParms;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<InvoicePrintReportResponse>> PrintStockSummary(InvoiceParameterRequest request)
        {
            try
            {
                var query = $@"SELECT 
	                                itm.ItemId
	                            , ItemName
                                , UnitName
								, itm.PartNo
                                , ItemCategoryName AS CategoryName
	                            , ISNULL(detail.Quantity,0) as Quantity
	                            , LastPrice AS LastPurchasePrice
                                FROM V_Item as itm
	                            LEFT JOIN
		                            (
		                                SELECT 
				                            ItemId
			                                , SUM(ISNULL(Quantity,0)) as Quantity
			                                FROM
			                                (
				                                SELECT ItemId as ItemId
					                            , SUM(OpeningQuantity) as Quantity
					                            FROM ItemOpening  
						                        {(request.ItemIds?.Length > 0 ? $"WHERE ItemId IN({request.ItemIds})" : "")}
                                                GROUP by ItemId
				                            UNION ALL
					                            SELECT v.ItemId
					                            , SUM(ISNULL(CASE TransactionType WHEN 1 THEN Quantity 
														                            WHEN 2 THEN -Quantity ELSE 0 END,0)) 
														                            as Quantity
					                            FROM V_InvoicePrint v      
					                            WHERE v.InvoiceDate<={"'" + request.ToDate + "'"}
                                                {(request.ItemIds?.Length > 0 ? $"AND ItemId IN({request.ItemIds})" : "")}
                                                GROUP by v.ItemId
				                            ) as tbl
					                            GROUP by ItemId

		                                ) as detail
				                            ON itm.ItemId=detail.ItemId
				                            {(request.ItemIds?.Length > 0 ? $"WHERE ItemId IN({request.ItemIds})" : "")}
                                            ORDER BY itm.ItemId ";


                var reportData = await _unit
                                       .DapperRepository
                                       .GetListQueryAsync<InvoicePrintReportResponse>(query);


                if (reportData?.Count > 0)
                {

                    if (request.IsIncludeZeroValue == false)
                    {
                        reportData.RemoveAll(c => c.Quantity == 0);
                    }

                    var reportParams = new Dictionary<string, string>();

                    var date = DateHelper.GetDateFormat(DateHelper.GetDateFormat(request.ToDate), false, false, DateFormats.DefaultFormat);
                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    reportParams.Add("date", date.ToString());
                    reportParams.Add("prmOutletName", _currentLogin.OutletName ?? "Outlet Name");
                    reportParams.Add("pmrOutletImage", _currentLogin?.OutletImage ?? "-");
                    reportParams.Add("pmrBaseUrl", baseUrl ?? "-");

                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.StockManagement.StockSummaryPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<InvoicePrintReportResponse>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"Stock Summary Print-{date}"
                    };

                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        #endregion


        #region Production reports
        public async Task<ReportDataParms<ProductionProcessRequest>> PrintOrderStatus(ProductionParameterRequest request)
        {
            try
            {
                string query = $@"SELECT m.DeliveryDate
		                                , m.OrderMasterId 
										, m.OrderName
		                                , d.ProductId
		                                , ProductName
										, d.ProductSizeId
										, ps.ProductSizeName
		                                , production.ProcessTypeName
		                                , SUM(d.Quantity) as OrderQuantity
		                                , ISNULL(production.IssueQuantity,0) as IssueQuantity
		                                , ISNULL(production.ReceiveQuantity,0) as ReceiveQuantity
		                                , ISNULL(production.BalanceQuantity,0)as BalanceQuantity
		                                , ISNULL(packing.Warehouse,'Warehouse') as Warehouse
		                                , ISNULL(packing.WarehouseQuantity,0) as WarehouseQuantity
	                                FROM OrderDetail d 
									INNER JOIN OrderMaster m ON d.OrderMasterId=m.OrderMasterId
	                                INNER JOIN Product pr ON d.ProductId=pr.ProductId
	                                INNER JOIN ProductSize as ps on d.ProductSizeId=ps.ProductSizeId

	                                LEFT JOIN
	                                (
		                                SELECT pp.ProductId
						                , pp.ProductSizeId
		                                , pt.ProcessTypeName
		                                , pp.OrderMasterId
		                                , SUM(pp.IssueQuantity) as IssueQuantity
		                                , SUM(pp.ReceiveQuantity)as ReceiveQuantity
		                                , SUM(pp.IssueQuantity)-SUM(pp.ReceiveQuantity)as BalanceQuantity
	                                FROM ProductionProcess pp 
					                INNER JOIN Process p ON pp.ProcessId=p.ProcessId
		                                INNER JOIN ProcessType pt
		                                ON p.ProcessTypeId=pt.ProcessTypeId
		                                --WHERE p.ProcessTypeId!=31
						                where pp.OrderMasterId={request.OrderMasterId}
		                                GROUP BY pp.ProductId,pp.ProductSizeId
												, pt.ProcessTypeName, pp.OrderMasterId
	                                )as production
	                                ON d.OrderMasterId=production.OrderMasterId
	                                AND d.ProductId=production.ProductId
									AND d.ProductSizeId=production.ProductSizeId
	                                LEFT JOIN
	                                (
		                                SELECT pp.ProductId
											, pp.ProductSizeId
		                                , SUM(pp.ReceiveQuantity)as WarehouseQuantity
		                                , pt.ProcessTypeName as Warehouse
		                                , pp.OrderMasterId
	                                FROM ProductionProcess pp
		                                INNER JOIN Process p ON pp.ProcessId=p.ProcessId 
		                                INNER JOIN ProcessType pt ON pt.ProcessTypeId=p.ProcessTypeId
		                                WHERE p.ProcessTypeId={request.ProcessTypeId}
						                AND pp.OrderMasterId={request.OrderMasterId}
		                                GROUP BY pp.ProductId,pp.ProductSizeId, pt.ProcessTypeName, pp.OrderMasterId
	                                )as packing
	                                ON d.OrderMasterId=packing.OrderMasterId
	                                AND d.ProductId=packing.ProductId
										AND d.ProductSizeId=packing.ProductSizeId
	                                 where d.OrderMasterId={request.OrderMasterId}
                                     GROUP BY
					                 m.OrderMasterId
									 , m.OrderName
		                                , d.ProductId
		                                , ProductName
										, d.ProductSizeId
										, ps.ProductSizeName
		                                , m.DeliveryDate
		                                , production.ProcessTypeName
		                                , packing.Warehouse
		                                , packing.WarehouseQuantity
						                ,production.IssueQuantity
						                ,production.ReceiveQuantity
						                ,production.BalanceQuantity";

                var reportData = await _unit
                                       .DapperRepository
                                       .GetListQueryAsync<ProductionProcessRequest>(query);

                var reportParams = new Dictionary<string, string>();

                if (reportData?.Count > 0)
                {
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Production.OrderStatusMatrixPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<ProductionProcessRequest>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"Order Status Matrix Print"
                    };

                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<ProductionProcessRequest>> PrintArticleBalanceEmployeeWise(ProductionParameterRequest request)
        {
            try
            {
                string query = $@"SELECT pp.ProductId,p.ProductName,pp.ProductSizeId
				                        , pp.EmployeeId,emp.EmployeeName
				                        , pp.ProcessId,pt.ProcessTypeName
				                        , SUM(IssueQuantity) as IssueQuantity
				                        , SUM(ReceiveQuantity) as ReceiveQuantity
				                        , SUM(IssueQuantity)-SUM(ReceiveQuantity) as BalanceQuantity
		                                    FROM ProductionProcess as pp
		                                    INNER JOIN Product p ON pp.ProductId=p.ProductId
											INNER JOIN Process pro ON pp.ProcessId=pro.ProcessId
		                                    INNER JOIN ProcessType pt ON pro.ProcessTypeId=pt.ProcessTypeId
		                                    INNER JOIN Employee emp ON pp.EmployeeId=emp.EmployeeId
		                                    WHERE pp.OrderMasterId={request.OrderMasterId}
		                                    GROUP by  pp.ProductId,p.ProductName
				                        , pp.EmployeeId,emp.EmployeeName
				                        ,pp.ProcessId, pro.ProcessTypeId,pt.ProcessTypeName,pp.ProductSizeId
		                        HAVING SUM(IssueQuantity)-SUM(ReceiveQuantity)>0";

                var reportData = await _unit
                                       .DapperRepository
                                       .GetListQueryAsync<ProductionProcessRequest>(query);

                var reportParams = new Dictionary<string, string>();

                if (reportData?.Count > 0)
                {
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Production.ArticleBalanceEmployeeWisePrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<ProductionProcessRequest>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"Order Status Matrix Print"
                    };

                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<ProductionProcessRequest>> PrintIssueSlip(ProductionParameterRequest request)
        {
            try
            {
                string query = $@"SELECT  
	                                    E.EmployeeName
	                                    , PP.EmployeeId
	                                    , E.DepartmentsName
	                                    , PP.ProductSizeId
	                                    , PP.OrderMasterId
	                                    , PP.ProductId
	                                    , SUM(PP.IssueQuantity) - SUM(PP.ReceiveQuantity) AS IssueQuantity
	                                    , PP.ProcessRate
	                                    , OM.OrderName
	                                    , P.ProductName
	                                    , PS.ProductSizeName
	                                    , PT.ProcessTypeName
                                    FROM ProductionProcess AS PP
                                    INNER JOIN V_EMPLOYEE AS E
	                                    ON E.EmployeeId = PP.EmployeeId
                                    INNER JOIN OrderMaster AS OM
	                                    ON OM.OrderMasterId = PP.OrderMasterId
                                    INNER JOIN Product AS P
	                                    ON P.ProductId = PP.ProductId
                                    INNER JOIN ProductSize AS PS
	                                    ON PS.ProductSizeId = PP.ProductSizeId
                                    INNER JOIN ProcessType AS PT
	                                    ON PT.ProcessTypeId = PP.ProcessTypeId
                                    WHERE PP.EmployeeId = {request.EmployeeId}
                                    AND PP.ProductId = {request.ProductId} 
                                    AND PP.ProductSizeId = {request.ProductSizeId}
                                    AND PP.IsPosted = 0
                                    GROUP BY 
	                                    E.EmployeeName
	                                    , PP.EmployeeId
	                                    , E.DepartmentsName
	                                    , PP.ProductSizeId
	                                    , PP.OrderMasterId
	                                    , PP.ProductId
	                                    , PP.ProcessRate
	                                    , OM.OrderName
	                                    , P.ProductName
	                                    , PS.ProductSizeName
	                                    , PT.ProcessTypeName
                                    HAVING SUM(IssueQuantity) - SUM(ReceiveQuantity) > 0";

                var reportData = await _unit
                                       .DapperRepository
                                       .GetListQueryAsync<ProductionProcessRequest>(query);

                var reportParams = new Dictionary<string, string>();
                reportParams.Add("pmrOutletName", _currentLogin.OutletName ?? "");
                if (reportData?.Count > 0)
                {
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Production.IssueSlipPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<ProductionProcessRequest>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = $"Issue Slip Print"
                    };

                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<OrderConsumptionRequest>> PrintOrderConsumption(ProductionParameterRequest request)
        {
            try
            {
                var query = $@"SELECT m.OrderMasterId
		                                , m.OrderName
		                                , m.OrderDate
		                                , m.DeliveryDate
		                                , d.ProductId
		                                , p.ProductName
		                                , ps.ProductSizeName
		                                , d.Quantity as ProductQuantity
		                                , materialPlaning.ItemId
		                                , materialPlaning.ItemName
		                                , ISNULL(materialPlaning.Quantity,0) as ItemQuantity
		                                , ISNULL(materialPlaning.Price,0) as ItemPrice
		                                , materialPlaning.UnitName
                                        , ISNULL(materialPlaning.IsManualPrice,0)as IsManualPrice
		                                , d.Quantity*ISNULL(materialPlaning.Quantity,0) as RequiredMaterialQuantity
                                        , CONCAT(d.ProductId,d.ProductSizeId) as ArticleId
                                        , ISNULL(m.OtherCost,0) AS OtherCost
		                                FROM OrderMaster m
	                                INNER JOIN OrderDetail d ON m.OrderMasterId=d.OrderMasterId
	                                INNER JOIN ProductSize ps  ON d.ProductSizeId=ps.ProductSizeId
	                                INNER JOIN Product p  ON d.ProductId=p.ProductId
	                                LEFT JOIN  -- material planning
		                                (
			                                SELECT 
					                                OrderMasterId
					                                , ProductId
					                                , ProductSizeId
					                                , d.ItemId
					                                , i.ItemName
					                                , d.Quantity
					                                , d.Price
					                                , u.UnitName
					                                , d.IsManualPrice
				                                FROM PlaningMaster m
					                                INNER JOIN PlaningDetail d ON m.PlaningMasterId=d.PlaningMasterId
					                                INNER JOIN Item i ON d.ItemId=i.ItemId
					                                INNER JOIN Unit u ON d.UnitId=u.UnitId
					                                WHERE m.OrderMasterId={request.OrderMasterId}
		                                )as materialPlaning
		                                ON CONCAT(d.OrderMasterId,d.ProductId,d.ProductSizeId)=CONCAT(materialPlaning.OrderMasterId,materialPlaning.ProductId,materialPlaning.ProductSizeId)
		                                WHERE m.OrderMasterId={request.OrderMasterId} ";

                var processQuery = $@"SELECT m.OrderMasterId
		                                        , m.OrderName		
		                                        , d.ProductId
		                                        , p.ProductName
		                                        , ps.ProductSizeName
		                                        , d.Quantity as ProductQuantity
		                                        , processPlaning.ProcessTypeName
		                                        , ISNULL(processPlaning.ProcessRate,0) as ProcessRate
		                                        , CONCAT(d.ProductId,d.ProductSizeId) as ArticleId
		                                        FROM OrderMaster m
	                                        INNER JOIN OrderDetail d ON m.OrderMasterId=d.OrderMasterId
	                                        INNER JOIN ProductSize ps  ON d.ProductSizeId=ps.ProductSizeId
	                                        INNER JOIN Product p  ON d.ProductId=p.ProductId
                                           LEFT JOIN 
		                                        (
			                                        SELECT 
					                                        OrderMasterId
					                                        , ProductId
					                                        , ProductSizeId
					                                        , pt.ProcessTypeName
					                                        , p.ProcessRate
				                                        FROM Process p
					                                        INNER JOIN ProcessType pt ON p.ProcessTypeId=pt.ProcessTypeId
					                                        WHERE p.OrderMasterId={request.OrderMasterId}
		                                        )as processPlaning
		                                        ON CONCAT(d.OrderMasterId,d.ProductId,d.ProductSizeId)=CONCAT(processPlaning.OrderMasterId,processPlaning.ProductId,processPlaning.ProductSizeId)
		                                        WHERE m.OrderMasterId={request.OrderMasterId}";


                var reportData = await _unit
                                       .DapperRepository
                                       .GetListQueryAsync<OrderConsumptionRequest>(query);

                var processData = await _unit
                                      .DapperRepository
                                      .GetListQueryAsync<OrderConsumptionRequest>(processQuery);


                var stockSummary = await Function_StockSummaryWithPrice(request);

                reportData.ForEach(x =>
                {
                    //if (x.ItemPrice == 0 || x.IsManualPrice == false)
                    //{
                    //    var itemStock = stockSummary.SingleOrDefault(c => c.ItemId == x.ItemId);
                    //    if (itemStock != null)
                    //    {
                    //        x.ItemPrice = itemStock.Price;
                    //        x.StockBalanceQuantity = itemStock.Quantity;
                    //    }
                    //}
                    var itemStock = stockSummary.SingleOrDefault(c => c.ItemId == x.ItemId);
                    if (itemStock != null)
                    {
                        x.ItemPrice = x.ItemPrice == 0 ? itemStock.Price : x.ItemPrice;  // this is last purchase price
                        x.StockBalanceQuantity = itemStock.Quantity;
                    }

                });



                decimal materialCost = 0;
                decimal newPurchaseCost = 0;
                decimal wagesCost = 0;
                decimal otherCost = 0;



                if (reportData?.Count > 0)
                {
                    materialCost = reportData.Sum(x => x.ItemPrice * x.RequiredMaterialQuantity);
                    newPurchaseCost = reportData.Sum(c => c.RequiredMaterialQuantity - c.StockBalanceQuantity > 0 ? (c.RequiredMaterialQuantity - c.StockBalanceQuantity) * c.ItemPrice : 0);
                    otherCost = reportData?.FirstOrDefault()?.OtherCost ?? 0;
                }

                if (processData?.Count > 0)
                    wagesCost = processData.Sum(c => c.ProductQuantity * c.ProcessRate);



                var reportParams = new Dictionary<string, string>();

                //var date = DateHelper.GetDateFormat(DateHelper.GetDateFormat(request.ToDate), false, false, DateFormats.DefaultFormat);
                //var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;

                reportParams.Add("materialCost", materialCost.ToString());
                reportParams.Add("wagesCost", wagesCost.ToString());
                reportParams.Add("newPurchaseCost", newPurchaseCost.ToString());
                reportParams.Add("otherCost", otherCost.ToString());


                var reportConfig = _rdlcConfigs
                                    ?.Value
                                    ?.Configs
                                    ?.Find(x => x.ReportName == ReportName.Production.OrderConsumptionPrint.ToString()) ?? new RdlcReportConfiguration();

                var dataParms = new ReportDataParms<OrderConsumptionRequest>()
                {
                    Data = reportData,
                    Data2 = processData,
                    Parms = reportParams,
                    ReportConfig = reportConfig,
                    DownloadFileName = $"Customer Order Consumption Print"
                };

                return dataParms;
            }

            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Functions

        private async Task<List<InvoicePrintReportResponse>> Function_ItemsLastPrice(DateTime toDate)
        {
            try
            {
                var query = $@"SELECT itm.ItemId
		                                , ItemName
		                                , ISNULL(ISNULL(detail.Price, opening.Price), 0) as LastPrice
                                        FROM Item as itm
                                        LEFT JOIN
                                            (
                                                SELECT ItemId
                                                , ISNULL(OpeningPrice,0) as Price
                                                FROM ItemOpening
			                                )as opening
                                            ON itm.ItemId = opening.ItemId
                                            LEFT JOIN
                                            (
                                                SELECT ItemId, Price
                                                FROM V_InvoicePrint
                                                WHERE InvoiceDetailId
                                                IN(
                                                SELECT MAX(InvoiceDetailId) as InvoiceDetailId
                                                        FROM V_InvoicePrint
                                                        WHERE InvoiceDate<= '{toDate}'
                                                        AND DocumentTypeId = 1
                                                        GROUP by ItemId)
			                                )as detail
                                            ON itm.ItemId = detail.ItemId";
                return await _unit
                                       .DapperRepository
                                       .GetListQueryAsync<InvoicePrintReportResponse>(query);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<InvoicePrintReportResponse>> Function_StockSummaryWithPrice(ProductionParameterRequest request)
        {
            try
            {
                var query = $@"SELECT 
	                            itm.ItemId
	                            , ItemName
                                , UnitName
	                            , itm.PartNo
                                , ItemCategoryName
	                            , ISNULL(detail.Quantity,0) as Quantity
	                            , itm.ReorderLevel
	                            , CASE WHEN ISNULL(itm.ReorderLevel,0)<= ISNULL(detail.Quantity,0) THEN 1 ELSE 0 END as 'OrderQuantity'
                                FROM V_Item as itm
	                            LEFT JOIN
		                            (
		                                SELECT 
				                            ItemId
			                                , SUM(ISNULL(Quantity,0)) as Quantity
			                                FROM
			                                (
				                                SELECT ItemId as ItemId
					                            , ISNULL(OpeningQuantity,0) as Quantity
					                            FROM ItemOpening  
				                            UNION ALL
					                            SELECT v.ItemId
					                            , SUM(ISNULL(CASE TransactionType WHEN 1 THEN Quantity 
														                            WHEN 2 THEN -Quantity ELSE 0 END,0)) 
														                            as Quantity
					                            FROM V_InvoicePrint v      
					                            WHERE v.InvoiceDate<='{request.ToDate}'
                                                GROUP by v.ItemId
				                            ) as tbl
					                            GROUP by ItemId

		                                ) as detail
				                            ON itm.ItemId=detail.ItemId
                                            ORDER BY itm.ItemId";


                var reportData = await _unit
                                       .DapperRepository
                                       .GetListQueryAsync<InvoicePrintReportResponse>(query);

                var itemPrices = await Function_ItemsLastPrice(request.ToDate);
                if (itemPrices?.Count > 0)
                {
                    reportData.ForEach(x => { x.Price = itemPrices.Single(c => c.ItemId == x.ItemId).Price; });
                }


                if (reportData?.Count > 0)
                {

                    if (request.IsIncludeZeroValue == false)
                    {
                        reportData.RemoveAll(c => c.Quantity == 0);
                    }



                    return reportData;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        private async Task<List<TrialBalancePrintReportResponse>> Function_TrialBalance(ReportPrintTrialBalanceRequest request)
        {
            try
            {
                var postingAccountsIds = "";
                postingAccountsIds = request.PostingAccountsIds?.Count > 0 ? string.Join(",", request.PostingAccountsIds) : "";

                var query = $@"SELECT 
	                          coa.PostingAccountsId 
	                            , PostingAccountsName          
	                            , coa.SubCategoriesId        
	                            , coa.SubCategoriesName           
	                            , coa.HeadCategoriesId        
	                            , coa.HeadCategoriesName          
	                            , coa.MainHeadsId        
	                            , coa.MainHeadsName   
	                            , ISNULL(coa.OpeningDebit-coa.OpeningCredit,0) as OpeningBalance
                                , ISNULL(bf.BfAmount,0) as BfAmount
	                            , ISNULL(detail.DebitAmount,0) as DebitAmount
	                            , ISNULL(detail.CreditAmount,0) as CreditAmount
                               , (ISNULL(coa.OpeningDebit-coa.OpeningCredit,0)
		                            +ISNULL(bf.BfAmount,0)
		                            +ISNULL(detail.DebitAmount,0))
		                            -ISNULL(detail.CreditAmount,0) as ClosingBalance
                                FROM V_ChartOfAccounts coa
                                LEFT JOIN
			                            (
			                                SELECT PostingAccountsId
			                               , SUM(DebitAmount-CreditAmount) as BfAmount
			                                FROM V_Vouchers 
			                               WHERE VoucherDate <'{request.FromDate}'
                                                 {(postingAccountsIds?.Length > 0 ? $"AND PostingAccountsId IN({postingAccountsIds})" : "")}
                                            GROUP by PostingAccountsId
			                            ) as bf
			                            ON coa.PostingAccountsId=bf.PostingAccountsId
	                            LEFT JOIN
			                            (
			                                SELECT PostingAccountsId
			                                , SUM(DebitAmount) as DebitAmount
			                                , SUM(CreditAmount) as CreditAmount
			                                FROM V_Vouchers 
			                                 WHERE VoucherDate BETWEEN '{request.FromDate}' AND '{request.ToDate}'
                                                              {(postingAccountsIds?.Length > 0 ? $"AND PostingAccountsId IN({postingAccountsIds})" : "")}
                                            GROUP by PostingAccountsId
			                            ) as detail
			                            ON coa.PostingAccountsId=detail.PostingAccountsId
                                     WHERE ISNULL(coa.IsActive,0)= CASE WHEN coa.IsActive = {(request.IsActive == true ? 1 : 0)} THEN  ISNULL(coa.IsActive,0) ELSE 1  END 
			                            {(postingAccountsIds?.Length > 0 ? $"AND coa.PostingAccountsId IN({postingAccountsIds})" : "")}";

                var reportData = await _unit.DapperRepository.GetListQueryAsync<TrialBalancePrintReportResponse>(query);

                if (reportData?.Count > 0)
                {
                    if (!request.IncludeZeroValue)
                    {
                        reportData.RemoveAll(c => c.DebitAmount == 0 && c.CreditAmount == 0 && c.ClosingBalance == 0);
                    }
                }

                return reportData.ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        #endregion

    }
}


