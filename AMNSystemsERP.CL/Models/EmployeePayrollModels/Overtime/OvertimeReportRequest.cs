namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Overtime
{
    public class OvertimeReportRequest
    {
        public long OvertimeId { get; set; }
        public DateTime OvertimeDate { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public decimal TimeDuration { get; set; }
        public string DetailMarkType { get; set; }
        public string EmployeeName { get; set; }
        public string FatherName { get; set; }
        public long EmployeeId { get; set; }
        public long DepartmentsId { get; set; }
        public string DepartmentsName { get; set; }
        public string DesignationName { get; set; }
        public string DayName { get; set; }
    }
}