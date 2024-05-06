namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Attendance
{
    public class AttendanceDetailRequest
    {
        public long AttendanceDetailId { get; set; }
        public long AttendanceId { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public long EmployeeId { get; set; }
        public string DetailMarkType { get; set; }
        public string DeviceIP { get; set; } = "";
    }
}