namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class AttendanceRegister
    {        
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FatherName { get; set; }
        public string DesignationName { get; set; }
        public DateTime DateOfMonth { get; set; }
        public int DaysOfMonth { get; set; }
        public string DaysName { get; set; }
        public string StatusName { get; set; }
        public long DepartmentsId { get; set; }
        public string DepartmentsName { get; set; }
    }
}
