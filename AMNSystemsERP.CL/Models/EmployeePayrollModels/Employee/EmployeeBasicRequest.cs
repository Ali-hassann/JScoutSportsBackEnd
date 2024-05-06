using AMNSystemsERP.CL.Models.EmployeePayrollModels.Base;

namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee
{
    public class EmployeeBasicRequest : EmployeeBaseRequest
    {      
        public string DepartmentsName { get; set; }        
        public string DesignationName { get; set; }
        public string OutletName { get; set; }
        public bool IsToCreateAccount { get; set; }
    }
}
