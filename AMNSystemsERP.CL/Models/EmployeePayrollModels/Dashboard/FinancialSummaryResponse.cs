namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Dashboard
{
    public class FinancialSummaryResponse
    {
        public long DepartmentsId { get; set; }
        public string DepartmentsName { get; set; }
        public decimal Amount { get; set; }
        public string TypeName { get; set; }
    }
}