namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Attendance
{
    public class AttendanceDetailResponse : AttendanceSummaryResponse
    {
        public long AttendanceDetailId { get; set; }
        public string DetailMarkType { get; set; }
    }
}