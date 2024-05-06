using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee
{
    public class EmployeeDocumentsRequest: CommonBaseModel
    {
        public long EmployeeDocumentsId { get; set; }
        public long EmployeeId { get; set; }
        public string ImagePath { get; set; }
        public bool IsDeleted { get; set; }
    }
}