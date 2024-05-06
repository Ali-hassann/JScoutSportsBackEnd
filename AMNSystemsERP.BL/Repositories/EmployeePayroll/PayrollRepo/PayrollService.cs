using AutoMapper;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;
using System.Data;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Loans;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Allowances;
using AMNSystemsERP.CL.Models.EmployeePayrollModels;
using AMNSystemsERP.DL.DB.DBSets.Accounts;
using AMNSystemsERP.CL.Enums.AccountEnums;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Enums;
using AMNSystemsERP.CL.Enums.PayrollEnums;
using System.Globalization;
using AMNSystemsERP.BL.Repositories.ChartOfAccounts;
using System.Linq;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Wages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AMNSystemsERP.CL.Models.AccountModels.Vouchers;
using AMNSystemsERP.BL.Repositories.Vouchers;

namespace AMNSystemsERP.BL.Repositories.EmployeePayroll.PayrollRepo
{
    public class PayrollService : IPayrollService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        // private readonly IVoucherService _voucherService;
        private readonly IChartOfAccountsService _chartOfAccountsService;
        private readonly IVoucherService _voucherService;

        public PayrollService(IUnitOfWork unit,
        IMapper mapper,
        IChartOfAccountsService chartOfAccountsService,
        IVoucherService voucherService)
        {
            _unit = unit;
            _mapper = mapper;
            _voucherService = voucherService;
            _chartOfAccountsService = chartOfAccountsService;

        }

