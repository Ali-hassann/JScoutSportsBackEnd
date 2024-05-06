using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class EmployeeDetail
    {
        [Key]
        public long EmployeeDetailId { get; set; }        
        public DateTime? DOB { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        [MaxLength(30)]
        public string Email { get; set; } 
        [MaxLength(50)]
        public string ContactNumber1 { get; set; }
        [MaxLength(50)]
        public string ContactNumber2 { get; set; }
        [MaxLength(50)]
        public string ContactNumber3 { get; set; }
        [MaxLength(25)]
        public string Nationality { get; set; }
        public long EmployeeId { get; set; }
        public string CasteName { get; set; }
        public string ReligionName { get; set; }

        public virtual Employee Employee { get; set; }       
    }
}
