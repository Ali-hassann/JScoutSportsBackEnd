using AMNSystemsERP.DL.DB.Base;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMNSystemsERP.DL.DB.DBSets.Identity
{
    public class ApplicationUser : IdentityUser, IDeletableEntity
    {
        [Required]
        public long OutletId { get; set; }
        [Required]
        public long OrganizationId { get; set; }
        [NotMapped]
        public string Password { get; set; } = string.Empty;
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        [Required]
        [MaxLength(250)]
        public string ImagePath { get; set; } = string.Empty;
        [NotMapped]
        public bool IsToDeleteImage { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public long OrganizationRoleId { get; set; }
    }
}
