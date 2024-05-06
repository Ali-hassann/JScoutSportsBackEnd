using AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Loans;
using Microsoft.AspNetCore.Mvc;
using AMNSystemsERP.BL.Repositories.EmployeePayroll.PayrollRepo;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Allowances;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;
using AMNSystemsERP.CL.Models.EmployeePayrollModels;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Wages;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/Payroll")]
    public class PayrollController : ApiController
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        // ------------------ EmployeeLoan Section Start ------------------
        [HttpPost]
        [Route("AddEmployeeLoan")]
        public async Task<EmployeeLoanRequest> AddEmployeeLoan([FromBody] EmployeeLoanRequest request)
        {
            if (request?.EmployeeId > 0
                && request.OutletId > 0
                && request.LoanTypeId > 0
                && request.LoanAmount > 0)
            {
                try
                {
                    return await _payrollService.AddEmployeeLoan(request);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateEmployeeLoan")]
        public async Task<EmployeeLoanRequest> UpdateEmployeeLoan([FromBody] EmployeeLoanRequest request)
        {
            try
            {
                if (request?.EmployeeLoanId > 0
                    && request.EmployeeId > 0
                    && request.OutletId > 0
                    && request.LoanTypeId > 0
                    && request.LoanAmount > 0)
                {
                    return await _payrollService.UpdateEmployeeLoan(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("EmployeeMultipleLoan")]
        public async Task<long> EmployeeMultipleLoan([FromBody] List<EmployeeLoanRequest> requestList)
        {
            try
            {
                if (requestList?.Count > 0)
                {
                    return await _payrollService.EmployeeMultipleLoan(requestList);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return -1;
        }

        [HttpPost]
        [Route("RemoveEmployeeLoan")]
        public async Task<bool> RemoveEmployeeLoan(long employeeLoanId)
        {
            try
            {
                if (employeeLoanId > 0)
                {
                    return await _payrollService.RemoveEmployeeLoan(employeeLoanId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetEmployeeLoan")]
        public async Task<List<EmployeeLoanRequest>> GetEmployeeLoan(long employeeId)
        {
            try
            {
                if (employeeId > 0)
                {
                    return await _payrollService.GetEmployeeLoan(employeeId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new List<EmployeeLoanRequest>();
        }

        [HttpGet]
        [Route("GetApprovalLoanList")]
        public async Task<List<EmployeeLoanRequest>> GetApprovalLoanList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _payrollService.GetApprovalLoanList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new List<EmployeeLoanRequest>();
        }

        [HttpPost]
        [Route("AddAllowance")]
        public async Task<EmployeeAllowancesRequest> AddAllowance([FromBody] EmployeeAllowancesRequest request)
        {
            try
            {
                if (request?.EmployeeId > 0
                    && request.AllowanceTypeId > 0
                    && request.Amount > 0)
                {
                    return await _payrollService.AddAllowance(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateAllowance")]
        public async Task<EmployeeAllowancesRequest> UpdateAllowance([FromBody] EmployeeAllowancesRequest request)
        {
            try
            {
                if (request?.EmployeeId > 0
                    && request.EmployeeAllowancesId > 0
                    && request.AllowanceTypeId > 0
                    && request.Amount > 0)
                {
                    return await _payrollService.UpdateAllowance(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveAllowance")]
        public async Task<bool> RemoveAllowance([FromBody] EmployeeAllowancesRequest request)
        {
            try
            {
                if (request.EmployeeAllowancesId > 0 && request.EmployeeId > 0)
                {
                    return await _payrollService.RemoveAllowance(request);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }

        [HttpGet]
        [Route("GetEmployeeAllowanceList")]
        public async Task<List<EmployeeAllowancesRequest>> GetEmployeeAllowanceList(long employeeId)
        {
            try
            {
                if (employeeId > 0)
                {
                    return await _payrollService.GetEmployeeAllowanceList(employeeId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new List<EmployeeAllowancesRequest>();
        }

        // ------------- Employee SalarySheet Calculation Section Start ---------------
        [HttpPost]
        [Route("GenerateSalarySheet")]
        public async Task<List<SalarySheet>> GenerateSalarySheet([FromBody] PayrollParameterRequest request)
        {
            try
            {
                if (request?.YearOf > 0 && request.MonthOf > 0)
                {
                    return await _payrollService.GenerateSalarySheet(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GenerateWages")]
        public async Task<List<WagesRequest>> GenerateWages([FromBody] PayrollParameterRequest request)
        {
            try
            {
                if (request?.OutletId > 0)
                {
                    return await _payrollService.GenerateWages(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetWagesApprovalList")]
        public async Task<List<WagesRequest>> GetWagesApprovalList([FromBody] PayrollParameterRequest request)
        {
            try
            {
                //if (request?.OutletId > 0)
                //{
                return await _payrollService.GetWagesApprovalList(request);
                //}
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("SaveSalarySheet")]
        public async Task<List<SalarySheet>> SaveSalarySheet([FromBody] List<SalarySheet> request, bool isToPost)
        {
            try
            {
                if (request?.Count > 0)
                {
                    return await _payrollService.SaveSalarySheet(request, isToPost);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("SaveWages")]
        public async Task<bool> SaveWages([FromBody] List<WagesRequest> request)
        {
            try
            {
                if (request?.Count > 0)
                {
                    return await _payrollService.SaveWages(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("PostWages")]
        public async Task<long> PostWages([FromBody] WagesRequest request)
        {
            try
            {
                if (request?.EmployeeId > 0 && request.OutletId > 0 && request.WagesAmount > 0)
                {
                    return await _payrollService.PostWages(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return -1;
        }
    }
}