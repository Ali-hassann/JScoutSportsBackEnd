using AMNSystemsERP.BL.Repositories.EmployeePayroll.PayrollReports;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.EmployeePayrollModels;
using AMNSystemsERP.CL.Models.RDLCModels;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;
using Microsoft.AspNetCore.Mvc;
using SMSRdlcReports.BL.Repositories.Reports;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/PayrollReports")]
    public class PayrollReportsController : ApiController
    {
        private readonly IPayrollReportsService _reportsService;
        private readonly ICommonRDLCReportsService _commonRDLCReportsService;

        public PayrollReportsController(IPayrollReportsService reportsService, ICommonRDLCReportsService commonRDLCReportsService)
        {
            _reportsService = reportsService;
            _commonRDLCReportsService = commonRDLCReportsService;
        }

        [HttpPost]
        [Route("GetEmployeesList")]
        public async Task<ActionResult> GetEmployeesList([FromBody] PayrollParameterRequest request)
        {
            try
            {
                if (request?.OutletId > 0)
                {
                    var reportResponse = await _reportsService.GetEmployeesList(request);
                    return await GetReportData(reportResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetAttendanceList")]
        public async Task<ActionResult> GetAttendanceList([FromBody] PayrollParameterRequest request)
        {
            try
            {
                if (request?.OutletId > 0 && request.MonthOf > 0 && request.YearOf > 0 && request.DepartmentTypeId > 0)
                {
                    var reprotResponse = await _reportsService.GetAttendanceList(request);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetLateHoursReport")]
        public async Task<ActionResult> GetLateHoursReport([FromBody] PayrollParameterRequest request)
        {
            try
            {
                if (request?.MonthOf > 0 && request.YearOf > 0)
                {
                    var reprotResponse = await _reportsService.GetLateHoursReport(request);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetOvertimeList")]
        public async Task<ActionResult> GetOvertimeList([FromBody] PayrollParameterRequest request)
        {
            try
            {
                if (request?.OutletId > 0 && request.MonthOf > 0 && request.YearOf > 0)
                {
                    var reprotResponse = await _reportsService.GetOvertimeList(request);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetSalarySheet")]
        public async Task<ActionResult> GetSalarySheet([FromBody] PayrollParameterRequest request)
        {
            try
            {
                if (request?.OutletId > 0)
                {
                    var reprotResponse = await _reportsService.GetSalarySheet(request);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetMonthlyAttendanceRegister")]
        public async Task<ActionResult> GetMonthlyAttendanceRegister([FromBody] PayrollParameterRequest request)
        {
            try
            {
                if (request?.OutletId > 0 && request.MonthOf > 0 && request.YearOf > 0)
                {
                    var reprotResponse = await _reportsService.GetMonthlyAttendanceRegister(request);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetPaySlip")]
        public async Task<ActionResult> GetPaySlip([FromBody] SalarySheet request)
        {
            try
            {
                if (request != null)
                {
                    var reprotResponse = await _reportsService.GetPaySlip(request);
                    return await GetReportData(reprotResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ActionResult> GetReportData<T>(ReportDataParms<T> reportData)
        {
            try
            {
                if (!string.IsNullOrEmpty(reportData?.ReportConfig?.ReportName) && reportData?.Data?.Count > 0)
                {
                    // Getting TempFolder Path
                    var tempFolderPath = @$"{DocumentHelper.GetDocumentDirectoryRootPath()}\TempDocuments";
                    DocumentHelper.CreateFolder(tempFolderPath);

                    var fileName = reportData.DownloadFileName != "" ? reportData.DownloadFileName : Guid.NewGuid().ToString();
                    var reportConfig = reportData?.ReportConfig;
                    // Creating RdlcReportRequest Model
                    var reportRequest = new RdlcReportRequest<T>()
                    {
                        ReportData = reportData.Data,
                        ReportDataSourceName = reportConfig.DataSource,
                        RdlcReportType = reportConfig.RDLCReportType,
                        RdlcReportName = $"{reportConfig.ReportName}.rdlc",
                        ReportFileName = $"{fileName}",
                        RenderType = reportConfig.RenderType,
                        TempFolderPath = tempFolderPath,
                        ReportParams = reportData.Parms,
                    };

                    // Generating Rdlc Report Link here
                    var bytesData = await _commonRDLCReportsService.GenerateRdlcReport(reportRequest);

                    return File(bytesData, "application/pdf", fileName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Ok("");
        }
    }
}
