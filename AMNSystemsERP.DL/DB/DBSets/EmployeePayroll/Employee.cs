using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class Employee : DeletableEntity
    {
        public Employee()
        {
            EmployeeQualifications = new HashSet<EmployeeQualifications>();
            EmployeeLoans = new HashSet<EmployeeLoan>();
            EmployeeAllowances = new HashSet<EmployeeAllowances>();
            SalarySheets = new HashSet<SalarySheet>();
        }

        [Key]
        public long EmployeeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string EmployeeName { get; set; }
        [MaxLength(50)]
        public string FatherName { get; set; }
        [MaxLength(50)]
        public string HusbandName { get; set; }
        [Required]
        [MaxLength(50)]
        public string ContactNumber { get; set; }
        [Required]
        [MaxLength(7)]
        public string Gender { get; set; }
        [Required]
        public string CNIC { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [MaxLength(250)]
        public string ImagePath { get; set; }
        [Required]
        public long DepartmentsId { get; set; }
        [Required]
        public long OutletId { get; set; }
        [Required]
        public long OrganizationId { get; set; }
        [Required]
        public DateTime JoiningDate { get; set; }
        public DateTime? LeftDate { get; set; }
        [Required]
        public decimal SalaryAmount { get; set; }
        [Required]
        public decimal OvertimeHourlyWageAmount { get; set; }
        [Required]
        public int WorkingHours { get; set; }
        public int SalaryType { get; set; }

        public long? DesignationId { get; set; }
        [MaxLength(7)]
        public string MaritalStatus { get; set; }
        [Required]
        public long EmployeeSerialNo { get; set; }
        [Required]
        [MaxLength(20)]
        public string EmployeeCode { get; set; }

        public virtual EmployeeDetail EmployeeDetailList { get; set; }

        public virtual ICollection<EmployeeQualifications> EmployeeQualifications { get; set; }
        public virtual ICollection<EmployeeLoan> EmployeeLoans { get; set; }
        public virtual ICollection<EmployeeAllowances> EmployeeAllowances { get; set; }
        public virtual ICollection<SalarySheet> SalarySheets { get; set; }
    }
}
