using AMNSystemsERP.CL.Models.EmployeePayrollModels.Base;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Allowances;

namespace AMNSystemsERP.BL.Repositories.EmployeePayroll.CommonDataRepo
{
    public interface ICommonDataService
    {
        // 3
        // --------------------------------------------------------------
        // ----------------- Allowance Type Section Start ---------------
        // --------------------------------------------------------------        
        Task<List<AllowanceTypeRequest>> GetAllowanceTypeList(long organizationId, long outletId);
        Task<AllowanceTypeRequest> AddAllowanceType(AllowanceTypeRequest request);
        Task<AllowanceTypeRequest> UpdateAllowanceType(AllowanceTypeRequest request);
        Task<bool> RemoveAllowanceType(long allowanceTypeId);

        Task<List<DepartmentsRequest>> GetDepartmentsList(long organizationId);
        Task<DepartmentsRequest> AddDepartment(DepartmentsRequest request);
        Task<DepartmentsRequest> UpdateDepartment(DepartmentsRequest request);
        Task<bool> RemoveDepartment(long departmentsId);
        // 5
        // --------------------------------------------------------------
        // ------------------ Designation Section Start -----------------
        // --------------------------------------------------------------
        Task<List<DesignationRequest>> GetDesignationList(long organizationId);
        Task<DesignationRequest> AddDesignation(DesignationRequest request);
        Task<DesignationRequest> UpdateDesignation(DesignationRequest request);
        Task<bool> RemoveDesignation(long designationId);
    }
}
