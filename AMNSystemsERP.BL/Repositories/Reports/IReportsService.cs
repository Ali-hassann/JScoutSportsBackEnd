using AMNSystemsERP.CL.Models.AccountModels.Reports;
using AMNSystemsERP.CL.Models.InventoryModels;
using AMNSystemsERP.CL.Models.ProductionModels;
using AMNSystemsERP.CL.Models.RDLCModels;
using AMNSystemsERP.CL.Models.StockManagementModels;
using AMNSystemsERP.CL.Models.StockManagementModels.Reports;

namespace AMNSystemsERP.BL.Repositories.Reports
{
    public interface IReportsService
    {
        // Print RDLC Accounts Reports

        // ----------------------------------------------------------------------------
        // ------------------ Rdlc Reports Section Start ------------------------------
        // ----------------------------------------------------------------------------
        #region Accounts
        Task<ReportDataParms<VoucherPrintReportResponse>> PrintVoucher(long vouchersMasterId, bool isAdvanceVoucher);
        Task<ReportDataParms<LedgerPrintReportResponse>> PrintAccountsLedger(ReportPrintLedgerRequest request);
        Task<ReportDataParms<LedgerPrintReportResponse>> GeneralExpensePrint(ReportPrintLedgerRequest request);
        Task<List<LedgerPrintReportResponse>> GetAccountsLedger(ReportPrintLedgerRequest request);
        Task<ReportDataParms<TrialBalancePrintReportResponse>> PrintSubCategoryLedger(ReportPrintTrialBalanceRequest request);
        Task<ReportDataParms<HeadCategoryPrintReportResponse>> PrintHeadCategoryLedger(ReportPrintHeadCategoryRequest request);
        Task<ReportDataParms<DetailPrintReportResponse>> PrintHeadCategoryDetailLedger(ReportPrintHeadCategoryRequest request);
        Task<ReportDataParms<DetailPrintReportResponse>> PrintMainHeadDetailLedger(ReportPrintMainHeadRequest request);
        Task<ReportDataParms<MainHeadPrintReportResponse>> PrintMainHeadLedger(ReportPrintMainHeadRequest request);
        Task<ReportDataParms<ChartOfAccountsPrintReportResponse>> PrintChartOfAccoutns(long organizationId, long outletId, string outletName, string address);
        Task<ReportDataParms<PayableReceiveablePrintReportResponse>> PrintPayableReceiveable(long organizationId, long outletId, string outletName, string address);
        Task<ReportDataParms<DailyVouchersPrintReportResponse>> PrintDailyVouchers(ReportPrintDailyVouchersRequest request);
        Task<ReportDataParms<DailyVouchersPrintReportResponse>> PrintDailyPaymentReceipt(ReportPrintDailyVouchersRequest request);
        Task<ReportDataParms<DailyVouchersPrintReportResponse>> PrintVoucherList(ReportPrintDailyVouchersRequest request);
        Task<ReportDataParms<TrialBalancePrintReportResponse>> PrintTrialBalance(ReportPrintTrialBalanceRequest request);
        Task<ReportDataParms<TrialBalancePrintReportResponse>> PrintBalanceSheet(ReportPrintTrialBalanceRequest request);
        Task<ReportDataParms<TrialBalancePrintReportResponse>> PrintProfitAndLoss(ReportPrintTrialBalanceRequest request);

        #endregion


        Task<ReportDataParms<InvoicePrintReportResponse>> PrintInvoice(long invoiceMasterId);
        Task<ReportDataParms<InvoicePrintReportResponse>> PrintPurchaseOrder(long invoiceMasterId);
        Task<ReportDataParms<ItemRequest>> PrintItems();



        #region Stock Reports
        Task<ReportDataParms<InvoicePrintReportResponse>> PrintDailyTransactions(InvoiceParameterRequest request);
        Task<ReportDataParms<InvoicePrintReportResponse>> PrintItemsLedger(InvoiceParameterRequest request);
        Task<ReportDataParms<InvoicePrintReportResponse>> PrintPartyLedger(InvoiceParameterRequest request);
        Task<ReportDataParms<InvoicePrintReportResponse>> PrintStockSummary(InvoiceParameterRequest request);
        #endregion 


        #region Production Reports
        Task<ReportDataParms<ProductionProcessRequest>> PrintOrderStatus(ProductionParameterRequest request);
        Task<ReportDataParms<ProductionProcessRequest>> PrintArticleBalanceEmployeeWise(ProductionParameterRequest request);
        Task<ReportDataParms<ProductionProcessRequest>> PrintIssueSlip(ProductionParameterRequest request);
        Task<ReportDataParms<OrderConsumptionRequest>> PrintOrderConsumption(ProductionParameterRequest request);
        #endregion


    }
}