using Microsoft.AspNetCore.Mvc;
using AMNSystemsERP.BL.Repositories.EmployeePayroll.CommonDataRepo;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Base;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Allowances;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/CommonData")]
    public class CommonDataController : ApiController
    {
        private ICommonDataService _commonDataService;

        public CommonDataController(ICommonDataService masterDataService)
        {
            _commonDataService = masterDataService;
        }

        // ------------------ AllowanceType Section Start ------------------
        [HttpGet]
        [Route("GetAllowanceTypeList")]
        public async Task<List<AllowanceTypeRequest>> GetAllowanceTypeList(long organizationId, long outletId)
        {
            if (organizationId > 0 && outletId > 0)
            {
                try
                {
                    return await _commonDataService.GetAllowanceTypeList(organizationId, outletId);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        [HttpPost]
        [Route("AddAllowanceType")]
        public async Task<AllowanceTypeRequest> AddAllowanceType([FromBody] AllowanceTypeRequest request)
        {
            if (!string.IsNullOrEmpty(request?.Name)
                && request.OutletId > 0
                && request.OrganizationId > 0)
            {
                try
                {
                    return await _commonDataService.AddAllowanceType(request);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateAllowanceType")]
        public async Task<AllowanceTypeRequest> UpdateAllowanceType([FromBody] AllowanceTypeRequest request)
        {
            if (!string.IsNullOrEmpty(request?.Name)
                && request.AllowanceTypeId > 0
                && request.OutletId > 0
                && request.OrganizationId > 0)
            {
                try
                {
                    return await _commonDataService.UpdateAllowanceType(request);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveAllowanceType")]
        public async Task<bool> RemoveAllowanceType(long allowanceTypeId)
        {
            if (allowanceTypeId > 0)
            {
                try
                {
                    return await _commonDataService.RemoveAllowanceType(allowanceTypeId);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return false;
        }

        // ------------------ Departments Section Start ------------------

        [HttpGet]
        [Route("GetDepartmentsList")]
        public async Task<List<DepartmentsRequest>> GetDepartmentsList(long organizationId)
        {
            try
            {
                if (organizationId > 0)
                {
                    return await _commonDataService.GetDepartmentsList(organizationId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new List<DepartmentsRequest>();
        }

        [HttpPost]
        [Route("AddDepartment")]
        public async Task<DepartmentsRequest> AddDepartment([FromBody] DepartmentsRequest request)
        {
            if (request?.OrganizationId > 0
                && !string.IsNullOrEmpty(request.Name))
            {
                try
                {
                    return await _commonDataService.AddDepartment(request);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateDepartment")]
        public async Task<DepartmentsRequest> UpdateDepartment([FromBody] DepartmentsRequest request)
        {
            if (request?.DepartmentsId > 0
                && request.OrganizationId > 0
                && !string.IsNullOrEmpty(request.Name))
            {
                try
                {
                    return await _commonDataService.UpdateDepartment(request);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveDepartment")]
        public async Task<bool> RemoveDepartment(long departmentsId)
        {
            if (departmentsId > 0)
            {
                try
                {
                    return await _commonDataService.RemoveDepartment(departmentsId);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }

        // ------------------ Designation Section Start ------------------

        [HttpGet]
        [Route("GetDesignationList")]
        public async Task<List<DesignationRequest>> GetDesignationList(long organizationId)
        {
            try
            {
                if (organizationId > 0)
                {
                    return await _commonDataService.GetDesignationList(organizationId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new List<DesignationRequest>();
        }

        [HttpPost]
        [Route("AddDesignation")]
        public async Task<DesignationRequest> AddDesignation([FromBody] DesignationRequest request)
        {
            try
            {
                if (request?.OrganizationId > 0
                && !string.IsNullOrEmpty(request.DesignationName))
                {
                    return await _commonDataService.AddDesignation(request);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("UpdateDesignation")]
        public async Task<DesignationRequest> UpdateDesignation([FromBody] DesignationRequest request)
        {
            try
            {
                if (request?.DesignationId > 0
                && request.OrganizationId > 0
                && !string.IsNullOrEmpty(request.DesignationName))
                {
                    return await _commonDataService.UpdateDesignation(request);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("RemoveDesignation")]
        public async Task<bool> RemoveDesignation(long designationId)
        {
            try
            {
                if (designationId > 0)
                {
                    return await _commonDataService.RemoveDesignation(designationId);
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
