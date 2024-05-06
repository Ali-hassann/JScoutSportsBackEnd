using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class Departments : DeletableEntity
    {
        [Key]
        public long DepartmentsId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(80)]
        public string Description { get; set; }
        public int DepartmentTypeId { get; set; }
        public new long? OrganizationId { get; set; }
    }
}
