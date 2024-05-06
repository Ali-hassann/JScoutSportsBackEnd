//using System.Data;
//using Core.CL.Helper;
//using Newtonsoft.Json;

//namespace AMNSystemsERP.BL.Repositories.EmployeePayroll.EmployeeReports
//{
//    public class ReportsService : IReportsService
//    {
//        private readonly IUnitOfWork _unit;
//        private readonly IDocumentHelperService _documentHelperService;
//        private readonly IEmployeeService _employeeService;
//        private readonly IAppConfiguration _appConfiguration;
//        private readonly HttpCaller _httpCaller;
//        private readonly IRDLCReportService _rdlcReportService;
//        private readonly ICacheUnit _cacheService;

//        public ReportsService(IUnitOfWork unit
//        , IDocumentHelperService documentHelperService
//        , IEmployeeService employeeService
//        , IAppConfiguration appConfiguration
//        , IRDLCReportService rdlcReportService
//        , ICacheUnit cacheService)
//        {
//            _unit = unit;
//            _documentHelperService = documentHelperService;
//            _employeeService = employeeService;
//            _appConfiguration = appConfiguration;
//            _httpCaller = new HttpCaller(_appConfiguration.RdlcReportUrl);
//            _rdlcReportService = rdlcReportService;
//            _cacheService = cacheService;
//        }

//        public async Task<string> GetEmployeeAttendanceSummaryReport(EmployeeFilterRequest request)
//        {
//            try
//            {
//                if (request.FromDate == null)
//                {
//                    request.FromDate = DateHelper.GetCurrentDate();
//                }
//                if (request.ToDate == null)
//                {
//                    request.ToDate = DateHelper.GetCurrentDate();
//                }
//                // Getting EmployeeAttendanceStatusIds 
//                var employeeAttendanceStatusIds = CommonHelper.GetEnumIdsFromEnumString(typeof(EmployeeAttendanceStatus), request.StatusIds);
//                //

//                var pOrganizationId = DBHelper.GenerateDapperParameter("ORGANIZATIONID", request.OrganizationId, DbType.Int64);
//                var pOutletIds = DBHelper.GenerateDapperParameter("OUTLETIDS", request.OutletIds ?? "", DbType.String);
//                var pEmployeeIds = DBHelper.GenerateDapperParameter("EMPLOYEEIDS", request.EmployeeIds ?? "", DbType.String);
//                var pFromDate = DBHelper.GenerateDapperParameter("FROMDATE",
//                                                                  DateHelper.GetDateFormat
//                                                                  (
//                                                                      request.FromDate.Value,
//                                                                      false,
//                                                                      true,
//                                                                      DateFormats.SqlDateFormat
//                                                                  ), DbType.String);
//                var pToDate = DBHelper.GenerateDapperParameter("TODATE",
//                                                                DateHelper.GetDateFormat
//                                                                (
//                                                                    request.ToDate.Value,
//                                                                    false,
//                                                                    true,
//                                                                    DateFormats.SqlDateFormat
//                                                                ), DbType.String);
//                var pEmployeeAttendanceStatusIds = DBHelper.GenerateDapperParameter("EMPLOYEEATTENDANCESTATUSIDS", employeeAttendanceStatusIds ?? "", DbType.String);
//                var pDepartmentIds = DBHelper.GenerateDapperParameter("DEPARTMENTIDS", request.DepartmentIds ?? "", DbType.String);

//                var reportData = await _unit
//                                       .DapperRepository
//                                       .GetManyWithStoreProcedureAsync<EmployeeAttendanceSummaryReport>("REPORT_GET_EMPLOYEE_ATTENDANCE_SUMMARY",
//                                                                                                         DBHelper.GetDapperParms
//                                                                                                         (
//                                                                                                             pOrganizationId,
//                                                                                                             pOutletIds,
//                                                                                                             pEmployeeIds,
//                                                                                                             pFromDate,
//                                                                                                             pToDate,
//                                                                                                             pEmployeeAttendanceStatusIds
//                                                                                                         ));
//                if (reportData?.Count > 0)
//                {
//                    // Assigning FatherName to LastName if (LastName Empty)
//                    reportData
//                        .ForEach(x =>
//                        {
//                            x.LastName = string.IsNullOrEmpty(x.LastName) ? "" : x.LastName;
//                        });
//                    //


//                    // Setting Params For Report
//                    var reportParams = new Dictionary<string, string>();
//                    reportParams.Add("pmrFromDate",
//                                      DateHelper.GetDateFormat
//                                      (
//                                          request.FromDate.Value.Date,
//                                          false,
//                                          false,
//                                          DateFormats.DefaultFormat
//                                      ));
//                    reportParams.Add("pmrToDate",
//                                      DateHelper.GetDateFormat
//                                      (
//                                          request.ToDate.Value.Date,
//                                          false,
//                                          false,
//                                          DateFormats.DefaultFormat
//                                      ));
//                    //

//                    // Calling and getting path of RDLC Report
//                    string date = DateHelper.GetReportFormatDate(request.FromDate.ToString(), request.ToDate.ToString());
//                    var downloadFileName = "Employee Attendence Summary-" + date;
//                    return await _rdlcReportService.GetReportData(ReportName.Employee.EmployeeAttendanceSummaryReport, reportData, reportParams, downloadFileName);
//                    //
//                }
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//            return null;
//        }

