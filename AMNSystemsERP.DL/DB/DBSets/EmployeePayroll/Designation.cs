using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class Designation : DeletableEntity
    {
        [Key]
        public long DesignationId { get; set; }
        [Required]
        [MaxLength(50)]
        public string DesignationName { get; set; }
        public long OrganizationId { get; set; }
    }
}