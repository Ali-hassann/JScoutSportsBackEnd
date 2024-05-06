using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Identity
{
    public class OrganizationRole
    {
        public OrganizationRole()
        {
            ApplicationUsers = new HashSet<ApplicationUser>();
            OrgRoleRights = new HashSet<OrgRoleRights>();
        }

        [Key]
        public long OrganizationRoleId { get; set; }
        [Required]
        [MaxLength(256)]
        public string RoleName { get; set; }
        [Required]
        [MaxLength(256)]
        public string NormalizedName { get; set; }
        [Required]
        public long OrganizationId { get; set; }
        [Required]
        public bool IsDefault { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<OrgRoleRights> OrgRoleRights { get; set; }
    }
}