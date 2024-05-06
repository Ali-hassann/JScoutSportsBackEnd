namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Allowances
{
    public class EmployeeAllowancesRequest
    {
        public long EmployeeAllowancesId { get; set; }
        public decimal Amount { get; set; }
        public long AllowanceTypeId { get; set; }
        public string Name { get; set; }
        public long EmployeeId { get; set; }
    }
}