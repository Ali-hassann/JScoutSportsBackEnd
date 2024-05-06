using AMNSystemsERP.BL.Repositories.EmployeePayroll.AttendanceRepo;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Attendance;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Overtime;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/Attendance")]
    public class AttendanceController : ApiController
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost]
        [Route("GetAttendanceSummaryAndHistory")]
        public async Task<List<AttendanceSummaryResponse>> GetAttendanceSummaryAndHistory([FromBody] EmployeeFilterRequest request)
        {
            if (request?.OrganizationId > 0 && request.OutletId > 0
                && !string.IsNullOrEmpty(request.EmployeeIds))
            {
                try
                {
                    return await _attendanceService.GetAttendanceSummaryAndHistory(request);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return new List<AttendanceSummaryResponse>();
        }

        [HttpPost]
        [Route("GetAttendanceList")]
        public async Task<List<AttendanceResponse>> GetAttendanceList([FromBody] EmployeeFilterRequest request)
        {
            if (request?.OrganizationId > 0 && request.OutletId > 0)
            {
                try
                {
                    return await _attendanceService.GetAttendanceList(request);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return new List<AttendanceResponse>();
        }

        [HttpPost]
        [Route("SaveAttendance")]
        public async Task<List<AttendanceRequest>> SaveAttendance([FromBody] List<AttendanceRequest> request)
        {
            if (request?.Count > 0)
            {
                try
                {
                    return await _attendanceService.SaveAttendance(request);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        [HttpGet]
        [Route("GetAttendanceDetailList")]
        public async Task<List<AttendanceDetailResponse>> GetAttendanceDetailList(long attendanceId)
        {
            if (attendanceId > 0)
            {
                try
                {
                    return await _attendanceService.GetAttendanceDetailList(attendanceId);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return new List<AttendanceDetailResponse>();
        }

        [HttpPost]
        [Route("SaveAttendanceDetailList")]
        public async Task<bool> SaveAttendanceDetailList([FromBody] List<AttendanceDetailRequest> request)
        {
            if (request?.Count > 0)
            {
                try
                {
                    return await _attendanceService.SaveAttendanceDetailList(request);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }

        [HttpPost]
        [Route("RemoveAttendanceDetail")]
        public async Task<bool> RemoveAttendanceDetail(long attendanceDetailId)
        {
            try
            {
                if (attendanceDetailId > 0)
                {
                    return await _attendanceService.RemoveAttendanceDetail(attendanceDetailId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("GetOvertimeList")]
        public async Task<List<OvertimeResponse>> GetOvertimeList([FromBody] EmployeeFilterRequest request)
        {
            if (request?.OrganizationId > 0 && request.OutletId > 0)
            {
                try
                {
                    return await _attendanceService.GetOvertimeList(request);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return new List<OvertimeResponse>();
        }

        [HttpPost]
        [Route("SaveOvertime")]
        public async Task<bool> SaveOvertime([FromBody] List<OvertimeRequest> request)
        {
            if (request?.Count > 0)
            {
                try
                {
                    return await _attendanceService.SaveOvertime(request);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }

        [HttpGet]
        [Route("GetOvertimeDetailList")]
        public async Task<List<OvertimeDetailResponse>> GetOvertimeDetailList(long OvertimeId)
        {
            if (OvertimeId > 0)
            {
                try
                {
                    return await _attendanceService.GetOvertimeDetailList(OvertimeId);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return new List<OvertimeDetailResponse>();
        }

        [HttpPost]
        [Route("SaveOvertimeDetailList")]
        public async Task<bool> SaveOvertimeDetailList([FromBody] List<OvertimeDetailRequest> request)
        {
            if (request?.Count > 0)
            {
                try
                {
                    return await _attendanceService.SaveOvertimeDetailList(request);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }

        [HttpPost]
        [Route("RemoveOvertimeDetail")]
        public async Task<bool> RemoveOvertimeDetail(long overtimeDetailId)
        {
            if (overtimeDetailId > 0)
            {
                try
                {
                    return await _attendanceService.RemoveOvertimeDetail(overtimeDetailId);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }
    }
}
