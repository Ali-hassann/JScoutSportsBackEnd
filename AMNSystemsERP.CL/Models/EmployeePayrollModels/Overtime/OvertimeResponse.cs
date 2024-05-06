namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Overtime
{
    public class OvertimeResponse
    {
        public long OvertimeId { get; set; }
        public DateTime OvertimeDate { get; set; }
        public string Remarks { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public int OvertimeMinutes { get; set; }
        public int WorkingHours { get; set; }
        public string MarkType { get; set; }
        public string EmployeeName { get; set; }
        public long EmployeeId { get; set; }
        public long DepartmentsId { get; set; }
        public string DepartmentsName { get; set; }
    }
}