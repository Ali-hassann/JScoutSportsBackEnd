using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMNSystemsERP.CL.Models.OrganizationModels
{
    public class OrganizationRegRequests
    {
        [Key]
        public long OrganizationRequestId { get; set; }
        [MaxLength(50)]
        [Required]
        public string OrganizationName { get; set; }
        [MaxLength(50)]
        [Required]
        public string OwnerName { get; set; }
        [MaxLength(50)]
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [MaxLength(3)]
        [Required]
        public string Country { get; set; }
        [Required]
        public long City { get; set; }
        [MaxLength(50)]
        [Required]
        public string ContactNumber { get; set; }
        public string Status { get; set; }
        public long OrganizationId { get; set; } = 0;
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
        [NotMapped]
        public string CountryName { get; set; }
        [NotMapped]
        public string StateName { get; set; }
        [NotMapped]
        public string CityName { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        [NotMapped]
        public string Password { get; set; }
    }
}
