using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Attendance
{
    public class AttendanceDashboardSummaryRequest : CommonBaseModel
    {
        public string OutletIds { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string StatusIds { get; set; }
        public string TypeIds { get; set; }
    }

    public class AttendanceDashboardSummaryResponse 
    {
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int LeavePaidCount { get; set; }
        public int LeaveUnPaidCount { get; set; }
        public int HalfLeavePaidCount { get; set; }
        public int HalfLeaveUnPaidCount { get; set; }
        public int OvertimeCount { get; set; }
        public int NotApplicableCount { get; set; }
        public int OfficialLeaveCount { get; set; }
    }
}