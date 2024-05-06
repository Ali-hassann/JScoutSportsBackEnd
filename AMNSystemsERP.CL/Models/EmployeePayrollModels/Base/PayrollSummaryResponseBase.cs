namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Base
{
    public class PayrollSummaryResponseBase
    {
        public string MonthFullName { get; set; }
        public int Year { get; set; }
        public decimal TotalAllowances { get; set; }
        public decimal TotalOvertimeAmount { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal TotalRoundedSalary { get; set; }
    }
}