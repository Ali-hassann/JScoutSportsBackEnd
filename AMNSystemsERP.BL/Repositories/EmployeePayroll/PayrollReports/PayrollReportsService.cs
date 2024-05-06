using AutoMapper;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee;
using AMNSystemsERP.CL.Models.EmployeePayrollModels;
using Microsoft.Extensions.Options;
using AMNSystemsERP.CL.Models.RDLCModels;
using AMNSystemsERP.CL.Enums.Reports;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Attendance;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Overtime;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;
using AMNSystemsERP.CL.Services.CurrentLogin;
using Microsoft.Extensions.Configuration;
using AMNSystemsERP.CL.Enums.PayrollEnums;
using Core.CL.Enums;

namespace AMNSystemsERP.BL.Repositories.EmployeePayroll.PayrollReports
{
    public class PayrollReportsService : IPayrollReportsService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IOptions<ReportsConfigs> _rdlcConfigs;
        private readonly IConfiguration _config;
        private readonly ICurrentLoginService _currentLogin;


        public PayrollReportsService(IUnitOfWork unit
        , IMapper mapper
        , IOptions<ReportsConfigs> rdlcConfigs
        , IConfiguration config
        , ICurrentLoginService currentLogin)
        {
            _unit = unit;
            _rdlcConfigs = rdlcConfigs;
            _config = config;
            _mapper = mapper;
            _currentLogin = currentLogin;
        }

