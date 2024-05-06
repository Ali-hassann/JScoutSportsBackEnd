using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class Overtime : Entity
    {
        public Overtime()
        {
            OvertimeDetails = new HashSet<OvertimeDetail>();
        }

        [Key]
        public long OvertimeId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public DateTime OvertimeDate { get; set; }
        [Required]
        [MaxLength(11)]
        public string MarkType { get; set; }

        public virtual ICollection<OvertimeDetail> OvertimeDetails { get; set; }
    }
}
