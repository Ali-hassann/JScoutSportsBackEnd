using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class EmployeeAllowances : Entity
    {
        [Key]
        public long EmployeeAllowancesId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public long AllowanceTypeId { get; set; }
        [Required]
        public long EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
