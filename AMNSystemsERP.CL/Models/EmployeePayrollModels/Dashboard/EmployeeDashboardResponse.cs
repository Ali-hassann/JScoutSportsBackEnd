namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Dashboard
{
    public class EmployeeDashboardResponse
    {
        public long DepartmentsId { get; set; }
        public string DepartmentsName { get; set; }
        public int EmployeeCount { get; set; }
        public decimal SalarySum { get; set; }
    }
}