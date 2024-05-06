using AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee;

namespace AMNSystemsERP.BL.Repositories.EmployeePayroll.EmployeeRepo
{
    public interface IEmployeeService
    {
        // -------------------------------------------------------------------------
        // ------------------ Employee Section Start -------------------------------
        // -------------------------------------------------------------------------
        Task<EmployeeBasicRequest> AddEmployee(EmployeeRequest request);
        Task<EmployeeBasicRequest> UpdateEmployee(EmployeeRequest request);
        Task<bool> RemoveEmployee(long employeeId);
        Task<EmployeeRequest> GetEmployeeById(long employeeId);
        Task<List<EmployeeBasicRequest>> GetEmployeeList(long organizationId, long outletId);

        // -------------------------------------------------------------------------
        // ------------------ Employee Document Section ----------------------------
        // -------------------------------------------------------------------------
        Task<List<EmployeeDocumentsRequest>> SaveEmployeeDocuments(List<EmployeeDocumentsRequest> request);
        Task<List<EmployeeDocumentsRequest>> GetEmployeeDocuments(EmployeeDocumentsRequest request);

        // -------------------------------------------------------------------------
        // ------------------ Employee Qualification Section -----------------------
        // -------------------------------------------------------------------------

        Task<EmployeeQualificationsRequest> SaveQualification(EmployeeQualificationsRequest request);
        Task<bool> RemoveQualification(long employeeQualificationsId);
        Task<EmployeeQualificationsRequest> GetQualificationById(long employeeQualificationsId);
        Task<List<EmployeeQualificationsRequest>> GetQualificationListByEmployeeId(long employeeId);
    }
}