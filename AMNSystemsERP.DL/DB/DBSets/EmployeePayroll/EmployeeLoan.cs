using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class EmployeeLoan : Entity
    {
        [Key]
        public long EmployeeLoanId { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal Installment { get; set; }
        public decimal DeductAmount { get; set; }
        public DateTime LoanDate { get; set; }
        public int LoanTypeId { get; set; }
        public long EmployeeId { get; set; }
        public long OutletId { get; set; }
        public string? Remarks { get; set; }
        public bool IsApproved { get; set; }
        public bool IsAdvanceDeducted { get; set; }
        //public decimal BasicSalary { get; set; }
        //public decimal GivenSalary { get; set; }
        //public decimal Overtime { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
