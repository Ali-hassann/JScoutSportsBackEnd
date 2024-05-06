using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Overtime
{
    public class OvertimeRequest : CommonBaseModel
    {
        public long EmployeeId { get; set; }
        public long OvertimeId { get; set; }
        public string Remarks { get; set; }
        public DateTime OvertimeDate { get; set; }
        public string MarkType { get; set; }
    }
}
