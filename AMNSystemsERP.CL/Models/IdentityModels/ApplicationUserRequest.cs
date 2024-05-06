using Microsoft.AspNetCore.Identity;

namespace AMNSystemsERP.CL.Models.IdentityModels
{
    public class ApplicationUserRequest : IdentityUser
    {
        public long OutletId { get; set; }
        public long OrganizationId { get; set; }
        public string Password { get; set; }
        public string RoleDescription { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ImagePath { get; set; }
        public long OrganizationRoleId { get; set; }
        public bool IsToDeleteImage { get; set; }
        public bool IsActive { get; set; }

        public bool Succeeded { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}