using AMNSystemsERP.CL.Models.EmployeePayrollModels.Attendance;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Overtime;

namespace AMNSystemsERP.BL.Repositories.EmployeePayroll.AttendanceRepo
{
    public interface IAttendanceService
    {
        Task<List<AttendanceSummaryResponse>> GetAttendanceSummaryAndHistory(EmployeeFilterRequest request);
        Task<List<AttendanceResponse>> GetAttendanceList(EmployeeFilterRequest request);
        Task<List<AttendanceRequest>> SaveAttendance(List<AttendanceRequest> request);
        Task<List<AttendanceDetailResponse>> GetAttendanceDetailList(long attendanceId);
        Task<bool> SaveAttendanceDetailList(List<AttendanceDetailRequest> request);
        Task<bool> RemoveAttendanceDetail(long attendanceDetailId);
        Task<AttendanceDashboardSummaryResponse> GetAttendanceDashboardSummary(AttendanceDashboardSummaryRequest attendanceSummaryRequest);

        Task<List<OvertimeResponse>> GetOvertimeList(EmployeeFilterRequest request);
        Task<bool> SaveOvertime(List<OvertimeRequest> request);
        Task<List<OvertimeDetailResponse>> GetOvertimeDetailList(long OvertimeId);
        Task<bool> SaveOvertimeDetailList(List<OvertimeDetailRequest> request);
        Task<bool> RemoveOvertimeDetail(long overtimeDetailId);
    }
}