//        public async Task<string> CreateEmployeeCard(EmployeeCardFilterRequest rdlcReportRequest)
//        {
//            try
//            {
//                var outletId = CommonHelper.ConvertStringToLong(rdlcReportRequest.OutletIds);

//                if (outletId > 0)
//                {
//                    // Getting Employee Data for Card Generation
//                    var pOrganizationId = DBHelper.GenerateDapperParameter("ORGANIZATIONID", rdlcReportRequest.OrganizationId, DbType.Int64);
//                    var pOutletIds = DBHelper.GenerateDapperParameter("OUTLETIDS", rdlcReportRequest.OutletIds ?? "", DbType.String);
//                    var pDepartmentsIds = DBHelper.GenerateDapperParameter("DEPARTMENTIDS", rdlcReportRequest.DepartmentsIds ?? "", DbType.String);
//                    var pEmployeeIds = DBHelper.GenerateDapperParameter("EMPLOYEEIDS", rdlcReportRequest.EmployeeIds ?? "", DbType.String);

//                    var employeesListResult = await _unit
//                                                    .DapperRepository
//                                                    .GetManyWithStoreProcedureAsync<CardRequest>("EMPLOYEE_GET_EMPLOYEESLIST_FOR_QRCODE_GENERATION",
//                                                                                                  DBHelper.GetDapperParms
//                                                                                                  (
//                                                                                                      pOrganizationId,
//                                                                                                      pOutletIds,
//                                                                                                      pDepartmentsIds,
//                                                                                                      pEmployeeIds
//                                                                                                  ));
//                    //                              

//                    // Generating QRCode Image & Update Employee QRCode Image Path
//                    if ((employeesListResult?.Count() ?? 0) > 0)
//                    {
//                        var employeeListToUpdate = _documentHelperService.GenerateAndSaveQRCodeImage(employeesListResult);
//                        await _employeeService.UpdateEmployeeQRCodePath(employeeListToUpdate);
//                    }
//                    //

//                    // Report Working Start
//                    // Getting EmployeeList For CardReport
//                    var reportData = await _unit
//                                           .DapperRepository
//                                           .GetManyWithStoreProcedureAsync<EmployeeCardResponse>("REPORT_GET_EMPLOYEESLIST_FOR_EMPLOYEE_CARD",
//                                                                                                  DBHelper.GetDapperParms
//                                                                                                  (
//                                                                                                      pOrganizationId,
//                                                                                                      pOutletIds,
//                                                                                                      pDepartmentsIds,
//                                                                                                      pEmployeeIds
//                                                                                                  ));
//                    //
//                    if (reportData?.Count > 0)
//                    {
//                        // Assigning FatherName to LastName if (LastName Empty)
//                        reportData
//                            .ForEach(x =>
//                            {
//                                x.LastName = string.IsNullOrEmpty(x.LastName) ? "" : x.LastName;
//                            });
//                        // 

//                        // Getting ReportDirectoryBasePath
//                        var reportBasePath = DocumentHelper.GetDocumentDirectoryRootPath()?.Replace(EnvironmentHelper.EnvironmentName, "") ?? "";
//                        //                    

//                        // Getting OrganizationProfile From Cache for Outlet & BranhBank Information
//                        var instituteProfile = await GetOrganizationProfile(rdlcReportRequest.OrganizationId, outletId);
//                        //

//                        // Setting Params For Report
//                        var reportParams = new Dictionary<string, string>();
//                        string toDate = DateHelper.GetDateFormat(rdlcReportRequest.ToDate.Value, false, false, DateFormats.DefaultFormat);
//                        reportParams.Add("pmrValidUpto", toDate);
//                        reportParams.Add("pmrBasePath", reportBasePath);
//                        reportParams.Add("pmrOutletImagePath", instituteProfile.ImagePath);
//                        //

//                        // Calling and getting path of RDLC Report

//                        var downloadFileName = "Employee Cards Valid Up to-" + toDate;

//                        return await _rdlcReportService.GetReportData(ReportName.Employee.EmployeeCardReport, reportData, reportParams, downloadFileName);
//                        //
//                    }
//                    //
//                }
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//            return null;
//        }

//        public async Task<string> ViewEmployeesPayrollSlip(PayrollSummaryRequest rdlcReportRequest)
//        {
//            try
//            {
//                // Getting PayrollSignOffConfig data If Exist
//                var payrollSignOffConfig = await _unit
//                                                 .PayrollSignOffConfigRepository
//                                                 .GetSingleAsync(x => x.Year == rdlcReportRequest.Year
//                                                                 && x.MonthId == rdlcReportRequest.MonthId
//                                                                 && x.OutletId == Convert.ToInt64(rdlcReportRequest.OutletIds)
//                                                                 && x.OrganizationId == rdlcReportRequest.OrganizationId);
//                //
//                if (payrollSignOffConfig != null)
//                {
//                    // Converting as DeSerializeObject for PayrollRdlcReportRequest
//                    var request = JsonConvert.DeserializeObject<PayrollRdlcReportRequest>(payrollSignOffConfig.PayrollData);
//                    //

//                    // Generating Rdlc Report Link here
//                    var endPoint = $"{_httpCaller._baseAddress}Payroll/CreateEmployeePayrollSlip";
//                    return _httpCaller.Post<RdlcReportResponse>(endPoint, request)?.ReportPath ?? "";
//                    //                 
//                }
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//            return null;
//        }
//    }
//}