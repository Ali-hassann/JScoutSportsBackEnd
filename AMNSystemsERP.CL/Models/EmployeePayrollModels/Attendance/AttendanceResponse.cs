using AMNSystemsERP.CL.Models.EmployeePayrollModels.Base;

namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Attendance
{
    public class AttendanceResponse : EmployeeBaseResponse, IAttendanceSummary
    {
        public long AttendanceId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string StatusName { get; set; }
        public string ShortDescription { get; set; }
        public string Remarks { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string PersonType { get; set; }
        public string Overtime { get; set; } // Formatted Time => {02:00}
        public int OvertimeMinutes { get; set; }
        public int WorkingHours { get; set; }
        public string MarkType { get; set; }
    }
}
