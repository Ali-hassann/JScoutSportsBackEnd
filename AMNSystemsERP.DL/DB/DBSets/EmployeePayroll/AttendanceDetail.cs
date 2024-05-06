using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class AttendanceDetail
    {
        [Key]
        public long AttendanceDetailId { get; set; }
        [Required]
        public long AttendanceId { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        [Required]
        [MaxLength(11)]
        public string DetailMarkType { get; set; }
        public string DeviceIP { get; set; }

        public virtual Attendance Attendance { get; set; }
    }
}
