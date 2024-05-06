using AMNSystemsERP.CL.Enums;

namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee
{
    public class EmployeeDetailRequest
    {
        public long EmployeeDetailId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string CasteName { get; set; }
        public string ReligionName { get; set; }
        public string Nationality { get; set; }
        public string ContactNumber1 { get; set; }
        public string ContactNumber2 { get; set; }
        public string ContactNumber3 { get; set; }
        public EntityState RecordState { get; set; }
    }
}
