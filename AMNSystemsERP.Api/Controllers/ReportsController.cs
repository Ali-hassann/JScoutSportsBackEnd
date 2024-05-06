using AMNSystemsERP.BL.Repositories.Reports;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.AccountModels.Reports;
using AMNSystemsERP.CL.Models.ProductionModels;
using AMNSystemsERP.CL.Models.RDLCModels;
using AMNSystemsERP.CL.Models.StockManagementModels;
using Microsoft.AspNetCore.Mvc;
using SMSRdlcReports.BL.Repositories.Reports;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/Reports")]

    public class ReportsController : ApiController
    {
        private readonly IReportsService _reportsService;
        private readonly ICommonRDLCReportsService _commonRDLCReportsService;

        public ReportsController(IReportsService reportsService, ICommonRDLCReportsService commonRDLCReportsService)
        {
            _reportsService = reportsService;
            _commonRDLCReportsService = commonRDLCReportsService;
        }

        #region Account Reports

        [HttpGet]
        [Route("PrintVoucher")]
        public async Task<ActionResult> PrintVoucher(long vouchersMasterId ,bool isAdvanceVoucher = false)
        {
            try
            {
                if (vouchersMasterId > 0)
                {
                    var reprotResponse = await _reportsService.PrintVoucher(vouchersMasterId, isAdvanceVoucher);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("PrintAccountsLedger")]
        public async Task<ActionResult> PrintAccountsLedger([FromBody] ReportPrintLedgerRequest request)
        {
            try
            {
                if (request != null)
                {
                    if (request.PostingAccountsId > 0
                        && !string.IsNullOrEmpty(request.FromDate)
                        && !string.IsNullOrEmpty(request.ToDate))
                    {
                        var reportResponse = await _reportsService.PrintAccountsLedger(request);

                        return await GetReportData(reportResponse);
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
        [Route("PrintSubCategoryLedger")]
        public async Task<ActionResult> PrintSubCategoryLedger([FromBody] ReportPrintTrialBalanceRequest request)
        {
            try
            {
                if (request != null)
                {
                    if (request.SubCategoriesId > 0)
                    {
                        var reportResponse = await _reportsService.PrintSubCategoryLedger(request);

                        return await GetReportData(reportResponse);
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
        [Route("PrintHeadCategoryLedger")]
        public async Task<ActionResult> PrintHeadCategoryLedger([FromBody] ReportPrintHeadCategoryRequest request)
        {
            try
            {
                if (request != null)
                {
                    if (request.HeadCategoriesId > 0)
                    {
                        if (request.IncludeDetail)
                        {
                            var reportResponse = await _reportsService.PrintHeadCategoryDetailLedger(request);
                            return await GetReportData(reportResponse);
                        }
                        else
                        {
                            var reportResponse = await _reportsService.PrintHeadCategoryLedger(request);
                            return await GetReportData(reportResponse);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Ok("not found");
        }

        [HttpPost]
        [Route("PrintMainHeadLedger")]
        public async Task<ActionResult> PrintMainHeadLedger([FromBody] ReportPrintMainHeadRequest request)
        {
            try
            {
                if (request?.MainHeadId > 0)
                {
                    if (request.IncludeDetail)
                    {
                        var reportResponse = await _reportsService.PrintMainHeadDetailLedger(request);
                        return await GetReportData(reportResponse);
                    }
                    else
                    {
                        var reportResponse = await _reportsService.PrintMainHeadLedger(request);
                        return await GetReportData(reportResponse);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("PrintChartOfAccoutns")]
        public async Task<ActionResult> PrintChartOfAccoutns(long organizationId, long outletId, string outletName, string address)
        {
            try
            {
                if (organizationId > 0 && outletId > 0)
                {
                    var dataResponse = await _reportsService.PrintChartOfAccoutns(organizationId, outletId, outletName, address);

                    return await GetReportData(dataResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("PrintDailyVouchers")]
        public async Task<ActionResult> PrintDailyVouchers([FromBody] ReportPrintDailyVouchersRequest request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request?.FromDate)
                    && !string.IsNullOrEmpty(request.ToDate)
                    )
                {
                    var reportResponse = await _reportsService.PrintDailyVouchers(request);

                    return await GetReportData(reportResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("PrintPayableReceiveable")]
        public async Task<ActionResult> PrintPayableReceiveable(long organizationId, long outletId, string outletName, string address)
        {
            try
            {
                if (organizationId > 0 && outletId > 0)
                {
                    var reportResponse = await _reportsService.PrintPayableReceiveable(organizationId, outletId, outletName, address);

                    return await GetReportData(reportResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("PrintTrialBalance")]
        public async Task<ActionResult> PrintTrialBalance([FromBody] ReportPrintTrialBalanceRequest request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request?.FromDate)
                    && !string.IsNullOrEmpty(request.ToDate)
                    )
                {
                    var reportResponse = await _reportsService.PrintTrialBalance(request);

                    return await GetReportData(reportResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("PrintBalanceSheet")]
        public async Task<ActionResult> PrintBalanceSheet([FromBody] ReportPrintTrialBalanceRequest request)
        {
            try
            {
                if (request != null)
                {
                    if (!string.IsNullOrEmpty(request.FromDate)
                    && !string.IsNullOrEmpty(request.ToDate))
                    {
                        var reportResponse = await _reportsService.PrintBalanceSheet(request);

                        return await GetReportData(reportResponse);
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
        [Route("ProfitAndLoss")]
        public async Task<ActionResult> ProfitAndLoss([FromBody] ReportPrintTrialBalanceRequest request)
        {
            try
            {
                if (request != null)
                {
                    if (!string.IsNullOrEmpty(request.FromDate)
                    && !string.IsNullOrEmpty(request.ToDate))
                    {
                        var reportResponse = await _reportsService.PrintProfitAndLoss(request);

                        return await GetReportData(reportResponse);
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
        [Route("GeneralExpensePrint")]
        public async Task<ActionResult> GeneralExpensePrint([FromBody] ReportPrintLedgerRequest request)
        {
            try
            {
                if (request != null)
                {
                    if ( !string.IsNullOrEmpty(request.FromDate)
                    && !string.IsNullOrEmpty(request.ToDate))
                    {
                        var reportResponse = await _reportsService.GeneralExpensePrint(request);

                        return await GetReportData(reportResponse);
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
        [Route("PrintDailyPaymentReceipt")]
        public async Task<ActionResult> PrintDailyPaymentReceipt([FromBody] ReportPrintDailyVouchersRequest request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.FromDate)
                    && !string.IsNullOrEmpty(request.ToDate)
                    )
                {
                    var reportResponse = await _reportsService.PrintDailyPaymentReceipt(request);

                    return await GetReportData(reportResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("PrintVoucherList")]
        public async Task<ActionResult> PrintVoucherList([FromBody] ReportPrintDailyVouchersRequest request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.FromDate)
                    && !string.IsNullOrEmpty(request.ToDate)
                    )
                {
                    var reportResponse = await _reportsService.PrintVoucherList(request);

                    return await GetReportData(reportResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region Stock management

        [HttpGet]
        [Route("PrintItems")]
        public async Task<ActionResult> PrintItems()
        {
            try
            {
                var reprotResponse = await _reportsService.PrintItems();
                return await GetReportData(reprotResponse);
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("PrintInvoice")]
        public async Task<ActionResult> PrintInvoice(long invoiceMasterId)
        {
            try
            {
                if (invoiceMasterId > 0)
                {
                    var reprotResponse = await _reportsService.PrintInvoice(invoiceMasterId);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("PrintPurchaseOrder")]
        public async Task<ActionResult> PrintPurchaseOrder(long purchaseOrderMasterId)
        {
            try
            {
                if (purchaseOrderMasterId > 0)
                {
                    var reprotResponse = await _reportsService.PrintPurchaseOrder(purchaseOrderMasterId);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("PrintDailyTransactions")]
        public async Task<ActionResult> PrintDailyTransactions([FromBody] InvoiceParameterRequest request)
        {
            try
            {
                if (request.DocumentTypeId > 0
                    && !string.IsNullOrEmpty(request.FromDate)
                         && !string.IsNullOrEmpty(request.ToDate))
                {
                    var reprotResponse = await _reportsService.PrintDailyTransactions(request);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("PrintItemsLedger")]
        public async Task<ActionResult> PrintItemsLedger([FromBody] InvoiceParameterRequest request)
        {
            try
            {
                if (
                    !string.IsNullOrEmpty(request.FromDate)
                         && !string.IsNullOrEmpty(request.ToDate))
                {
                    var reprotResponse = await _reportsService.PrintItemsLedger(request);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("PrintStockSummary")]
        public async Task<ActionResult> PrintStockSummary([FromBody] InvoiceParameterRequest request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.ToDate))
                {
                    var reprotResponse = await _reportsService.PrintStockSummary(request);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("PrintPartyLedger")]
        public async Task<ActionResult> PrintPartyLedger([FromBody] InvoiceParameterRequest request)
        {
            try
            {
                if (
                    !string.IsNullOrEmpty(request.FromDate)
                         && !string.IsNullOrEmpty(request.ToDate)
                         && request.ParticularId > 0)
                {
                    var reprotResponse = await _reportsService.PrintPartyLedger(request);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }


        #endregion

        #region Production

        [HttpPost]
        [Route("PrintOrderStatus")]
        public async Task<ActionResult> PrintOrderStatus([FromBody] ProductionParameterRequest request)
        {
            try
            {
                if (
                     request?.OrderMasterId > 0
                    )
                {
                    var reprotResponse = await _reportsService.PrintOrderStatus(request);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("PrintArticleBalanceEmployeeWise")]
        public async Task<ActionResult> PrintArticleBalanceEmployeeWise([FromBody] ProductionParameterRequest request)
        {
            try
            {
                if (
                     request?.OrderMasterId > 0
                    )
                {
                    var reprotResponse = await _reportsService.PrintArticleBalanceEmployeeWise(request);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("PrintIssueSlip")]
        public async Task<ActionResult> PrintIssueSlip([FromBody] ProductionParameterRequest request)
        {
            try
            {
                if (request?.OrderMasterId > 0)
                {
                    var reprotResponse = await _reportsService.PrintIssueSlip(request);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("PrintOrderConsumption")]
        public async Task<ActionResult> PrintOrderConsumption([FromBody] ProductionParameterRequest request)
        {
            try
            {
                if (request?.OrderMasterId > 0)
                {
                    var reportResponse = await _reportsService.PrintOrderConsumption(request);
                    return await GetReportData(reportResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        public async Task<ActionResult> GetReportData<T>(ReportDataParms<T> reportData)
        {
            try
            {
                if (!string.IsNullOrEmpty(reportData?.ReportConfig?.ReportName) && reportData?.Data?.Count > 0)
                {
                    // Getting TempFolder Path
                    var tempFolderPath = @$"{DocumentHelper.GetDocumentDirectoryRootPath()}\TempDocuments";
                    DocumentHelper.CreateFolder(tempFolderPath);

                    var fileName = reportData.DownloadFileName != "" ? reportData.DownloadFileName : Guid.NewGuid().ToString();
                    var reportConfig = reportData?.ReportConfig;
                    // Creating RdlcReportRequest Model
                    var reportRequest = new RdlcReportRequest<T>()
                    {
                        ReportData = reportData.Data,
                        ReportDataSourceName = reportConfig.DataSource,
                        ReportData2 = reportData.Data2,
                        ReportDataSourceName2 = reportConfig.DataSource2,
                        RdlcReportType = reportConfig.RDLCReportType,
                        RdlcReportName = $"{reportConfig.ReportName}.rdlc",
                        ReportFileName = $"{fileName}",
                        RenderType = reportConfig.RenderType,
                        TempFolderPath = tempFolderPath,
                        ReportParams = reportData.Parms,
                    };

                    // Generating Rdlc Report Link here
                    var bytesData = await _commonRDLCReportsService.GenerateRdlcReport(reportRequest);

                    return File(bytesData, "application/pdf", fileName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Ok("");
        }
    }

}