        // ------------------ EmployeeLoan Section Start ------------------
        public async Task<EmployeeLoanRequest> AddEmployeeLoan(EmployeeLoanRequest request)
        {
            try
            {
                // Adding EmployeeLoan
                var employeeLoan = _mapper.Map<EmployeeLoan>(request);

                _unit.EmployeeLoanRepository.InsertSingle(employeeLoan);
                //

                if (await _unit.SaveAsync())
                {
                    request.EmployeeLoanId = employeeLoan.EmployeeLoanId;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        private async Task<EmployeeLoanRequest> EmployeeLoanWithVoucher(EmployeeLoanRequest request)
        {
            try
            {
                var voucherToAdd = new VouchersMasterRequest();
                var configurationList = await _unit
                                              .ConfigurationSettingRepository
                                              .GetAsync(c => c.OutletId == request.OutletId);

                var loanTypeName = (request.LoanTypeId == LoanType.Loan ? "Loan" : "Advance");

                var postingAccountAlreadyExist = await _unit
                                                       .PostingAccountsRepository
                                                       .GetSingleAsync(p => p.EmployeeId == request.EmployeeId);

                var postingAccountId = postingAccountAlreadyExist?.PostingAccountsId ?? 0;
                if (request.LoanTypeId == LoanType.Loan)
                {
                    if (postingAccountId > 0)
                    {
                        var cashAccountId = configurationList?.FirstOrDefault(s => s.AccountName == "CashAccount")?.AccountValue ?? 0;
                        if (cashAccountId > 0)
                        {
                            // voucher
                            voucherToAdd.VoucherTypeId = VoucherType.CashPayment;
                            voucherToAdd.OutletId = request.OutletId;
                            voucherToAdd.Remarks = @$"{loanTypeName} of {request.EmployeeName}";
                            voucherToAdd.VoucherDate = request.LoanDate;
                            voucherToAdd.ReferenceNo = @$"{loanTypeName} Voucher";
                            voucherToAdd.TotalAmount = request.LoanAmount;
                            voucherToAdd.VoucherStatus = (int)VoucherStatus.Posted;
                            voucherToAdd.TransactionType = VoucherTransactionType.Cash.ToString();

                            // voucher detail
                            voucherToAdd.VouchersDetailRequest = new List<VouchersDetailRequest>()
                            {
                                new VouchersDetailRequest()
                                {
                                    CreditAmount = request.LoanAmount,
                                    DebitAmount = 0,
                                    PostingAccountsId = cashAccountId,
                                    Narration = @$"{loanTypeName} of {request.EmployeeName}"
                                },
                                new VouchersDetailRequest()
                                {
                                    CreditAmount = 0,
                                    DebitAmount = request.LoanAmount,
                                    PostingAccountsId = postingAccountId,
                                    Narration = @$"{loanTypeName} of {request.EmployeeName}"
                                }
                            };
                            var response = await _voucherService.AddVoucher(voucherToAdd);
                            voucherToAdd.VouchersMasterId = response.VouchersMasterId;
                        }
                        //
                    }
                }
                else if (request.LoanTypeId == LoanType.Advance)
                {
                    var cashAccountId = configurationList?.FirstOrDefault(s => s.AccountName == "CashAccount")?.AccountValue ?? 0;
                    if (cashAccountId > 0 && postingAccountId > 0)
                    {
                        // voucher
                        voucherToAdd.VoucherTypeId = VoucherType.CashPayment;
                        voucherToAdd.OutletId = request.OutletId;
                        voucherToAdd.Remarks = @$"{loanTypeName} of {request.EmployeeName}";
                        voucherToAdd.VoucherDate = request.LoanDate;
                        voucherToAdd.ReferenceNo = @$"{loanTypeName} Voucher";
                        voucherToAdd.TotalAmount = request.LoanAmount;
                        voucherToAdd.VoucherStatus = (int)VoucherStatus.Posted;
                        voucherToAdd.TransactionType = VoucherTransactionType.Cash.ToString();

                        // voucher detail
                        voucherToAdd.VouchersDetailRequest = new List<VouchersDetailRequest>()
                            {
                                new VouchersDetailRequest()
                                {
                                    DebitAmount = request.LoanAmount,
                                    CreditAmount = 0,
                                    PostingAccountsId = postingAccountId,
                                    Narration = @$"{loanTypeName} of {request.EmployeeName}"
                                },
                                new VouchersDetailRequest()
                                {
                                    DebitAmount = 0,
                                    CreditAmount = request.LoanAmount,
                                    PostingAccountsId = cashAccountId,
                                    Narration = @$"{loanTypeName} of {request.EmployeeName}"
                                }
                            };
                        var response = await _voucherService.AddVoucher(voucherToAdd);
                        voucherToAdd.VouchersMasterId = response.VouchersMasterId;
                    }
                    //
                }

                // Adding EmployeeLoan
                var employeeLoan = _mapper.Map<EmployeeLoan>(request);

                _unit.EmployeeLoanRepository.Update(employeeLoan);
                //

                if (await _unit.SaveAsync())
                {
                    request.VoucherMasterId = voucherToAdd.VouchersMasterId;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }


        public async Task<EmployeeLoanRequest> UpdateEmployeeLoan(EmployeeLoanRequest request)
        {
            try
            {
                return request.IsToCreateVoucher ?
                                await EmployeeLoanWithVoucher(request)
                                : await EmployeeLoanWithOpeningBalance(request);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<long> EmployeeMultipleLoan(List<EmployeeLoanRequest> requestList)
        {
            try
            {
                var voucherToAdd = new VouchersMasterRequest();
                voucherToAdd.VoucherTypeId = VoucherType.CashPayment;
                voucherToAdd.OutletId = requestList.FirstOrDefault().OutletId;
                voucherToAdd.Remarks = @$"Loan / Advance Voucher";
                voucherToAdd.VoucherDate = DateTime.Now.Date;
                voucherToAdd.ReferenceNo = string.Empty;
                voucherToAdd.TotalAmount = requestList.Sum(r => r.LoanAmount);
                voucherToAdd.VoucherStatus = (int)VoucherStatus.Posted;
                voucherToAdd.TransactionType = VoucherTransactionType.Cash.ToString();

                var configurationList = await _unit
                                              .ConfigurationSettingRepository
                                              .GetAsync();
                var cashAccountId = configurationList?.FirstOrDefault(s => s.AccountName == "CashAccount")?.AccountValue ?? 0;

                var postingAccountAlreadyExist = (await _unit
                                                       .PostingAccountsRepository
                                                       .GetAsync()).ToList();
                if (cashAccountId > 0)
                {
                    requestList.ForEach(request =>
                    {
                        var postingAccountId = postingAccountAlreadyExist.FirstOrDefault(r => r.EmployeeId == request.EmployeeId)?.PostingAccountsId ?? 0;
                        if (postingAccountId > 0)
                        {
                            if (request.LoanTypeId == LoanType.Loan)
                            {
                                if (postingAccountId > 0)
                                {
                                    // voucher detail
                                    voucherToAdd.VouchersDetailRequest.Add(
                                        new VouchersDetailRequest()
                                        {
                                            CreditAmount = 0,
                                            DebitAmount = request.LoanAmount,
                                            PostingAccountsId = postingAccountId,
                                            Narration = @$"{request.Remarks}"
                                        });
                                    //
                                }
                            }
                            else if (request.LoanTypeId == LoanType.Advance)
                            {
                                // voucher detail
                                voucherToAdd.VouchersDetailRequest.Add(
                                    new VouchersDetailRequest()
                                    {
                                        DebitAmount = request.LoanAmount,
                                        CreditAmount = 0,
                                        PostingAccountsId = postingAccountId,
                                        Narration = @$"{request.Remarks}"
                                    });
                                //
                            }
                        }

                        // Adding EmployeeLoan
                        var employeeLoan = _mapper.Map<EmployeeLoan>(request);

                        _unit.EmployeeLoanRepository.Update(employeeLoan);
                        //
                    });

                    voucherToAdd.VouchersDetailRequest.Add(
                                        new VouchersDetailRequest()
                                        {
                                            CreditAmount = requestList.Sum(r => r.LoanAmount),
                                            DebitAmount = 0,
                                            PostingAccountsId = cashAccountId,
                                            Narration = @$""
                                        });

                    return (await _voucherService.AddVoucher(voucherToAdd)).VouchersMasterId;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return 0;
        }

        private async Task<EmployeeLoanRequest> EmployeeLoanWithOpeningBalance(EmployeeLoanRequest request)
        {
            try
            {
                if (request.LoanTypeId == LoanType.Loan)
                {
                    var configurationList = await _unit
                                              .ConfigurationSettingRepository
                                              .GetAsync(c => c.OutletId == request.OutletId);

                    var postingAccountAlreadyExist = await _unit
                                                           .PostingAccountsRepository
                                                           .GetSingleAsync(p => p.EmployeeId == request.EmployeeId);

                    var postingAccountId = postingAccountAlreadyExist?.PostingAccountsId ?? 0;
                    if (postingAccountAlreadyExist == null || postingAccountAlreadyExist?.EmployeeId == 0)
                    {
                        var debtorsId = configurationList?.FirstOrDefault(s => s.AccountName == "Debtors")?.AccountValue ?? 0;
                        if (debtorsId > 0)
                        {
                            var postingAccountToAdd = new PostingAccounts()
                            {
                                OutletId = request.OutletId,
                                EmployeeId = request.EmployeeId,
                                IsActive = true,
                                Name = request.EmployeeName,
                                SubCategoriesId = debtorsId,
                                OpeningDate = DateHelper.GetCurrentDate(),
                                OpeningCredit = 0,
                                OpeningDebit = request.LoanAmount
                            };
                            _unit.PostingAccountsRepository.InsertSingle(postingAccountToAdd);

                            if (await _unit.SaveAsync())
                            {
                                postingAccountId = postingAccountToAdd.PostingAccountsId;
                            }
                        }
                    }
                    else
                    {
                        postingAccountAlreadyExist.OpeningDebit += request.LoanAmount;
                        _unit.PostingAccountsRepository.Update(postingAccountAlreadyExist);
                    }
                }

                // Adding EmployeeLoan
                var employeeLoan = _mapper.Map<EmployeeLoan>(request);

                _unit.EmployeeLoanRepository.Update(employeeLoan);
                //

                if (await _unit.SaveAsync())
                {
                    request.VoucherMasterId = 0;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<bool> RemoveEmployeeLoan(long employeeLoanId)
        {
            try
            {
                var query = @$"DELETE FROM EmployeeLoan WHERE EmployeeLoanId = {employeeLoanId}";
                return await _unit.DapperRepository.ExecuteNonQuery(query, null, CommandType.Text) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmployeeLoanRequest>> GetEmployeeLoan(long employeeId)
        {
            try
            {
                var query = $@"SELECT                                                         
                                EmployeeLoanId                
                                , CAST(LoanAmount AS DECIMAL(18, 2)) AS LoanAmount                            
                                , CAST(Installment AS DECIMAL(18, 2)) AS Installment              
                                , LoanDate                
                                , DeductAmount               
                                , LoanTypeId              
                                , EmployeeId                
                                , OutletId
                                , Remarks
                                , IsApproved
                                , IsAdvanceDeducted
	                            , SUM(ISNULL(LoanAmount-DeductAmount,0)) OVER(partition by EmployeeId    
                                    ORDER BY EmployeeId, LoanDate,EmployeeLoanId) AS Balance        
                            FROM EmployeeLoan
                            where EmployeeId ={employeeId}
                            ORDER BY EmployeeLoanId";

                return await _unit.DapperRepository.GetListQueryAsync<EmployeeLoanRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmployeeLoanRequest>> GetApprovalLoanList(long outletId)
        {
            try
            {
                var query = $@"SELECT                                                         
                                E.EmployeeName                
                                , E.FatherName                
                                , E.DepartmentsName
                                , E.DesignationName                
                                , EmployeeLoanId                
                                , CAST(LoanAmount AS DECIMAL(18, 2)) AS LoanAmount                                    
                                , CAST(Installment AS DECIMAL(18, 2)) AS Installment              
                                , LoanDate                
                                , DeductAmount               
                                , LoanTypeId              
                                , CASE WHEN LoanTypeId = 1 THEN 'Loan' ELSE 'Advance' END AS TypeName
                                , EL.EmployeeId                
                                , EL.OutletId
                                , Remarks        
                                , IsApproved        
                                , IsAdvanceDeducted        
                            FROM EmployeeLoan AS EL
							INNER JOIN V_EMPLOYEE AS E
								ON E.EmployeeId = EL.EmployeeId
                            where EL.OutletId = {outletId}
							AND EL.IsApproved = 0
							AND EL.DeductAmount = 0
                            ORDER BY EmployeeLoanId";

                return await _unit.DapperRepository.GetListQueryAsync<EmployeeLoanRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ---------------- Employee Benefits (Allowance) Section Start ------------------------

        public async Task<EmployeeAllowancesRequest> AddAllowance(EmployeeAllowancesRequest request)
        {
            try
            {
                var allowance = _mapper.Map<EmployeeAllowances>(request);
                _unit.EmployeeAllowancesRepository.InsertSingle(allowance);

                if (await _unit.SaveAsync())
                {
                    request.EmployeeAllowancesId = allowance.EmployeeAllowancesId;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<EmployeeAllowancesRequest> UpdateAllowance(EmployeeAllowancesRequest request)
        {
            try
            {
                var allowance = _mapper.Map<EmployeeAllowances>(request);
                _unit.EmployeeAllowancesRepository.Update(allowance);

                if (await _unit.SaveAsync())
                {
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<bool> RemoveAllowance(EmployeeAllowancesRequest request)
        {
            try
            {
                var currentDate = DateHelper.GetCurrentDate();
                _unit.EmployeeAllowancesRepository.DeleteById(request.EmployeeAllowancesId);

                var salarySheet = await _unit
                                        .SalarySheetRepository
                                        .GetSingleAsync(d => d.EmployeeId == request.EmployeeId
                                                             && d.SalaryDate.Month == (currentDate.Month - 1)
                                                             && d.IsPosted == false
                                                             && d.SalaryDate.Year == currentDate.Year);

                if (salarySheet?.Allowance > 0)
                {
                    salarySheet.Allowance -= request.Amount;
                    salarySheet.Allowance = salarySheet.Allowance < 0 ? 0 : salarySheet.Allowance;

                    _unit.SalarySheetRepository.Update(salarySheet);
                }

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmployeeAllowancesRequest>> GetEmployeeAllowanceList(long employeeId)
        {
            try
            {
                var query = $@"SELECT                                   
                                     EA.EmployeeAllowancesId                                  
                                     , CAST(EA.Amount AS DECIMAL(18, 2)) AS Amount                            
                                     , EA.AllowanceTypeId                                   
                                     , AT.Name                              
                                     , EA.EmployeeId   
                                    FROM EmployeeAllowances AS EA                                  
                                    INNER JOIN AllowanceType AS AT                      
                                     ON AT.AllowanceTypeId = EA.AllowanceTypeId                                  
                                    WHERE EA.EmployeeId = {employeeId}";
                return await _unit.DapperRepository.GetListQueryAsync<EmployeeAllowancesRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ------------- Employee Payroll Calculation Section Start ---------------

        public async Task<List<SalarySheet>> GenerateSalarySheet(PayrollParameterRequest request)
        {
            try
            {
                var salarySheetDate = new DateTime(request.YearOf, request.MonthOf, 28);

                var query1 = $@"
                                             DECLARE @TotalMonthDays int =0;
                                             DECLARE @TotalSundays int =0;
                                             DECLARE @DepartmentTypeId int = {request.DepartmentTypeId};
                                             DECLARE @TotalDaysWithoutSunday int =0;
                                             DECLARE @Month int = {request.MonthOf};
                                             DECLARE @Year int = {request.YearOf};
                                             SELECT @TotalMonthDays=TotalMonthDays
		                                    , @TotalSundays=TotalSundays
		                                    , @TotalDaysWithoutSunday= TotalDaysWithoutSunday
		                                    FROM dbo.F_DaysOfMonth(@Month,@Year);
                                with cte as
					                (
					                SELECT e.EmployeeId
                                        , '{salarySheetDate}' as SalaryDate
						                , EmployeeName
                                        , deg.DesignationName
                                        , e.DepartmentsId
				                        , dept.Name as DepartmentsName
                                        , dept.DepartmentTypeId
				                        , ISNULL(E.SalaryAmount,0) AS SalaryAmount
				                        , ISNULL(att.TotalWorkingHoursCount,0) as TotalWorkingHoursCount
                                        , ISNULL(att.LateHours,0) as TotalLateHours
				                        , ISNULL(overTime.TotalOverTimeHours,0) as TotalOverTimeHours
				                        , ISNULL(tblAllowance.Allowance,0) as Allowance
				                        , tblAllowance.AllowanceDetail
                                        , ISNULL(E.SalaryAmount/@TotalMonthDays ,0) as PerDayRate
				                        , ISNULL((E.SalaryAmount/@TotalMonthDays/8) ,0) as PerHourRate
                                        , ISNULL((E.SalaryAmount/@TotalMonthDays/6) ,0) as OverTimeRate
				                        , ISNULL(salary.Installment,ISNULL(tblInstallment.Installment,0)) as Installment
				                        , ISNULL(tblAdvance.Advance,0) as Advance
				                        , ISNULL(salary.IncomeTax,0) as IncomeTax
				                        , ISNULL(salary.OthersDeduction,0) as OthersDeduction
										, ISNULL(OthersAddition,0) AS OthersAddition
										, ISNULL(AB,0) AS TotalAbsents
										, ISNULL(P,0) AS TotalPresents
										, ISNULL(LP,0) AS TotalPaidLeaves
	                        			, 'P:'+cast(P as varchar(5))+', LP:'+cast(LP as varchar(5))+', LUP:'+CAST(LUP as varchar)+', AB:'+cast(AB as varchar)+', HL:'+cast(HL as varchar) as AttendanceSummary
					                    , Case WHEN ISNULL(AB,0) > 0 AND (P>10 OR LP>10) THEN ISNULL(E.SalaryAmount/@TotalMonthDays ,0) ELSE 0 END AS AbsentAmount
                                        , Case WHEN (ISNULL(AB,0)-1) > 0 THEN CASE WHEN P<10 THEN ((AB)*ISNULL(E.SalaryAmount/@TotalMonthDays ,0)) ELSE ((AB-1)*ISNULL(E.SalaryAmount/@TotalMonthDays ,0) ) END ELSE 0 END + (ISNULL(att.SundaysNotPay,0) * ISNULL(E.SalaryAmount/@TotalMonthDays ,0)) AS AbsentDeductedAmount
										, @TotalSundays - ISNULL(att.SundaysNotPay,0) AS PaidSundays
										, att.SundaysNotPay
										, e.OutletId
                                        , e.JoiningDate
										, e.ContactNumber
										, e.ImagePath
										, e.Gender
                                        , e.FatherName
                                        , e.IsActive
                                        , e.SalaryType
                                        , BankAccountId
										, Balance
										, ISNULL(salary.SalarySheetId,0) AS SalarySheetId
										, salary.AbsentAmount AS SAbsentAmount
										, salary.AbsentDeductedAmount AS SAbsentDeductedAmount
										, salary.Advance AS SAdvance
										, salary.Allowance AS SAllowance
										, salary.AllowanceDetail AS SAllowanceDetail
										, salary.AttendanceSummary AS SAttendanceSummary
										, salary.GrossPay AS SGrossPay
										, salary.NetPay AS SNetPay
										, salary.OvertimeAmount AS SOvertimeAmount
										, salary.OverTimeRate AS SOverTimeRate
										, salary.PerDayRate AS SPerDayRate
										, salary.PerHourRate AS SPerHourRate
										, salary.SalaryAmount AS SSalaryAmount
										, salary.SundaysAmount AS SSundaysAmount
										, salary.SundaysNotPay AS SSundaysNotPay
										, salary.TotalAbsents AS STotalAbsents
										, salary.TotalDays AS STotalDays
										, salary.TotalHours AS STotalHours
										, salary.TotalLateHours AS STotalLateHours
										, salary.TotalOverTimeHours AS STotalOverTimeHours
										, salary.TotalPaidLeaves AS STotalPaidLeaves
										, salary.TotalPresents AS STotalPresents
										, salary.TotalSundays AS STotalSundays
										, salary.TotalWorkAmount AS STotalWorkAmount
										, salary.TotalWorkingHoursCount AS STotalWorkingHoursCount
										, salary.WorkingDays AS SWorkingDays
										, ISNULL(salary.IsPosted,0) AS IsPosted
										, salary.Remarks
						                 FROM Employee e
						                 LEFT JOIN
						                 (
							                -- Calculation of Attendance
											   SELECT tb1.EmployeeId
													, TotalWorkingHoursCount
                                                    , LateHours
													, typ.P,typ.LP,typ.LUP,typ.AB,typ.HL,SundaysNotPay
													   FROM
															(
															SELECT 
																EmployeeId
																, SUM(ISNULL(TotalWorkingHoursCount,0)) as TotalWorkingHoursCount
																, SUM(ISNULL(LateHours,0)) AS LateHours
																FROM
																(
																	SELECT E.EmployeeId
																	, CASE ISNULL(mast.AttendanceStatusId,CASE WHEN E.JoiningDate<C.CalendarDate THEN 4 ELSE 1 END) WHEN 1 THEN --Present
																			 	SUM(CAST(CASE WHEN DATEDIFF(minute,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' AND @DepartmentTypeId = 1 AND DATEPART(dw, mast.AttendanceDate) <> 1 then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0 >= 9 THEN 8 
																				WHEN DATEDIFF(MINUTE,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' AND @DepartmentTypeId = 1 AND DATEPART(dw, mast.AttendanceDate) <> 1 then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0 >= 5 THEN DATEDIFF(MINUTE,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' AND @DepartmentTypeId = 1 then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0 - 1
																				ELSE DATEDIFF(MINUTE,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' AND @DepartmentTypeId = 1 AND DATEPART(dw, mast.AttendanceDate) <> 1 then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0 END AS DECIMAL(18,2))) 
																				WHEN 2 THEN SUM(CASE WHEN DATEPART(dw, mast.AttendanceDate) <> 1 THEN 1 ELSE 0 END)* 8  --Leave Paid
																				WHEN 3 THEN 0	--Leave UNPaid																	
																				WHEN 4 THEN 0   --Absent
																				WHEN 5 THEN     -- Half Leave Paid
																				SUM(CAST(CASE WHEN DATEDIFF(minute,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' AND @DepartmentTypeId = 1 AND DATEPART(dw, mast.AttendanceDate) <> 1 then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0>8 THEN 8 
																				ELSE DATEDIFF(minute,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' AND @DepartmentTypeId = 1 AND DATEPART(dw, mast.AttendanceDate) <> 1 then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0 END AS DECIMAL(18,2))) 
																				ELSE 0
																				END	AS TotalWorkingHoursCount
																	 , CASE mast.AttendanceStatusId WHEN 1 THEN --Present
																				SUM(CAST(CASE WHEN DATEDIFF(minute,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' AND @DepartmentTypeId = 1 AND DATEPART(dw, mast.AttendanceDate) <> 1 then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0 >= 9 THEN 0 
																				WHEN DATEDIFF(MINUTE,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' AND @DepartmentTypeId = 1 AND DATEPART(dw, mast.AttendanceDate) <> 1 then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0 >= 5 THEN (9 - DATEDIFF(MINUTE,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0)
																				ELSE (8-DATEDIFF(MINUTE,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' AND @DepartmentTypeId = 1 AND DATEPART(dw, mast.AttendanceDate) <> 1 then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0) END AS DECIMAL(18,2))) 
																				WHEN 5 THEN     -- Half Leave Paid
																				SUM(CAST(CASE WHEN DATEDIFF(minute,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' AND @DepartmentTypeId = 1 AND DATEPART(dw, mast.AttendanceDate) <> 1 then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0 >= 9 THEN 0 
																				WHEN DATEDIFF(MINUTE,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' AND @DepartmentTypeId = 1 AND DATEPART(dw, mast.AttendanceDate) <> 1 then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0 >= 5 THEN (9 - DATEDIFF(MINUTE,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0)
																				ELSE (8 - DATEDIFF(MINUTE,CheckIn,CAse When ISNULL(CheckOut,CheckIn) > '17:30:00.0000000' AND @DepartmentTypeId = 1 AND DATEPART(dw, mast.AttendanceDate) <> 1 then '17:30:00.0000000' else ISNULL(CheckOut,CheckIn) end)/60.0) END AS DECIMAL(18,2)))
																				ELSE 0
																				END	AS LateHours
															FROM GenerateMonthCalendar(@Month,@Year) AS C
															CROSS JOIN Employee AS E
															LEFT JOIN Attendance mast
																ON CAST(mast.AttendanceDate AS DATE) = C.CalendarDate
																AND MONTH(AttendanceDate)= @Month
																AND YEAR(AttendanceDate)= @Year
																AND E.EmployeeId = mast.EmployeeId
															LEFT JOIN AttendanceDetail det ON mast.AttendanceId=det.AttendanceId
															WHERE ISNULL(E.IsDeleted,0) = 0
															GROUP BY E.EmployeeId,AttendanceStatusId,E.JoiningDate,C.CalendarDate
															) as tb
															GROUP BY tb.EmployeeId
						
														) as tb1
														INNER JOIN
														(
															SELECT EmployeeId
																, SUM(P) as P,SUM(LP) as LP,SUM(LUP) as LUP,SUM(AB) as AB,SUM(HL) as HL, SUM(SundaysNotPay) AS SundaysNotPay
																FROM
																(
																	SELECT E.EmployeeId
																	, SUM(CASE WHEN DATEPART(dw, C.CalendarDate) <> 1 AND ISNULL(mast.AttendanceStatusId,4) = 1 THEN 1 ELse 0 END) as P
																	, SUM(CASE WHEN DATEPART(dw, C.CalendarDate) <> 1 AND ISNULL(mast.AttendanceStatusId,4) = 2 THEN 1 ELse 0 END) as LP
																	, SUM(CASE WHEN DATEPART(dw, C.CalendarDate) <> 1 AND ISNULL(mast.AttendanceStatusId,4) = 3 THEN 1 ELse 0 END) as LUP
																	, SUM(CASE WHEN DATEPART(dw, C.CalendarDate) <> 1 AND ISNULL(mast.AttendanceStatusId,4) = 4 THEN 1 ELse 0 END) as AB
																	, SUM(CASE WHEN DATEPART(dw, C.CalendarDate) = 1 AND (ISNULL(mast.AttendanceStatusId,1) = 4 OR E.JoiningDate>C.CalendarDate) THEN 1 ELSE 0 END) AS SundaysNotPay
																	, SUM(CASE WHEN DATEPART(dw, C.CalendarDate) <> 1 AND ISNULL(mast.AttendanceStatusId,4) = 5 THEN 1 ELse 0 END) as HL
																	FROM GenerateMonthCalendar(@Month,@Year) AS C
																	CROSS JOIN Employee AS E
																	LEFT JOIN Attendance mast
																		ON CAST(mast.AttendanceDate AS DATE) = C.CalendarDate
																		AND MONTH(AttendanceDate)= @Month
																		AND YEAR(AttendanceDate)= @Year
																		AND E.EmployeeId = mast.EmployeeId
																	WHERE ISNULL(E.IsDeleted,0) = 0
																	GROUP BY E.EmployeeId,AttendanceStatusId
																) as attType
																GROUP BY EmployeeId
														) as typ
														ON tb1.EmployeeId=typ.EmployeeId
						                    ) att
						                    ON e.EmployeeId=att.EmployeeId

						                 LEFT JOIN
						                 (
							                -- Calculation of Total OverTime
							                SELECT EmployeeId
								                   , SUM(CAST(DATEDIFF(minute,CAse When CheckIn > '17:30:00.0000000' then '17:31:00.0000000' else CheckIn end,ISNULL(CheckOut,CheckIn))/60.0 as decimal(18,2))) as TotalOverTimeHours
									                FROM Overtime ot
									                INNER JOIN OvertimeDetail otd ON ot.OvertimeId=otd.OvertimeId
									                WHERE MONTH(OvertimeDate)= @Month
									                AND YEAR(OvertimeDate)= @Year
									                GROUP BY EmployeeId
						                ) overTime
						                ON e.EmployeeId=overTime.EmployeeId

						                LEFT JOIN
						                (
							                --  Loan Installement
								                 SELECT EmployeeId
		                                            , CASE WHEN  ISNULL(MAX(Balance),0)>SUM(Installment) THEN SUM(Installment) ELSE ISNULL(MAX(Balance),0) END as Installment
													, MAX(Balance) AS Balance
		                                            FROM
		                                            (
		                                            SELECT  emp.EmployeeId
				                                            , Installment
				                                            , ext.Balance
					                                            FROM EmployeeLoan as emp
						                                            INNER JOIN
				                                            (
					                                            SELECT EmployeeId
					                                            , SUM(LoanAmount-DeductAmount) as Balance 
					                                            FROM EmployeeLoan
                                                               WHERE IsApproved=1
					                                            GROUP BY EmployeeId
					                                            HAVING SUM(LoanAmount-DeductAmount)>0
				                                            ) ext
				                                            ON emp.EmployeeId=ext.EmployeeId
			                                            WHERE LoanTypeId=1
			                                            AND Installment>0
		                                            )as tbl
		                                            GROUP BY EmployeeId
					                       )as tblInstallment
								                ON e.EmployeeId=tblInstallment.EmployeeId
						                LEFT JOIN
						                (
							                -- Advances
							                SELECT EmployeeId
								                  , SUM(LoanAmount) as Advance
								                  FROM EmployeeLoan
								                  WHERE IsApproved=1
								                  AND LoanTypeId=2	
								                  AND LoanAmount>0								                  
                                                  AND IsAdvanceDeducted=0
												  GROUP BY EmployeeId
						                )as tblAdvance
								                ON e.EmployeeId=tblAdvance.EmployeeId
						                LEFT JOIN
						                (
							                -- Allowances
							                  SELECT EmployeeId
									                , SUM(Amount) as Allowance
									                , STRING_AGG (Concat(typ.Name+'= ',ROUND(CAST(Amount as decimal),0)),',') 
										                WITHIN GROUP (order by name) AllowanceDetail 
									                FROM EmployeeAllowances al
									                INNER JOIN AllowanceType typ
									                ON al.AllowanceTypeId=typ.AllowanceTypeId 
									                GROUP BY EmployeeId
						                )as tblAllowance
								                ON e.EmployeeId=tblAllowance.EmployeeId
                                                INNER JOIN Departments dept 
                                                    ON e.DepartmentsId=dept.DepartmentsId
												INNER JOIN Designation deg ON e.DesignationId=deg.DesignationId
                                         LEFT JOIN
				                              (
					                            -- Salary Sheet
					                            SELECT * FROM SalarySheet
							                    WHERE MONTH(SalaryDate)= @Month
							                    AND YEAR(SalaryDate)= @Year
				                                )as salary
						                                ON e.EmployeeId=salary.EmployeeId
				                       )
                                            SELECT EmployeeId
						                      , SalaryDate
						                        , EmployeeName
                                                , DepartmentsId
                                                , DepartmentsName
                                                , DepartmentTypeId
						                        , DesignationName
												, CASE WHEN IsPosted = 1 THEN SAttendanceSummary
													ELSE AttendanceSummary END AS AttendanceSummary
												, CASE WHEN IsPosted = 1 THEN SSalaryAmount
													ELSE SalaryAmount END AS SalaryAmount
												, CASE WHEN IsPosted = 1 THEN STotalSundays
													ELSE PaidSundays 
													END AS TotalSundays
												, CASE WHEN IsPosted = 1 THEN SSundaysAmount 
													ELSE 
														CASE WHEN @DepartmentTypeId =3 OR SalaryType = 3 THEN 0 
														ELSE
														CASE WHEN TotalPresents >6 OR TotalPaidLeaves>6 THEN
															CAST(ROUND((PerDayRate*PaidSundays),1) AS decimal(18,1)) ELSE 0
														END 
													END 
												  END as SundaysAmount
				    	                        , CASE WHEN IsPosted = 1 THEN SWorkingDays
													ELSE CONCAT ((cast(TotalWorkingHoursCount - (TotalWorkingHoursCount % 8) as int) / 8) , ' days, ', TotalWorkingHoursCount % 8, ' hours') 
													END as WorkingDays
												, CASE WHEN IsPosted = 1 THEN STotalDays
													ELSE CAST((TotalWorkingHoursCount - (TotalWorkingHoursCount % 8)) / 8 as INT) END AS TotalDays
												, CASE WHEN IsPosted = 1 THEN STotalHours
													ELSE (TotalWorkingHoursCount % 8) END AS TotalHours
												, CASE WHEN IsPosted = 1 THEN STotalWorkingHoursCount
													ELSE TotalWorkingHoursCount END AS TotalWorkingHoursCount
												, CASE WHEN IsPosted = 1 THEN STotalLateHours
													ELSE TotalLateHours END AS TotalLateHours
												, CASE WHEN IsPosted = 1 THEN SPerDayRate
													ELSE PerDayRate END AS PerDayRate
												, CASE WHEN IsPosted = 1 THEN SPerHourRate
													ELSE PerHourRate END AS PerHourRate
												, CASE WHEN IsPosted = 1 THEN STotalOverTimeHours
													ELSE TotalOverTimeHours END AS TotalOverTimeHours
												, CASE WHEN IsPosted = 1 THEN SOverTimeRate
													ELSE OverTimeRate 
													END AS OverTimeRate
												, CASE WHEN IsPosted = 1 THEN SAllowance
													ELSE CAST(Allowance AS decimal(18,2)) 
													END AS Allowance
												, CASE WHEN IsPosted = 1 THEN SAllowanceDetail
													ELSE ISNULL(AllowanceDetail,'') 
													END AS AllowanceDetail  
												, CASE WHEN IsPosted = 1 THEN SAbsentAmount
													ELSE CAST(AbsentAmount AS decimal(18,0)) 
													END AS AbsentAmount
												, CASE WHEN IsPosted = 1 THEN SAbsentDeductedAmount
													ELSE 
														CASE WHEN @DepartmentTypeId =3 OR SalaryType = 3 THEN 0 else
														CAST(AbsentDeductedAmount AS bigint) 
														END END AS AbsentDeductedAmount
                                                , CASE WHEN IsPosted = 1 THEN SOvertimeAmount
													ELSE 
													CASE WHEN @DepartmentTypeId =3 OR SalaryType = 3 THEN 0 else
														CAST(CASE WHEN (TotalOverTimeHours - TotalLateHours) < 0 then (TotalOverTimeHours - TotalLateHours) * PerHourRate
																	when (TotalOverTimeHours - TotalLateHours) > 0 then (TotalOverTimeHours-TotalLateHours) * OverTimeRate else 0 end
																	as bigint) 
														END END AS OvertimeAmount
												, CAST(OthersAddition AS decimal(18,2)) AS OthersAddition
						                        , CASE WHEN IsPosted = 1 THEN SGrossPay
													ELSE 
													CASE WHEN @DepartmentTypeId =3 OR SalaryType = 3 THEN -- for fix and  directors
																	SalaryAmount
																	+ OthersAddition
																	+ Allowance
																	ELSE
																	CASE WHEN @DepartmentTypeId = 2 THEN -- for Official Staff
																	CAST(TotalWorkingHoursCount * PerHourRate
																		+ CASE WHEN TotalAbsents>0 AND (TotalPresents>10 OR TotalPaidLeaves > 10) THEN PerDayRate ELSE 0 END
																		+ CASE WHEN (TotalPresents > 6 OR TotalPaidLeaves > 6) THEN PerDayRate * PaidSundays ELSE 0 END    
																		+ Allowance 
																		+ OthersAddition AS BIGINT)
																	ELSE
																	CAST(TotalWorkingHoursCount * PerHourRate -- for production
																	+ CASE WHEN (TotalOverTimeHours - TotalLateHours) > 0 then (TotalOverTimeHours * OverTimeRate -(TotalLateHours*OverTimeRate - TotalLateHours*PerHourRate))  
																	else
																	CASE WHEN (TotalOverTimeHours - TotalLateHours) < 0 then TotalOverTimeHours * PerHourRate else 0 end end
											                        + CASE WHEN TotalAbsents>0 AND (TotalPresents>10 OR TotalPaidLeaves > 10) THEN PerDayRate ELSE 0 END
																	+ CASE WHEN (TotalPresents > 6 OR TotalPaidLeaves > 6) THEN PerDayRate * PaidSundays ELSE 0 END    
																	+ Allowance 
																	+ OthersAddition AS BIGINT)
																	END END END
																	AS GrossPay
						                        , CASE WHEN IsPosted = 1 THEN SNetPay 
												  ELSE 
													CASE WHEN @DepartmentTypeId =3 OR SalaryType = 3 THEN --for fix and directors
																	SalaryAmount
																	+ OthersAddition
																	+ Allowance
																	- (Installment
											                        + Advance
											                        + IncomeTax
											                        + OthersDeduction)
													else 
													CASE WHEN @DepartmentTypeId = 2 THEN -- for Official Staff
																	CAST(TotalWorkingHoursCount * PerHourRate
																		+ CASE WHEN TotalAbsents>0 AND (TotalPresents>10 OR TotalPaidLeaves > 10) THEN PerDayRate ELSE 0 END
																		+ CASE WHEN (TotalPresents > 6 OR TotalPaidLeaves > 6) THEN PerDayRate * PaidSundays ELSE 0 END    
																		+ Allowance 
																		+ OthersAddition AS BIGINT)
													ELSE -- for production
																	CAST(TotalWorkingHoursCount * PerHourRate
																	+ CASE WHEN (TotalOverTimeHours - TotalLateHours) > 0 then (TotalOverTimeHours * OverTimeRate -(TotalLateHours*OverTimeRate - TotalLateHours*PerHourRate)) else 0 end
																	+ CASE WHEN (TotalOverTimeHours - TotalLateHours) < 0 then TotalOverTimeHours * PerHourRate else 0 end
											                        + CASE WHEN TotalAbsents>0 AND (TotalPresents>10 OR TotalPaidLeaves > 10) THEN PerDayRate ELSE 0 END
																	+ CASE WHEN (TotalPresents > 6 OR TotalPaidLeaves > 6) THEN PerDayRate * PaidSundays ELSE 0 END
																	+ Allowance 
																	+ OthersAddition
											                        - (Installment
											                        + Advance
											                        + IncomeTax
											                        + OthersDeduction) as bigint)
													END END END as NetPay
						                        , CAST(Installment AS decimal(18,2)) AS Installment
						                        , CAST(Advance AS decimal(18,2)) AS Advance
						                        , IncomeTax 
						                        , OthersDeduction 
												, ISNULL(Remarks,'') AS Remarks
						                        , OutletId
                                                , ISNULL(IsPosted, 0) AS IsPosted
                                                , CONVERT(VARCHAR,JoiningDate, 107) AS JoiningDate
												, ContactNumber
												, ImagePath
												, Gender
                                                , FatherName
                                                , @TotalMonthDays AS TotalMonthDays
                                                , CASE WHEN IsPosted = 1 THEN STotalWorkAmount
													ELSE 
													CAST((ISNULL(TotalWorkingHoursCount * PerHourRate,0) + AbsentAmount) AS Decimal(18,1)) 
													END AS TotalWorkAmount
                                                , IsActive
                                                , SalaryType
                                                , ISNULL(BankAccountId,0) AS BankAccountId
												, CASE WHEN IsPosted = 1 THEN STotalAbsents
													ELSE TotalAbsents
													END AS TotalAbsents
												, CASE WHEN IsPosted = 1 THEN STotalPresents
													ELSE TotalPresents
													END AS TotalPresents
												, CASE WHEN IsPosted = 1 THEN STotalPaidLeaves
													ELSE TotalPaidLeaves
													END AS TotalPaidLeaves
												, CASE WHEN IsPosted = 1 THEN SSundaysNotPay
													ELSE SundaysNotPay
													END AS SundaysNotPay
												, CAST(ISNULL(Balance,0) AS DECIMAL(18,0)) AS LoanBalance
						                        FROM CTE
                                                WHERE DepartmentTypeId = @DepartmentTypeId 
                                                AND JoiningDate <= DATEFROMPARTS(@Year, @Month,28)
                                                AND (IsActive = 1 OR SalarySheetId > 0 )
                                                AND SalaryType <> 2";

                return await _unit.DapperRepository.GetListQueryAsync<SalarySheet>(query1);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WagesRequest>> GetWagesApprovalList(PayrollParameterRequest request)
        {
            try
            {
                var query = $@"SELECT
	                                E.EmployeeId
	                                , E.EmployeeName
	                                , WagesId
	                                , E.FatherName
	                                , E.DepartmentsName
	                                , E.DesignationName
                                    , E.OutletId
	                                , W.WagesAmount
									, Advance
									, Installment
									, ISNULL(Remarks,'') AS Remarks
									, WagesDate
									, IsPosted
                                FROM V_EMPLOYEE AS E
								INNER JOIN Wages AS W
									ON W.EmployeeId = E.EmployeeId
									AND W.OutletId = E.OutletId
									AND W.IsPosted = 0
                                WHERE SalaryType = 2";

                return await _unit.DapperRepository.GetListQueryAsync<WagesRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WagesRequest>> GenerateWages(PayrollParameterRequest request)
        {
            try
            {
                //        var query = $@"
                //                         SELECT
                //                         PP.EmployeeId
                //                         , E.EmployeeName
                //                         , E.FatherName
                //                         , E.DepartmentsName
                //                         , E.DesignationName
                //                            , E.OutletId
                //                         , CAST(SUM((ISNULL(PP.ProcessRate,0) * ISNULL(ReceiveQuantity,0)))AS DECIMAL(18,0)) AS WagesAmount
                //	, CAST(ISNULL(tblAdvance.Advance,0) AS DECIMAL(18,0)) AS Advance
                //	, CAST(ISNULL(tblInstallment.Installment,0)AS DECIMAL(18,0)) AS Installment
                //                        FROM ProductionProcess AS PP
                //                        INNER JOIN V_EMPLOYEE AS E
                //                         ON E.EmployeeId = PP.EmployeeId
                //LEFT JOIN (SELECT EmployeeId
                //                                      , CASE WHEN  ISNULL(MAX(Balance),0)>SUM(Installment) THEN SUM(Installment) ELSE ISNULL(MAX(Balance),0) END as Installment
                //                                      FROM
                //                                      (
                //                                      SELECT  emp.EmployeeId
                //                                        , Installment
                //                                        , ext.Balance
                //                                         FROM EmployeeLoan as emp
                //                                          INNER JOIN
                //                                        (
                //                                         SELECT EmployeeId
                //                                         , SUM(LoanAmount-DeductAmount) as Balance 
                //                                         FROM EmployeeLoan
                //                                         WHERE LoanTypeId=1 --Loan
                //                                                       AND IsApproved=1
                //                                         GROUP BY EmployeeId
                //                                         HAVING SUM(LoanAmount-DeductAmount)>0
                //                                        ) ext
                //                                        ON emp.EmployeeId=ext.EmployeeId
                //                                       WHERE LoanTypeId=1
                //                                       AND Installment>0
                //                                      )as tbl
                //                                      GROUP BY EmployeeId
                //                    )as tblInstallment
                //                ON e.EmployeeId=tblInstallment.EmployeeId
                //              LEFT JOIN
                //              (
                //               -- Advances
                //               SELECT EmployeeId
                //                  , SUM(LoanAmount) as Advance
                //                  FROM EmployeeLoan
                //                  WHERE IsApproved=1
                //                  AND LoanTypeId=2	
                //                  AND LoanAmount>0								                  
                //                                          AND IsAdvanceDeducted=0
                //				  GROUP BY EmployeeId
                //              )as tblAdvance
                //                ON e.EmployeeId=tblAdvance.EmployeeId
                //                        WHERE PP.ReceiveQuantity > 0
                //                        AND PP.Status = 2
                //                        AND SalaryType = 2
                //                        AND PP.OutletId = 1
                //                        AND PP.IsPosted = 0
                //                        GROUP BY 
                //                         PP.EmployeeId
                //                         , E.EmployeeName
                //                         , E.FatherName
                //                         , E.DepartmentsName
                //                         , E.DesignationName
                //                         , E.OutletId
                //	, Advance
                //	, Installment";

                var query = $@"SELECT
	                                E.EmployeeId
	                                , E.EmployeeName
	                                , E.FatherName
	                                , E.DepartmentsName
	                                , E.DesignationName
                                    , E.OutletId
	                                , 0 AS WagesAmount
	                                , GETDATE() AS WagesDate
									, CAST(ISNULL(tblAdvance.Advance,0) AS DECIMAL(18,0)) AS Advance
									, CAST(ISNULL(tblInstallment.Installment,0)AS DECIMAL(18,0)) AS Installment
                                FROM V_EMPLOYEE AS E
								LEFT JOIN (SELECT EmployeeId
		                                            , CASE WHEN  ISNULL(MAX(Balance),0)>SUM(Installment) THEN SUM(Installment) ELSE ISNULL(MAX(Balance),0) END as Installment
		                                            FROM
		                                            (
		                                            SELECT  emp.EmployeeId
				                                            , Installment
				                                            , ext.Balance
					                                            FROM EmployeeLoan as emp
						                                            INNER JOIN
				                                            (
					                                            SELECT EmployeeId
					                                            , SUM(LoanAmount-DeductAmount) as Balance 
					                                            FROM EmployeeLoan
					                                            WHERE LoanTypeId=1 --Loan
                                                               AND IsApproved=1
					                                            GROUP BY EmployeeId
					                                            HAVING SUM(LoanAmount-DeductAmount)>0
				                                            ) ext
				                                            ON emp.EmployeeId=ext.EmployeeId
			                                            WHERE LoanTypeId=1
			                                            AND Installment>0
		                                            )as tbl
		                                            GROUP BY EmployeeId
					                       )as tblInstallment
								                ON e.EmployeeId=tblInstallment.EmployeeId
						                LEFT JOIN
						                (
							                -- Advances
							                SELECT EmployeeId
								                  , SUM(LoanAmount) as Advance
								                  FROM EmployeeLoan
								                  WHERE IsApproved=1
								                  AND LoanTypeId=2	
								                  AND LoanAmount>0								                  
                                                  AND IsAdvanceDeducted=0
												  GROUP BY EmployeeId
						                )as tblAdvance
								                ON e.EmployeeId=tblAdvance.EmployeeId
                                WHERE SalaryType = 2";

                return await _unit.DapperRepository.GetListQueryAsync<WagesRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SaveWages(List<WagesRequest> request)
        {
            try
            {
                var employeeIds = request.Where(r => r.OutletId > 0 && r.EmployeeId > 0)
                                                    ?.Select(p => p.EmployeeId).ToList() ?? new List<long>();
                var employeeIdsString = string.Join(",", employeeIds);
                if (employeeIdsString?.Length > 0)
                {
                    // Post Wages and production process
                    //            var query = $@"DELETE FROM Wages
                    //                        WHERE WagesAmount > 0
                    //                        AND OutletId = {request.FirstOrDefault()?.OutletId}
                    //                        AND IsPosted = 0
                    //AND EmployeeId IN ({employeeIdsString})";
                    //

                    //Update
                    //                ProductionProcess SET IsPosted = 1
                    //            WHERE ReceiveQuantity > 0
                    //            AND Status = 2
                    //            AND OutletId = { request.OutletId }
                    //            AND IsPosted = 0

                    //            AND EmployeeId = { request.EmployeeId }

                    //await _unit.DapperRepository.ExecuteNonQuery(query, null, CommandType.Text);
                }
                request.ForEach(x =>
                {
                    if (x.WagesAmount > 0 && x.EmployeeId > 0 && x.OutletId > 0)
                    {
                        x.WagesId = 0;
                        _unit.WagesRepository.InsertSingle(_mapper.Map<Wages>(x));
                    }
                });

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<long> PostWages(WagesRequest request)
        {
            try
            {
                var wagesToPost = _mapper.Map<Wages>(request);
                wagesToPost.IsPosted = true;
                _unit.WagesRepository.Update(wagesToPost);


                var employeeAdvances = (await _unit
                                              .EmployeeLoanRepository
                                              .GetAsync(adv => adv.EmployeeId == request.EmployeeId
                                                                && adv.IsAdvanceDeducted == false
                                                                && adv.LoanTypeId == (int)LoanType.Advance
                                                                && adv.LoanAmount > 0
                                                                && adv.IsApproved == true))
                                              ?.ToList() ?? new List<EmployeeLoan>();

                if (employeeAdvances?.Count > 0)
                {
                    employeeAdvances.ForEach(adv => { adv.IsAdvanceDeducted = true; });
                    _unit.EmployeeLoanRepository.UpdateList(employeeAdvances);
                }

                var configurationList = (await _unit
                                               .ConfigurationSettingRepository
                                               .GetAsync(x => x.OutletId == request.OutletId))?.ToList();

                var wagesAccountId = configurationList?.FirstOrDefault(d => d.AccountName == "Wages")?.AccountValue ?? 0;
                var cashAccountId = configurationList?.FirstOrDefault(d => d.AccountName == "CashAccount")?.AccountValue ?? 0;
                if (wagesAccountId > 0)
                {
                    var postingAccountsList = await _chartOfAccountsService.GetPostingAccountList(request.OutletId);
                    var employeeAccounts = postingAccountsList.Where(e => e.EmployeeId == request.EmployeeId);

                    // JV
                    var journalVoucher = new VouchersMaster
                    {
                        VoucherDate = DateHelper.GetCurrentDate().Date, // set date
                        ReferenceNo = "",
                        VoucherTypeId = (int)VoucherType.Journal,
                        Remarks = $"Wages",
                        TotalAmount = request.WagesAmount,
                        OutletId = request.OutletId,
                        TransactionType = VoucherTransactionType.None.ToString(),
                        VoucherStatus = (int)VoucherStatus.Posted
                    };

                    //voucher detail
                    journalVoucher.VouchersDetail.Add(new VouchersDetail
                    {
                        PostingAccountsId = wagesAccountId, // Wages a/c
                        DebitAmount = request.WagesAmount,
                        CreditAmount = decimal.Zero,
                        Narration = $"Wages"
                    });

                    var postingAccountId = employeeAccounts?.FirstOrDefault(e => (e.EmployeeId ?? 0) == request.EmployeeId)?.PostingAccountsId ?? 0;
                    if (postingAccountId > 0)
                    {
                        journalVoucher.VouchersDetail.Add(new VouchersDetail
                        {
                            PostingAccountsId = postingAccountId, // Contractor a/c
                            DebitAmount = decimal.Zero,
                            CreditAmount = request.WagesAmount,
                            Narration = $""
                        });
                    }

                    var netPayVoucher = new VouchersMaster();
                    if ((request.WagesAmount - (request.Advance + request.Installment)) > 0 && postingAccountId > 0)
                    {
                        // Net Pay

                        netPayVoucher.VoucherDate = DateHelper.GetCurrentDate().Date; // set date
                        netPayVoucher.ReferenceNo = "";
                        netPayVoucher.VoucherTypeId = (int)VoucherType.CashPayment;
                        netPayVoucher.Remarks = $"Net Wages After Installment/Advance";
                        netPayVoucher.TotalAmount = request.WagesAmount - (request.Installment + request.Advance);
                        netPayVoucher.OutletId = request.OutletId;
                        netPayVoucher.TransactionType = VoucherTransactionType.None.ToString();
                        netPayVoucher.VoucherStatus = (int)VoucherStatus.Posted;

                        //voucher detail
                        netPayVoucher.VouchersDetail.Add(new VouchersDetail
                        {
                            PostingAccountsId = postingAccountId, // Person a/c
                            DebitAmount = request.WagesAmount - (request.Installment + request.Advance),
                            CreditAmount = decimal.Zero,
                            Narration = $"Net Pay ({(request.Installment > 0 ? request.Installment : "")} / {(request.Advance > 0 ? request.Advance : "")})"
                        });

                        //voucher detail
                        netPayVoucher.VouchersDetail.Add(new VouchersDetail
                        {
                            PostingAccountsId = cashAccountId, // Cash a/c
                            DebitAmount = decimal.Zero,
                            CreditAmount = request.WagesAmount - (request.Installment + request.Advance),
                            Narration = $"Net Wages pay"
                        });


                    }

                    var totalWages = new EmployeeLoan()
                    {
                        EmployeeId = request.EmployeeId,
                        DeductAmount = request.WagesAmount,
                        LoanTypeId = (int)LoanType.Wages,
                        LoanDate = DateHelper.GetCurrentDate().Date,
                        OutletId = request.OutletId,
                        Remarks = "Total Wages",
                        IsApproved = true
                    };
                    _unit.EmployeeLoanRepository.InsertSingle(totalWages);

                    var paidWages = new EmployeeLoan()
                    {
                        EmployeeId = request.EmployeeId,
                        DeductAmount = 0,
                        LoanAmount = request.WagesAmount - (request.Installment + request.Advance),
                        LoanTypeId = (int)LoanType.PaidWages,
                        LoanDate = DateHelper.GetCurrentDate().Date,
                        OutletId = request.OutletId,
                        Remarks = "Paid Wages",
                        IsApproved = true
                    };
                    _unit.EmployeeLoanRepository.InsertSingle(paidWages);

                    _unit.VouchersMasterRepository.InsertSingle(journalVoucher);
                    if (netPayVoucher.TotalAmount > 0)
                    {
                        _unit.VouchersMasterRepository.InsertSingle(netPayVoucher);
                    }
                    //

                    if (await _unit.SaveAsync())
                    {
                        // Post Wages
                        var query = $@"Update
	                                ProductionProcess SET IsPosted = 1
                                WHERE ReceiveQuantity > 0
                                AND Status = 2
                                AND OutletId = {request.OutletId}
                                AND IsPosted = 0
								AND EmployeeId = {request.EmployeeId}

                                        UPDATE Wages SET IsPosted = 1
                                        WHERE IsPosted = 0
                                        AND WagesId = {request.WagesId}
                                        AND EmployeeId = {request.EmployeeId}
                                        AND OutletId = {request.OutletId}";
                        //

                        await _unit.DapperRepository.ExecuteNonQuery(query, null, CommandType.Text);
                        return netPayVoucher.VouchersMasterId;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return 0;
        }

        public async Task<List<SalarySheet>> SaveSalarySheet(List<SalarySheet> request, bool isToPost)
        {
            try
            {
                var defaultSalary = request?.FirstOrDefault();
                var monthName = CultureInfo.CurrentCulture?.DateTimeFormat?.GetMonthName(defaultSalary.SalaryDate.Month) ?? "";
                var year = defaultSalary.SalaryDate.Year;
                var employeeIds = request?.Select(t => t.EmployeeId)?.ToList() ?? new List<long>();
                if (defaultSalary?.SalaryDate.Month > 0)
                {
                    var salaryToDelete = (await _unit
                                               .SalarySheetRepository
                                               .GetAsync(x => x.SalaryDate.Month == defaultSalary.SalaryDate.Month
                                                         && x.SalaryDate.Year == defaultSalary.SalaryDate.Year
                                                         && x.IsPosted == false
                                                         && employeeIds.Contains(x.EmployeeId)))
                                            ?.ToList()
                                            ?? new List<SalarySheet>();

                    if (salaryToDelete?.Count > 0)
                    {
                        _unit.SalarySheetRepository.DeleteRangeEntities(salaryToDelete);
                    }
                }

                _unit.SalarySheetRepository.InsertList(request);

                // Employee Ledger Work
                if (isToPost == true)
                {
                    // Advance Deduct update
                    var advancesToUpdate = (await _unit
                                                 .EmployeeLoanRepository
                                                 .GetAsync(o => employeeIds.Contains(o.EmployeeId)
                                                                && o.LoanTypeId == (int)LoanType.Advance
                                                                && o.IsApproved == true
                                                                && o.IsAdvanceDeducted == false)).ToList();
                    advancesToUpdate.ForEach(r => r.IsAdvanceDeducted = true);
                    _unit.EmployeeLoanRepository.UpdateList(advancesToUpdate);
                    //

                    foreach (var singleSalary in request)
                    {
                        var overtime = singleSalary.OvertimeAmount > 0 ? singleSalary.OvertimeAmount : 0;
                        if (singleSalary.SalaryAmount > 0)
                        {
                            var salary = singleSalary.GrossPay
                                            - singleSalary.Allowance
                                            - singleSalary.OthersDeduction
                                            - singleSalary.IncomeTax
                                            - overtime;

                            var employeeLoan = new EmployeeLoan()
                            {
                                EmployeeId = singleSalary.EmployeeId,
                                DeductAmount = salary,
                                LoanTypeId = (int)LoanType.Salary,
                                LoanDate = DateHelper.GetCurrentDate().Date,
                                OutletId = defaultSalary.OutletId,
                                Remarks = $"Salary {monthName}-{year}",
                                IsApproved = true
                            };
                            _unit.EmployeeLoanRepository.InsertSingle(employeeLoan);
                        }

                        if (singleSalary.Allowance > 0)
                        {
                            var employeeLoan = new EmployeeLoan()
                            {
                                EmployeeId = singleSalary.EmployeeId,
                                DeductAmount = singleSalary.Allowance,
                                LoanTypeId = (int)LoanType.Allowance,
                                LoanDate = DateHelper.GetCurrentDate().Date,
                                OutletId = defaultSalary.OutletId,
                                Remarks = $"{singleSalary.AllowanceDetail} {monthName}-{year}",
                                IsApproved = true
                            };
                            _unit.EmployeeLoanRepository.InsertSingle(employeeLoan);
                        }

                        if (overtime > 0)
                        {
                            var employeeLoan = new EmployeeLoan()
                            {
                                EmployeeId = singleSalary.EmployeeId,
                                DeductAmount = singleSalary.OvertimeAmount,
                                LoanTypeId = (int)LoanType.OTAmount,
                                LoanDate = DateHelper.GetCurrentDate().Date,
                                OutletId = defaultSalary.OutletId,
                                Remarks = $"O.T {monthName}-{year}",
                                IsApproved = true
                            };
                            _unit.EmployeeLoanRepository.InsertSingle(employeeLoan);
                        }

                        if (singleSalary.NetPay > 0)
                        {
                            var paidSalary = singleSalary.NetPay;
                            var employeeLoan = new EmployeeLoan()
                            {
                                EmployeeId = singleSalary.EmployeeId,
                                LoanAmount = paidSalary,
                                LoanTypeId = (int)LoanType.PaidSalary,
                                LoanDate = DateHelper.GetCurrentDate().Date,
                                OutletId = defaultSalary.OutletId,
                                Remarks = $"Paid {monthName}-{year} {(singleSalary.Installment > 0 ? $"Inst({singleSalary.Installment})" : "")}",
                                IsApproved = true
                            };
                            _unit.EmployeeLoanRepository.InsertSingle(employeeLoan);
                        }
                    }
                    //

                    var configurationList = (await _unit
                                                  .ConfigurationSettingRepository
                                                  .GetAsync(x => x.OutletId == defaultSalary.OutletId))?.ToList();

                    if (configurationList?.Count > 0)
                    {
                        //var salariesPaidAccountId = configurationList.FirstOrDefault(d => d.AccountName == "Salary Paid")?.AccountValue ?? 0;
                        var cashAccountId = configurationList.FirstOrDefault(d => d.AccountName == "CashAccount")?.AccountValue ?? 0;

                        var overtimeId = configurationList.FirstOrDefault(d => d.AccountName == "OvertimeProduction")?.AccountValue ?? 0;

                        /********* Generate auto Voucher in Accounting system  ************/
                        var salaryList = request.ToList();
                        var productionExpenseAmount = decimal.Zero;
                        var netSalariesId = new Int64();

                        if (defaultSalary.DepartmentTypeId == (int)DepartmentType.Production)
                        {
                            netSalariesId = configurationList.FirstOrDefault(d => d.AccountName == "SalaryProduction")?.AccountValue ?? 0;
                            productionExpenseAmount = salaryList
                                                            .Where(d => d.DepartmentTypeId == (int)DepartmentType.Production)
                                                            ?.Sum(c => (c.GrossPay - (c.OvertimeAmount > 0 ? c.OvertimeAmount : 0) - c.OthersDeduction - c.IncomeTax))
                                                            ?? decimal.Zero;
                        }
                        else if (defaultSalary.DepartmentTypeId == (int)DepartmentType.OfficialStaff)
                        {
                            netSalariesId = configurationList.FirstOrDefault(d => d.AccountName == "SalaryOfficialStaff")?.AccountValue ?? 0;
                            productionExpenseAmount = salaryList
                                                        .Where(d => d.DepartmentTypeId == (int)DepartmentType.OfficialStaff)
                                                        .Sum(c => (c.GrossPay - (c.OvertimeAmount > 0 ? c.OvertimeAmount : 0) - c.OthersDeduction - c.IncomeTax));
                        }
                        else if (defaultSalary.DepartmentTypeId == (int)DepartmentType.Directors)
                        {
                            netSalariesId = configurationList.FirstOrDefault(d => d.AccountName == "SalaryDirector")?.AccountValue ?? 0;
                            productionExpenseAmount = salaryList
                                                        .Where(d => d.DepartmentTypeId == (int)DepartmentType.Directors)
                                                        .Sum(c => (c.GrossPay - (c.OvertimeAmount > 0 ? c.OvertimeAmount : 0) - c.OthersDeduction - c.IncomeTax));
                        }

                        var overtimePaidAmount = salaryList
                                                    .Sum(c => (c.OvertimeAmount > 0 ? c.OvertimeAmount : 0));

                        var salariesPaidAmount = productionExpenseAmount + overtimePaidAmount;

                        var postingAccounts = await _chartOfAccountsService.GetPostingAccountList(defaultSalary.OutletId);

                        // JV
                        var voucher = new VouchersMaster
                        {
                            VoucherDate = DateHelper.GetCurrentDate().Date, // set date
                            ReferenceNo = "",
                            VoucherTypeId = (int)VoucherType.Journal,
                            Remarks = $"Salary of {monthName}-{year}",
                            TotalAmount = salariesPaidAmount,
                            OutletId = defaultSalary.OutletId,
                            TransactionType = VoucherTransactionType.None.ToString(),
                            VoucherStatus = (int)VoucherStatus.Posted
                        };

                        //voucher detail
                        voucher.VouchersDetail.Add(new VouchersDetail
                        {
                            PostingAccountsId = netSalariesId, // Salary a/c
                            DebitAmount = productionExpenseAmount,
                            CreditAmount = decimal.Zero,
                            Narration = $"Gross Salary Amount {monthName}-{year}"
                        });
                        if (overtimePaidAmount > 0)
                        {
                            voucher.VouchersDetail.Add(new VouchersDetail
                            {
                                PostingAccountsId = overtimeId, // Cash a/c
                                DebitAmount = overtimePaidAmount,
                                CreditAmount = decimal.Zero,
                                Narration = $"Overtime Amount {monthName}-{year}"
                            });
                        }

                        salaryList.ForEach(person =>
                        {
                            var postingAccountId = postingAccounts?.FirstOrDefault(e => (e.EmployeeId ?? 0) == person.EmployeeId)?.PostingAccountsId ?? 0;
                            if (postingAccountId > 0 && (person.GrossPay - person.OthersDeduction - person.IncomeTax) > 0)
                            {
                                voucher.VouchersDetail.Add(new VouchersDetail
                                {
                                    PostingAccountsId = postingAccountId, //  a/c
                                    DebitAmount = decimal.Zero,
                                    CreditAmount = (person.GrossPay - person.OthersDeduction - person.IncomeTax),
                                    Narration = $""
                                });
                            }
                        });

                        _unit.VouchersMasterRepository.InsertSingle(voucher);
                        //

                        // second payment voucher
                        if (cashAccountId > 0)
                        {
                            var cashNetAmount = salaryList.Where(u => u.BankAccountId == 0).Sum(c => c.NetPay);

                            var cashVoucher = new VouchersMaster
                            {
                                VoucherDate = DateHelper.GetCurrentDate().Date, // set date
                                ReferenceNo = $"",
                                VoucherTypeId = (int)VoucherType.CashPayment,
                                Remarks = $"Net Pay of {monthName}-{year} after deductions",
                                TotalAmount = cashNetAmount,
                                OutletId = defaultSalary.OutletId,
                                TransactionType = VoucherTransactionType.Cash.ToString(),
                                VoucherStatus = (int)VoucherStatus.Posted
                            };

                            //voucher detail
                            salaryList.Where(u => u.BankAccountId == 0)?.ToList()?.ForEach(person =>
                            {
                                var postingAccountId = postingAccounts?.FirstOrDefault(e => (e.EmployeeId ?? 0) == person.EmployeeId)?.PostingAccountsId ?? 0;

                                if (postingAccountId > 0 && person.NetPay > 0)
                                {
                                    if (person.OvertimeAmount > 0)
                                    {
                                        cashVoucher.VouchersDetail.Add(new VouchersDetail
                                        {
                                            PostingAccountsId = postingAccountId, // person a/c
                                            DebitAmount = person.OvertimeAmount,
                                            CreditAmount = decimal.Zero,
                                            Narration = $"Overtime Paid {monthName}-{year}"
                                        });

                                        cashVoucher.VouchersDetail.Add(new VouchersDetail
                                        {
                                            PostingAccountsId = postingAccountId, // person a/c
                                            DebitAmount = person.NetPay - person.OvertimeAmount,
                                            CreditAmount = decimal.Zero,
                                            Narration = $"Salary Paid {monthName}-{year} {(person.Installment > 0 ? $"Inst({person.Installment})" : "")}"
                                        });
                                    }
                                    else
                                    {
                                        cashVoucher.VouchersDetail.Add(new VouchersDetail
                                        {
                                            PostingAccountsId = postingAccountId, // person a/c
                                            DebitAmount = person.NetPay,
                                            CreditAmount = decimal.Zero,
                                            Narration = $"Salary Paid {monthName}-{year} {(person.Installment > 0 ? $"Inst({person.Installment})" : "")}"
                                        });
                                    }
                                }
                            });

                            cashVoucher.VouchersDetail.Add(new VouchersDetail
                            {
                                PostingAccountsId = cashAccountId, // Cash a/c
                                DebitAmount = decimal.Zero,
                                CreditAmount = cashNetAmount,
                                Narration = $"cash payment"
                            });
                            _unit.VouchersMasterRepository.InsertSingle(cashVoucher);
                            //

                            // Banks Payment Voucher
                            var bankNetAmount = salaryList.Where(u => u.BankAccountId > 0)?.Sum(c => c.NetPay) ?? 0;
                            if (bankNetAmount > 0)
                            {
                                var bankVoucher = new VouchersMaster
                                {
                                    VoucherDate = DateHelper.GetCurrentDate().Date, // set date
                                    ReferenceNo = $"",
                                    VoucherTypeId = (int)VoucherType.CashPayment,
                                    Remarks = $"Net Pay of {monthName}-{year} after deductions",
                                    TotalAmount = bankNetAmount,
                                    OutletId = defaultSalary.OutletId,
                                    TransactionType = VoucherTransactionType.Bank.ToString(),
                                    VoucherStatus = (int)VoucherStatus.Posted
                                };

                                //voucher detail
                                salaryList.Where(u => u.BankAccountId > 0)?.ToList()?.ForEach(person =>
                                {
                                    var postingAccountId = postingAccounts?.FirstOrDefault(e => (e.EmployeeId ?? 0) == person.EmployeeId)?.PostingAccountsId ?? 0;

                                    if (postingAccountId > 0 && person.NetPay > 0)
                                    {
                                        //person debit
                                        if (person.OvertimeAmount > 0)
                                        {
                                            bankVoucher.VouchersDetail.Add(new VouchersDetail
                                            {
                                                PostingAccountsId = postingAccountId, // overtime
                                                DebitAmount = person.OvertimeAmount,
                                                CreditAmount = decimal.Zero,
                                                Narration = $"Overtime Paid {monthName}-{year}"
                                            });

                                            bankVoucher.VouchersDetail.Add(new VouchersDetail
                                            {
                                                PostingAccountsId = postingAccountId, // person a/c
                                                DebitAmount = person.NetPay - person.OvertimeAmount,
                                                CreditAmount = decimal.Zero,
                                                Narration = $"Salary Paid {monthName}-{year} {(person.Installment > 0 ? $"Inst({person.Installment})" : "")}"
                                            });
                                        }
                                        else
                                        {
                                            bankVoucher.VouchersDetail.Add(new VouchersDetail
                                            {
                                                PostingAccountsId = postingAccountId, // person a/c
                                                DebitAmount = person.NetPay,
                                                CreditAmount = decimal.Zero,
                                                Narration = $"Salary Paid {monthName}-{year} {(person.Installment > 0 ? $"Inst({person.Installment})" : "")}"
                                            });
                                        }

                                        // bank credit
                                        bankVoucher.VouchersDetail.Add(new VouchersDetail
                                        {
                                            PostingAccountsId = person.BankAccountId, // bank a/c
                                            DebitAmount = decimal.Zero,
                                            CreditAmount = person.NetPay,
                                            Narration = $"Bank payment"
                                        });
                                    }
                                });

                                _unit.VouchersMasterRepository.InsertSingle(bankVoucher);
                            }
                            //
                        }
                    }
                }

                if (await _unit.SaveAsync())
                {
                    return request;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }
    }
}