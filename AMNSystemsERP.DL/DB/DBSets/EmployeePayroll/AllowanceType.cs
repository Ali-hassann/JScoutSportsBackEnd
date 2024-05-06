using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class AllowanceType : DeletableEntity
    {
        [Key]
        public long AllowanceTypeId { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public long? OutletId { get; set; }
        [Required]
        public long? OrganizationId { get; set; }
    }
}