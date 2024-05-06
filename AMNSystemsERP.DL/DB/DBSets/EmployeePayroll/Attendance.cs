using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class Attendance : DeletableEntity
    {
        public Attendance()
        {
            AttendanceDetails = new HashSet<AttendanceDetail>();
        }

        [Key]
        public long AttendanceId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public int AttendanceStatusId { get; set; }
        [Required]
        public DateTime AttendanceDate { get; set; }
        [Required]
        [MaxLength(11)]
        public string MarkType { get; set; }

        public virtual ICollection<AttendanceDetail> AttendanceDetails { get; set; }
    }
}
