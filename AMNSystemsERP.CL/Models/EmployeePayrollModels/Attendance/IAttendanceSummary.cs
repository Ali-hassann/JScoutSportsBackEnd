namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Attendance
{
    public interface IAttendanceSummary
    {       
        public string StatusName { get; set; }
        public string ShortDescription { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
    }
}
