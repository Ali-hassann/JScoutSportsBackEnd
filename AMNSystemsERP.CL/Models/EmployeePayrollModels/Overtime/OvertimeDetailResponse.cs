namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Overtime
{
    public class OvertimeDetailResponse 
    {
        public long OvertimeDetailId { get; set; }
        public long OvertimeId { get; set; }
        public string? CheckIn { get; set; }
        public string? CheckOut { get; set; }
        public DateTime? OvertimeDate { get; set; }
        public long EmployeeId { get; set; }
        public string DetailMarkType { get; set; }
        public string DeviceIP { get; set; } = "";
    }
}