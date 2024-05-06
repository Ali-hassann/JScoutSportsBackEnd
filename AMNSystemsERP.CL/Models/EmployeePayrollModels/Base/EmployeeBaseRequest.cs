using AMNSystemsERP.CL.Enums;

namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Base
{
    public class EmployeeBaseRequest
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FatherName { get; set; }
        public string HusbandName { get; set; }
        public string ContactNumber { get; set; }
        public string Gender { get; set; }
        public string CNIC { get; set; }
        public bool IsActive { get; set; }
        public string ImagePath { get; set; }
        public long DepartmentsId { get; set; }
        public long OutletId { get; set; }
        public long OrganizationId { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? LeftDate { get; set; }
        public decimal SalaryAmount { get; set; }
        public decimal OvertimeHourlyWageAmount { get; set; }
        public int WorkingHours { get; set; }
        public int SalaryType { get; set; }
        public long DesignationId { get; set; }
        public bool EmployeeStatus { get; set; }
        public bool IsToDeleteImage { get; set; }
        public string MaritalStatus { get; set; }
        public long EmployeeSerialNo { get; set; }
        public string EmployeeCode { get; set; }
        public EntityState RecordState { get; set; }
    }
}
