using AMNSystemsERP.CL.Models.EmployeePayrollModels.Enums;

namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Loans
{
    public class EmployeeLoanRequest
    {
        public long EmployeeLoanId { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal Installment { get; set; }
        public decimal DeductAmount { get; set; }
        public decimal Balance { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime LoanDate { get; set; }
        public LoanType LoanTypeId { get; set; }
        public string TypeName { get { return LoanTypeId.ToString(); } }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FatherName { get; set; }
        public string DepartmentsName { get; set; }
        public string DesignationName { get; set; }
        public string Remarks { get; set; }
        public bool IsApproved { get; set; }
        public bool IsAdvanceDeducted { get; set; }
        public long OutletId { get; set; }
        public long VoucherMasterId { get; set; }
        public bool IsToCreateVoucher { get; set; }
    }
}
