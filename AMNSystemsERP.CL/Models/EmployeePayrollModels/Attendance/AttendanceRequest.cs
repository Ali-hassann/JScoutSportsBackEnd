using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Attendance
{
    public class AttendanceRequest : CommonBaseModel
    {
        public long EmployeeId { get; set; }           
        public long AttendanceId { get; set; }           
        public int AttendanceStatusId { get; set; }
        public string StatusName{ get; set; }
        public string Remarks { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string MarkType { get; set; }
    }
}
