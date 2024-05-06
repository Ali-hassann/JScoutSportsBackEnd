namespace AMNSystemsERP.CL.Enums.Reports
{
    public static class ReportName
    {
        // Configuration
        // RDLC Reports Name        

        public static class Accounts
        {
            public static readonly string VoucherPrint = "VoucherPrint";
            public static readonly string VoucherAdvancePrint = "VoucherAdvancePrint";
            public static readonly string LedgerPrint = "LedgerPrint";
            public static readonly string SubCategoryPrint = "SubCategoryPrint";
            public static readonly string SumSubCategoriesByHeadCategoryIdPrint = "SumSubCategoriesByHeadCategoryIdPrint";
            public static readonly string SumHeadCategoriesByMainHeadIdPrint = "SumHeadCategoriesByMainHeadIdPrint";
            public static readonly string ChartOfAccountsPrint = "ChartOfAccountsPrint";
            public static readonly string TrialBalancePrint = "TrialBalancePrint";
            public static readonly string MainHeadDetailPrint = "MainHeadDetailPrint";
            public static readonly string BalanceSheetPrint = "BalanceSheetPrint";
            public static readonly string HeadCategoryDetailPrint = "HeadCategoryDetailPrint";
            public static readonly string DailyVouchersPrint = "DailyVouchersPrint";
            public static readonly string PayableReceiveablePrint = "PayableReceiveablePrint";
            public static readonly string ProfitAndLossPrint = "ProfitAndLossPrint";
            public static readonly string LedgerExpensePrint = "LedgerExpensePrint";
            public static readonly string DailyVouchersPaymentReceiptPrint = "DailyVouchersPaymentReceiptPrint";
            public static readonly string VoucherListPrint = "VoucherListPrint";
        }

        public static class StockManagement
        {
            //public static readonly string PurchasePrint = "PurchasePrint";
            //public static readonly string PurchaseReturnPrint = "PurchaseReturnPrint";
            //public static readonly string SalePrint = "SalePrint";
            //public static readonly string SaleReturnPrint = "SaleReturnPrint";
            //public static readonly string IssuancePrint = "IssuancePrint";
            //public static readonly string IssuanceReturnPrint = "IssuanceReturnPrint";

            public static readonly string ItemsPrint = "ItemsPrint";
            public static readonly string InvoicePrint = "InvoicePrint";
            public static readonly string PurchaseOrderPrint = "PurchaseOrderPrint";
            public static readonly string DailyTransactionsPrint = "DailyTransactionsPrint";

            public static readonly string ItemsLedgerPrint = "ItemsLedgerPrint";
            public static readonly string PartyLedgerPrint = "PartyLedgerPrint";
            public static readonly string StockSummaryPrint = "StockSummaryPrint";
            public static readonly string ReorderLevelPrint = "ReorderLevelPrint";

        }

        public static class Payroll
        {
            public static readonly string EmployeeListPrint = "EmployeeListPrint";
            public static readonly string AttendanceListPrint = "AttendanceListPrint";
            public static readonly string SalarySheetPrint = "SalarySheetPrint";
            public static readonly string DirectorSalarySheetPrint = "DirectorSalarySheetPrint";
            public static readonly string PaySlipPrint = "PaySlipPrint";
            public static readonly string OvertimeListPrint = "OvertimeListPrint";
            public static readonly string AttendanceRegisterPrint = "AttendanceRegisterPrint";
        }
        public static class Production
        {
            public static readonly string OrderStatusMatrixPrint = "OrderStatusMatrixPrint";
            public static readonly string ArticleBalanceEmployeeWisePrint = "ArticleBalanceEmployeeWisePrint";
            public static readonly string IssueSlipPrint = "IssueSlipPrint";
            public static readonly string OrderConsumptionPrint = "OrderConsumptionPrint";
        }
    }
}