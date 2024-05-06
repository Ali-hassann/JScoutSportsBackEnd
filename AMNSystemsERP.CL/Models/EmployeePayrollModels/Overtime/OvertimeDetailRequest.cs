namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Overtime
{
    public class OvertimeDetailRequest
    {
        public long OvertimeDetailId { get; set; }
        public long OvertimeId { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public long EmployeeId { get; set; }
        public string DetailMarkType { get; set; }
        public string DeviceIP { get; set; } = "";
    }
}