namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Base
{
    public class DepartmentsRequest
    {
        public long DepartmentsId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DepartmentTypeId { get; set; }
        public long? OrganizationId { get; set; }
    }
}
