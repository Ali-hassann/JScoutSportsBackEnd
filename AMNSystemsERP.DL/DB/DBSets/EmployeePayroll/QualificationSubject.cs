using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class QualificationSubject 
    {
        [Key]
        public long QualificationSubjectId { get; set; }
        [Required]
        public long EmployeeQualificationsId { get; set; }
        [Required]
        public long SubjectsId { get; set; }

        public virtual EmployeeQualifications EmployeeQualifications { get; set; }
    }
}