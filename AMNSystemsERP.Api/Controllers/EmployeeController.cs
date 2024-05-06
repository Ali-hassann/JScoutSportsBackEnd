using AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee;
using Microsoft.AspNetCore.Mvc;
using AMNSystemsERP.BL.Repositories.EmployeePayroll.EmployeeRepo;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/Employee")]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<EmployeeBasicRequest> AddEmployee([FromBody] EmployeeRequest request)
        {
            if (request?.Employee?.OrganizationId > 0
                && request.Employee.DepartmentsId > 0
                && request.Employee.OutletId > 0)
            {
                try
                {
                    return await _employeeService.AddEmployee(request);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateEmployee")]
        public async Task<EmployeeBasicRequest> UpdateEmployee([FromBody] EmployeeRequest request)
        {
            try
            {
                if (request?.Employee?.OrganizationId > 0
                && request.Employee.EmployeeId > 0
                && request.Employee.DepartmentsId > 0
                && request.Employee.OutletId > 0)
                {
                    return await _employeeService.UpdateEmployee(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveEmployee")]
        public async Task<bool> RemoveEmployee(long employeeId)
        {
            try
            {
                if (employeeId > 0)
                {

                    return await _employeeService.RemoveEmployee(employeeId);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        [HttpGet]
        [Route("GetEmployeeList")]
        public async Task<List<EmployeeBasicRequest>> GetEmployeeList(long organizationId, long outletId)
        {
            if (organizationId > 0 && outletId > 0)
            {
                try
                {
                    return await _employeeService.GetEmployeeList(organizationId, outletId);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return new List<EmployeeBasicRequest>();
        }

        [HttpGet]
        [Route("GetEmployeeById")]
        public async Task<EmployeeRequest> GetEmployeeById(long employeeId)
        {
            try
            {
                if (employeeId > 0)
                {
                    return await _employeeService.GetEmployeeById(employeeId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        // -------------------------------------------------------------------------
        // ------------------ Employee Document Section ----------------------------
        // -------------------------------------------------------------------------

        [HttpPost]
        [Route("SaveEmployeeDocuments")]
        public async Task<List<EmployeeDocumentsRequest>> SaveEmployeeDocuments([FromBody] List<EmployeeDocumentsRequest> request)
        {
            try
            {
                if (request?.Count > 0)
                {
                    return await _employeeService.SaveEmployeeDocuments(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetEmployeeDocuments")]
        public async Task<List<EmployeeDocumentsRequest>> GetEmployeeDocuments([FromBody] EmployeeDocumentsRequest request)
        {
            try
            {
                if (request?.OrganizationId > 0
                    && request.OutletId > 0
                    && request.EmployeeId > 0)
                {
                    return await _employeeService.GetEmployeeDocuments(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        // -------------------------------------------------------------------------
        // ------------------ Employee Qualification Section -----------------------
        // -------------------------------------------------------------------------

        [HttpPost]
        [Route("SaveQualification")]
        public async Task<EmployeeQualificationsRequest> SaveQualification([FromBody] EmployeeQualificationsRequest request)
        {
            try
            {
                if (request?.EducationalOrganizationsId > 0
                    && request.QualificationId > 0
                    && request.PassingYear > 0
                    && request.EmployeeId > 0
                    && (
                          request.CGPA > 0
                          ||
                          request.Percentage > 0
                       )
                    )
                {
                    return await _employeeService.SaveQualification(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveQualification")]
        public async Task<bool> RemoveQualification(long employeeQualificationsId)
        {
            try
            {
                if (employeeQualificationsId > 0)
                {
                    return await _employeeService.RemoveQualification(employeeQualificationsId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetQualificationById")]
        public async Task<EmployeeQualificationsRequest> GetQualificationById(long employeeQualificationsId)
        {
            try
            {
                if (employeeQualificationsId > 0)
                {
                    return await _employeeService.GetQualificationById(employeeQualificationsId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetQualificationListByEmployeeId")]
        public async Task<List<EmployeeQualificationsRequest>> GetQualificationListByEmployeeId(long employeeId)
        {
            try
            {
                if (employeeId > 0)
                {
                    return await _employeeService.GetQualificationListByEmployeeId(employeeId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        //
    }
}
