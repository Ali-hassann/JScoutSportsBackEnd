using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class EmployeeDocuments
    {
        [Key]
        public long EmployeeDocumentsId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        public string Imagepath { get; set; }
        public long OrganizationId { get; set; }
        public long OutletId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
