using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee
{
    public class EmployeeFilterRequest : CommonBaseModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string StatusIds { get; set; }
        public string TypeIds { get; set; }
        public string EmployeeIds { get; set; }
        public string DepartmentIds { get; set; }
    }
}
