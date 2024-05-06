using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class EmployeeQualifications : Entity
    {
        public EmployeeQualifications()
        {
            QualificationSubjects = new HashSet<QualificationSubject>();
        }

        [Key]
        public long EmployeeQualificationsId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public long QualificationId { get; set; }
        [Required]
        public long EducationalOrganizationsId { get; set; }
        [Required]
        public int PassingYear { get; set; }
        [Required]
        public decimal CGPA { get; set; }
        [Required]
        public decimal Percentage { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual ICollection<QualificationSubject> QualificationSubjects { get; set; }
    }
}
