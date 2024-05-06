namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Base
{
    public class EmployeeBaseResponse
    {
        public long EmployeeId { get; set; }
        public long RegistrationNo { get; set; }
        public string EmployeeName { get; set; }
        public long OutletId { get; set; }
        public string OutletName { get; set; }
        public string OutletImagePath { get; set; }
        public long DepartmentsId { get; set; }
        public string DepartmentsName { get; set; }
        public string DesignationName { get; set; }
        public string ImagePath { get; set; }
        public string JoiningDate { get; set; }
    }
}