        public async Task<ReportDataParms<EmployeeBasicRequest>> GetEmployeesList(PayrollParameterRequest request)
        {
            try
            {
                // string.Join(",", request.DepartmentIds.Select(c => c.Id.ToString()).ToArray());

                var salaryType = request.SalaryType == (int)SalaryType.Wages ? $"{(int)SalaryType.Wages}" : $"{(int)SalaryType.SalaryPerson},{(int)SalaryType.FixSalary}";

                var query = $@"SELECT e.EmployeeId
		                            , e.EmployeeName
		                            , e.FatherName
		                            , e.Gender
		                            , e.IsActive
                                    , e.DepartmentsId
		                            , d.Name as DepartmentsName
		                            , desi.DesignationName
		                            , e.SalaryAmount
		                            , e.WorkingHours
		                            , e.EmployeeCode
                                      FROM EMPLOYEE e
		                                INNER JOIN Departments d ON e.DepartmentsId=d.DepartmentsId
		                                INNER JOIN Designation desi ON e.DesignationId=desi.DesignationId
		                                WHERE OutLetId={request.OutletId}
                                        AND e.SalaryType IN ({salaryType})
                                        {(request.EmployeeIds.Length > 0 ? $" AND EmployeeId IN ({request.EmployeeIds})" : "")} 
                                        {(request.DepartmentIds.Length > 0 ? $" AND e.DepartmentsId IN ({request.DepartmentIds})" : "")} ";

                var reportData = await _unit.DapperRepository.GetListQueryAsync<EmployeeBasicRequest>(query);

                if (reportData?.Count > 0)
                {
                    // Calling and getting path of RDLC Report
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Payroll.EmployeeListPrint.ToString()) ?? new RdlcReportConfiguration();

                    var reportParams = new Dictionary<string, string>();
                    reportParams.Add("pmrType", $"{(request.SalaryType == (int)SalaryType.Wages ? "Contractor" : "Employee") ?? ""}");

                    var dataParms = new ReportDataParms<EmployeeBasicRequest>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = "Employee List Print"
                    };
                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<AttendanceSummaryResponse>> GetAttendanceList(PayrollParameterRequest request)
        {
            try
            {
                var query = $@"  WITH cte
		                                AS
		                                (
			                                SELECT
				                                    e.EmployeeId
				                                    , e.EmployeeName
				                                    , e.FatherName
				                                    , e.DepartmentsId
				                                    , dept.Name as DepartmentsName
                                                    , desig.DesignationName
				                                    , attendance.AttendanceDate
				                                    , CheckIn
				                                    , CheckOut
                                                    , DetailMarkType
				                                    FROM Employee e
			                                    INNER JOIN 
			                                    (
				                                    SELECT  mast.EmployeeId
				                                        , AttendanceDate
				                                        , CASE WHEN CheckIn < '08:30:00.0000000' THEN '08:30:00.0000000' ELSE CheckIn END AS CheckIn
				                                        , CASE WHEN CheckOut > '17:30:00.0000000' THEN '17:30:00.0000000' ELSE CheckOut END AS CheckOut
				                                        , det.DetailMarkType
				                                        FROM Attendance mast
					                                        INNER JOIN AttendanceDetail det ON mast.AttendanceId=det.AttendanceId
					                                        AND Month(mast.AttendanceDate)={request.MonthOf}
					                                        AND YEAR(mast.AttendanceDate)={request.YearOf}
			                                    ) attendance
					                                    ON e.EmployeeId=attendance.EmployeeId
					                                    INNER JOIN Departments dept ON e.DepartmentsId=dept.DepartmentsId	
                                                        INNER JOIN Designation desig ON e.DesignationId=desig.DesignationId
                                                        WHERE e.OutletId={request.OutletId}
                                                        {(request.DepartmentIds.Length > 0 ? $" AND e.DepartmentsId IN ({request.DepartmentIds})" : "")}
                                                        {(request.EmployeeIds.Length > 0 ? $" AND e.EmployeeId IN ({request.EmployeeIds})" : "")}
                                                        AND IsActive = 1
                                                        AND dept.DepartmentTypeId = {request.DepartmentTypeId}
                                            )
			                                SELECT  EmployeeId
						                                , EmployeeName
						                                , FatherName
                                                        , DepartmentsId
						                                , DepartmentsName
                                                        , DesignationName
						                                , AttendanceDate
                                                        , DATENAME(dw, AttendanceDate) AS DayName
                                                        , DetailMarkType
						                                , FORMAT(CAST(CheckIn as datetime), 'hh:mm tt') AS CheckIn
						                                , FORMAT(CAST(CheckOut as datetime), 'hh:mm tt') AS CheckOut
						                                , CASE WHEN CAST(DATEDIFF(MINUTE,CheckIn,CheckOut)/60.0 as decimal(18,1)) >= 5 
																THEN CAST(DATEDIFF(MINUTE,CheckIn,CheckOut)/60.0 as decimal(18,1)) - 1 
																ELSE CAST(DATEDIFF(MINUTE,CheckIn,CheckOut)/60.0 as decimal(18,1)) END as TimeDuration
					                                FROM cte  ";

                var reportData = await _unit.DapperRepository.GetListQueryAsync<AttendanceSummaryResponse>(query);

                if (reportData?.Count > 0)
                {
                    // Calling and getting path of RDLC Report
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Payroll.AttendanceListPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<AttendanceSummaryResponse>()
                    {
                        Data = reportData,
                        Parms = null,
                        ReportConfig = reportConfig,
                        DownloadFileName = "Attendance Print"
                    };
                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<AttendanceSummaryResponse>> GetLateHoursReport(PayrollParameterRequest request)
        {
            try
            {
                var query = $@"
                                    DECLARE @DepartmentTypeId INT = {request.DepartmentTypeId};
                                    WITH cte
		                                AS
		                                (
			                                SELECT
				                                    e.EmployeeId
				                                    , e.EmployeeName
				                                    , e.FatherName
				                                    , e.DepartmentsId
				                                    , dept.Name as DepartmentsName
                                                    , desig.DesignationName
				                                    , attendance.AttendanceDate
				                                    , CheckIn
				                                    , CheckOut
                                                    , DetailMarkType
				                                    FROM Employee e
			                                    INNER JOIN 
			                                    (
				                                    SELECT  mast.EmployeeId
				                                        , AttendanceDate
				                                        , CheckIn
				                                        , CAse When CheckOut > '17:30:00.0000000' AND @DepartmentTypeId = 1 then '17:30:00.0000000' else CheckOut end AS CheckOut
				                                        , det.DetailMarkType
				                                        FROM Attendance mast
					                                        INNER JOIN AttendanceDetail det ON mast.AttendanceId=det.AttendanceId
					                                        AND Month(mast.AttendanceDate)={request.MonthOf}
					                                        AND YEAR(mast.AttendanceDate)={request.YearOf}
                                                            AND CAST(DATEDIFF(MINUTE,CheckIn,CAse When CheckOut > '17:30:00.0000000' AND @DepartmentTypeId = 1 then '17:30:00.0000000' else CheckOut end)/60.0 as decimal(18,2)) < 9
			                                    ) attendance
					                                    ON e.EmployeeId=attendance.EmployeeId
					                                    INNER JOIN Departments dept ON e.DepartmentsId=dept.DepartmentsId	
                                                        INNER JOIN Designation desig ON e.DesignationId=desig.DesignationId
                                                        WHERE e.OutletId={request.OutletId}
                                                        {(request.DepartmentIds.Length > 0 ? $" AND e.DepartmentsId IN ({request.DepartmentIds})" : "")}
                                                        {(request.EmployeeIds.Length > 0 ? $" AND e.EmployeeId IN ({request.EmployeeIds})" : "")}
                                                        AND e.IsActive = 1
                                            )
			                                SELECT  EmployeeId
						                                , EmployeeName
						                                , FatherName
                                                        , DepartmentsId
						                                , DepartmentsName
                                                        , DesignationName
						                                , AttendanceDate
                                                        , DATENAME(dw, AttendanceDate) AS DayName
                                                        , DetailMarkType
						                                , FORMAT(CAST(CheckIn as datetime), 'hh:mm tt') AS CheckIn
						                                , FORMAT(CAST(CAse When CheckOut > '17:30:00.0000000' AND @DepartmentTypeId = 1 then '17:30:00.0000000' else CheckOut end as datetime), 'hh:mm tt') AS CheckOut
						                                , CASE WHEN CAST(DATEDIFF(MINUTE,CheckIn,CAse When CheckOut > '17:30:00.0000000' AND @DepartmentTypeId = 1 then '17:30:00.0000000' else CheckOut end)/60.0 as decimal(18,2)) <= 5 THEN
						                                 8 - CAST(DATEDIFF(MINUTE,CheckIn,CAse When CheckOut > '17:30:00.0000000' AND @DepartmentTypeId = 1 then '17:30:00.0000000' else CheckOut end)/60.0 as decimal(18,2)) ELSE 
														 9 - CAST(DATEDIFF(MINUTE,CheckIn,CAse When CheckOut > '17:30:00.0000000' AND @DepartmentTypeId = 1 then '17:30:00.0000000' else CheckOut end)/60.0 as decimal(18,2)) END AS TimeDuration
					                                FROM cte  ";

                var reportData = await _unit.DapperRepository.GetListQueryAsync<AttendanceSummaryResponse>(query);

                if (reportData?.Count > 0)
                {
                    // Calling and getting path of RDLC Report
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Payroll.AttendanceListPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<AttendanceSummaryResponse>()
                    {
                        Data = reportData,
                        Parms = null,
                        ReportConfig = reportConfig,
                        DownloadFileName = "Late Hours Print"
                    };
                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<OvertimeReportRequest>> GetOvertimeList(PayrollParameterRequest request)
        {
            try
            {
                var query = $@"  WITH cte
		                                AS
		                                (
			                                SELECT
				                                    e.EmployeeId
				                                    , e.EmployeeName
				                                    , e.FatherName
				                                    , e.DepartmentsId
				                                    , dept.Name as DepartmentsName
                                                    , desig.DesignationName
				                                    , attendance.OvertimeDate
				                                    , CheckIn
				                                    , CheckOut
                                                    , DetailMarkType
				                                    FROM Employee e
			                                    INNER JOIN 
			                                    (
				                                    SELECT  mast.EmployeeId
				                                        , OvertimeDate
				                                        , CheckIn
				                                        , CheckOut
				                                        , det.DetailMarkType
				                                        FROM Overtime mast
					                                        INNER JOIN OvertimeDetail det ON mast.OvertimeId=det.OvertimeId
					                                        AND Month(mast.OvertimeDate)={request.MonthOf}
					                                        AND YEAR(mast.OvertimeDate)={request.YearOf}
			                                    ) attendance
					                                    ON e.EmployeeId=attendance.EmployeeId
					                                    INNER JOIN Departments dept ON e.DepartmentsId=dept.DepartmentsId	
                                                        INNER JOIN Designation desig ON e.DesignationId=desig.DesignationId
                                                        WHERE e.OutletId={request.OutletId}
                                                        {(request.DepartmentIds.Length > 0 ? $" AND e.DepartmentsId IN ({request.DepartmentIds})" : "")}
                                                        {(request.EmployeeIds.Length > 0 ? $" AND e.EmployeeId IN ({request.EmployeeIds})" : "")}
                                            )
			                                SELECT  EmployeeId
						                                , EmployeeName
						                                , FatherName
                                                        , DepartmentsId
						                                , DepartmentsName
                                                        , DesignationName
						                                , OvertimeDate
						                                , DATENAME(dw, OvertimeDate) AS DayName
                                                        , DetailMarkType
						                                , FORMAT(CAST(CAse When CheckIn > '17:30:00.0000000' then '17:31:00.0000000' else CheckIn end as datetime), 'hh:mm tt') AS CheckIn
						                                , FORMAT(CAST(CheckOut as datetime), 'hh:mm tt') AS CheckOut
						                                , CAST(DATEDIFF(MINUTE,CAse When CheckIn > '17:30:00.0000000' then '17:31:00.0000000' else CheckIn end,CheckOut)/60.0 as decimal(18,1)) as TimeDuration
					                                FROM cte";

                var reportData = await _unit.DapperRepository.GetListQueryAsync<OvertimeReportRequest>(query);

                if (reportData?.Count > 0)
                {
                    // Calling and getting path of RDLC Report
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Payroll.OvertimeListPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<OvertimeReportRequest>()
                    {
                        Data = reportData,
                        Parms = null,
                        ReportConfig = reportConfig,
                        DownloadFileName = "Overtime Print"
                    };
                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<SalarySheet>> GetSalarySheet(PayrollParameterRequest request)
        {
            try
            {
                //var reportData = await _unit
                //                    .SalarySheetRepository.GetAsync(x => x.SalaryDate.Month == request.MonthOf
                //                                                           && x.SalaryDate.Year == request.YearOf);

                var query = $@" SELECT ss.*
                                    , e.EmployeeName
                                    , e.FatherName
			                        , DesignationName
                                    , e.DepartmentsId
			                        , dept.Name as DepartmentsName
		                                FROM SalarySheet ss
		                                INNER JOIN Employee e ON ss.EmployeeId=e.EmployeeId
		                                INNER JOIN Departments dept ON e.DepartmentsId=dept.DepartmentsId
		                                INNER JOIN Designation desg ON e.DesignationId=desg.DesignationId
		                                WHERE MONTH(SalaryDate)={request.MonthOf}
		                                AND YEAR(SalaryDate)={request.YearOf}
                                        AND dept.DepartmentTypeId = {request.DepartmentTypeId}";

                var reportData = await _unit.DapperRepository.GetListQueryAsync<SalarySheet>(query);

                if (reportData.ToList()?.Count > 0)
                {
                    var reportConfig = new RdlcReportConfiguration();
                    if (request.DepartmentTypeId == (int)DepartmentType.Directors)
                    {
                        reportConfig = _rdlcConfigs
                                    ?.Value
                                    ?.Configs
                                    ?.Find(x => x.ReportName == ReportName.Payroll.DirectorSalarySheetPrint.ToString()) ?? new RdlcReportConfiguration();
                    }
                    else
                    {
                        reportConfig = _rdlcConfigs
                                    ?.Value
                                    ?.Configs
                                    ?.Find(x => x.ReportName == ReportName.Payroll.SalarySheetPrint.ToString()) ?? new RdlcReportConfiguration();
                    }

                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;
                    var reportParams = new Dictionary<string, string>();
                    reportParams.Add("pmrOutletName", _currentLogin?.OutletName ?? "-");
                    reportParams.Add("pmrAddress", _currentLogin?.Address ?? "-");
                    reportParams.Add("pmrOutletImage", _currentLogin?.OutletImage ?? "-");
                    reportParams.Add("pmrDepartmentTypeId", request.DepartmentTypeId.ToString() ?? "1");
                    reportParams.Add("pmrBaseUrl", baseUrl ?? "-");

                    var dataParms = new ReportDataParms<SalarySheet>()
                    {
                        Data = reportData,
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = "Salary Sheet Print"
                    };
                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<AttendanceRegister>> GetMonthlyAttendanceRegister(PayrollParameterRequest request)
        {
            try
            {
                var query = $@"WITH all_dates AS (
                                  SELECT DATEADD(DAY, number, DATEFROMPARTS({request.YearOf}, {request.MonthOf}, 1)) AS AttendanceDate
                                  FROM master..spt_values
                                  WHERE type = 'P' AND DATEADD(DAY, number, DATEFROMPARTS({request.YearOf}, {request.MonthOf}, 1)) < DATEADD(MONTH, 1, DATEFROMPARTS({request.YearOf}, {request.MonthOf}, 1))
                                )

                                SELECT 
                                  ad.AttendanceDate AS DateOfMonth
                                  , DAY(ad.AttendanceDate) as DaysOfMonth
                                  , E.EmployeeId
                                  , E.EmployeeName
                                  , E.DepartmentsName
                                  , E.DesignationName
                                  , CASE
                                        WHEN ad.AttendanceDate > CAST(GETDATE() AS DATE) THEN ''
		                                WHEN DATEPART(WEEKDAY, ad.AttendanceDate) = 1 THEN 'S'
		                                WHEN a.AttendanceStatusId IS NULL THEN '-'
		                                ELSE ATS.ShortDescription
	                                END AS StatusName
                                  , ad.AttendanceDate
                                FROM all_dates ad
                                CROSS JOIN (
                                  SELECT DISTINCT EmployeeId
                                  , EmployeeName
                                  , DepartmentsName
                                  , DesignationName
                                  , DepartmentsId
                                  FROM V_EMPLOYEE
                                  {(request.DepartmentIds.Length > 0 ? $" WHERE DepartmentsId IN ({request.DepartmentIds})" : $"{(request.EmployeeIds.Length > 0 ? $"WHERE EmployeeId IN ({request.EmployeeIds})" : "")}")}
                                  {(request.EmployeeIds.Length > 0 ? $"AND EmployeeId IN ({request.EmployeeIds})" : "")}
                                ) e
                                LEFT JOIN attendance a ON CONVERT(DATE, a.AttendanceDate) = ad.AttendanceDate AND a.EmployeeId = e.EmployeeId
                                LEFT JOIN AttendanceStatus AS ATS ON ATS.AttendanceStatusId = a.AttendanceStatusId
                                ORDER BY e.EmployeeId, ad.AttendanceDate;";

                var reportData = await _unit.DapperRepository.GetListQueryAsync<AttendanceRegister>(query);

                if (reportData.ToList()?.Count > 0)
                {
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Payroll.AttendanceRegisterPrint.ToString()) ?? new RdlcReportConfiguration();

                    var dataParms = new ReportDataParms<AttendanceRegister>()
                    {
                        Data = reportData,
                        Parms = null,
                        ReportConfig = reportConfig,
                        DownloadFileName = "Attendance Register Print"
                    };
                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ReportDataParms<SalarySheet>> GetPaySlip(SalarySheet request)
        {
            try
            {
                if (request != null)
                {
                    var reportConfig = _rdlcConfigs
                                        ?.Value
                                        ?.Configs
                                        ?.Find(x => x.ReportName == ReportName.Payroll.PaySlipPrint.ToString()) ?? new RdlcReportConfiguration();

                    var baseUrl = _config.GetSection("ApplicationSettings:BaseUrl").Value;

                    var reportParams = new Dictionary<string, string>();
                    reportParams.Add("pmrOutletName", _currentLogin?.OutletName ?? "-");
                    reportParams.Add("pmrAddress", _currentLogin?.Address ?? "-");
                    reportParams.Add("pmrOutletImage", _currentLogin?.OutletImage ?? "-");
                    reportParams.Add("pmrBaseUrl", baseUrl ?? "-");

                    var dataParms = new ReportDataParms<SalarySheet>()
                    {
                        Data = new List<SalarySheet>() { request },
                        Parms = reportParams,
                        ReportConfig = reportConfig,
                        DownloadFileName = "Salary Sheet Print"
                    };
                    return dataParms;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
    }
}