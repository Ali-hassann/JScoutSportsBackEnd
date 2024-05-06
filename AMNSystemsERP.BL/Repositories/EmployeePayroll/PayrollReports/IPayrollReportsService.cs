using AMNSystemsERP.CL.Models.EmployeePayrollModels;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Attendance;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Overtime;
using AMNSystemsERP.CL.Models.RDLCModels;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;

namespace AMNSystemsERP.BL.Repositories.EmployeePayroll.PayrollReports
{
    public interface IPayrollReportsService
    {
        Task<ReportDataParms<EmployeeBasicRequest>> GetEmployeesList(PayrollParameterRequest request);
        Task<ReportDataParms<AttendanceSummaryResponse>> GetAttendanceList(PayrollParameterRequest request);
        Task<ReportDataParms<AttendanceSummaryResponse>> GetLateHoursReport(PayrollParameterRequest request);
        Task<ReportDataParms<SalarySheet>> GetSalarySheet(PayrollParameterRequest request);
        Task<ReportDataParms<OvertimeReportRequest>> GetOvertimeList(PayrollParameterRequest request);
        Task<ReportDataParms<AttendanceRegister>> GetMonthlyAttendanceRegister(PayrollParameterRequest request);
        Task<ReportDataParms<SalarySheet>> GetPaySlip(SalarySheet request);
    }
}