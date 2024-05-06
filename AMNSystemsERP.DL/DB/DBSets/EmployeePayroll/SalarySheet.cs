using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class SalarySheet
    {
        [Key]
        public long SalarySheetId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime SalaryDate { get; set; }
        public decimal SalaryAmount { get; set; }
        public string? WorkingDays { get; set; }
        public int TotalDays { get; set; }
        public int TotalHours { get; set; }
        public decimal TotalOverTimeHours { get; set; }
        public decimal OverTimeRate { get; set; }
        public decimal GrossPay { get; set; }
        public decimal Installment { get; set; }
        public decimal Advance { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal OthersDeduction { get; set; }
        public decimal NetPay { get; set; }
        public decimal Allowance { get; set; }
        public string? AllowanceDetail { get; set; }
        public decimal TotalWorkingHoursCount { get; set; }
        public decimal PerHourRate { get; set; }
        public decimal PerDayRate { get; set; }
        public string? AttendanceSummary { get; set; }
        public long OutletId { get; set; }
        public bool IsPosted { get; set; }
        public decimal OthersAddition { get; set; }
        public decimal SundaysAmount { get; set; }
        public decimal AbsentAmount { get; set; }
        public decimal AbsentDeductedAmount { get; set; }
        public decimal TotalLateHours { get; set; }
        public decimal TotalWorkAmount { get; set; }
        public decimal OvertimeAmount { get; set; }
        public int TotalSundays { get; set; }
        public string? Remarks { get; set; }
        public long BankAccountId { get; set; }
        public int TotalAbsents { get; set; }
        public int TotalPresents { get; set; }
        public int TotalPaidLeaves { get; set; }
        public int SundaysNotPay { get; set; }

        /*****  Not Mapped Properties  *******/
        [NotMapped]
        public string FatherName { get; set; }
        [NotMapped]
        public string EmployeeName { get; set; }
        [NotMapped]
        public long DepartmentsId { get; set; }
        [NotMapped]
        public string DepartmentsName { get; set; }
        [NotMapped]
        public string DesignationName { get; set; }
        [NotMapped]
        public string TotalHoursAndMinutes { get; set; }
        [NotMapped]
        public int TotalMinutes { get; set; }
        [NotMapped]
        public int TotalNetPay { get; set; }
        [NotMapped]
        public string JoiningDate { get; set; }
        [NotMapped]
        public string ContactNumber { get; set; }
        [NotMapped]
        public string ImagePath { get; set; }
        [NotMapped]
        public string Gender { get; set; }
        [NotMapped]
        public int TotalMonthDays { get; set; }
        [NotMapped]
        public int DepartmentTypeId { get; set; }
        [NotMapped]
        public decimal LoanBalance { get; set; }
        [NotMapped]
        public bool Selected { get; set; }
    }
}
