using AMNSystemsERP.CL.Models.EmployeePayrollModels;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Allowances;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Loans;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Wages;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.BL.Repositories.EmployeePayroll.PayrollRepo
{
    public interface IPayrollService
    {
        // 1
        // -------------------------------------------------------------------------------------------------
        // ---------------- EmployeeLoan Section Start -----------------------------------------------------
        // -------------------------------------------------------------------------------------------------
        Task<EmployeeLoanRequest> AddEmployeeLoan(EmployeeLoanRequest request);
        Task<EmployeeLoanRequest> UpdateEmployeeLoan(EmployeeLoanRequest request);
        Task<long> EmployeeMultipleLoan(List<EmployeeLoanRequest> requestList);
        Task<bool> RemoveEmployeeLoan(long employeeLoanId);
        Task<List<EmployeeLoanRequest>> GetEmployeeLoan(long employeeId);
        Task<List<EmployeeLoanRequest>> GetApprovalLoanList(long outletId);

        // -------------------------------------------------------------------------
        // ------------------ Allowances Section Start -----------------------------
        // -------------------------------------------------------------------------
        Task<EmployeeAllowancesRequest> AddAllowance(EmployeeAllowancesRequest request);
        Task<EmployeeAllowancesRequest> UpdateAllowance(EmployeeAllowancesRequest request);
        Task<bool> RemoveAllowance(EmployeeAllowancesRequest request);
        Task<List<EmployeeAllowancesRequest>> GetEmployeeAllowanceList(long employeeId);

        // 5
        // -------------------------------------------------------------------------------------------------
        // ---------------- Employee SalarySheet Calculation Section Start ---------------------------------
        // -------------------------------------------------------------------------------------------------
        Task<List<SalarySheet>> GenerateSalarySheet(PayrollParameterRequest request);
        Task<List<WagesRequest>> GenerateWages(PayrollParameterRequest request);
        Task<List<WagesRequest>> GetWagesApprovalList(PayrollParameterRequest request);
        Task<bool> SaveWages(List<WagesRequest> request);
        Task<long> PostWages(WagesRequest request);
        Task<List<SalarySheet>> SaveSalarySheet(List<SalarySheet> request, bool isToPosted);
    }
}