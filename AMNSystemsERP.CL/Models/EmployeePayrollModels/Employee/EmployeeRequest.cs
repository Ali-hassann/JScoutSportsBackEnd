namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee
{
    public class EmployeeRequest
    {
        public EmployeeBasicRequest Employee { get; set; }
        public EmployeeDetailRequest? EmployeeDetail { get; set; }
    }
}
