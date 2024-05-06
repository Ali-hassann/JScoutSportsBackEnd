using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Organization
{
    public class Outlet : DeletableEntity
    {
        [Key]
        public new long OutletId { get; set; }
        [MaxLength(50)]
        [Required]
        public string OutletName { get; set; } = string.Empty;
        [MaxLength(200)]
        [Required]
        public string OutletOwnerName { get; set; } = string.Empty;
        [MaxLength(50)]
        [Required]
        public string ContactNumber { get; set; } = string.Empty;
        [MaxLength(100)]
        [Required]
        public string Address { get; set; } = string.Empty;
        [MaxLength(50)]
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; } = string.Empty;
        [MaxLength(3)]
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        public long City { get; set; }
        public long OrganizationId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(250)]
        [Required]
        public string ImagePath { get; set; } = string.Empty;
        [Required]
        public DateTime StartingTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }

        public virtual Organization Organization { get; set; }
    }
}