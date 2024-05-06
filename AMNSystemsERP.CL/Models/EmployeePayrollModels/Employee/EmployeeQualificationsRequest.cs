namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee
{
    public class EmployeeQualificationsRequest
    {
        public long EmployeeQualificationsId { get; set; }
        public long EmployeeId { get; set; }
        public long QualificationId { get; set; }
        public long EducationalOrganizationsId { get; set; }
        public int PassingYear { get; set; }
        public decimal CGPA { get; set; }
        public decimal Percentage { get; set; }
        public List<QualificationSubjectRequest> QualificationSubjectList { get; set; }
    }
    
    public class QualificationSubjectRequest
    {
        public long QualificationSubjectId { get; set; }
        public long EmployeeQualificationsId { get; set; }
        public long SubjectsId { get; set; }
    }
}
