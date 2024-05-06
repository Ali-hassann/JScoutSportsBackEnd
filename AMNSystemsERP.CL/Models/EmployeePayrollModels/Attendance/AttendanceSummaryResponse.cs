namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Attendance
{
    public class AttendanceSummaryResponse : IAttendanceSummary
    {
        public long AttendanceId { get; set; }        
        public DateTime AttendanceDate { get; set; }
        public string CheckTime { get; set; }
        public string CheckType { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public decimal TimeDuration { get; set; }
        public string StatusName { get; set; }
        public string ShortDescription { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FatherName { get; set; }
        public string DepartmentsId { get; set; }
        public string DepartmentsName { get; set; }
        public string DesignationName { get; set; }
        public string MarkType { get; set; }
        public string DetailMarkType { get; set; }
        public string DayName { get; set; }
    }
}
