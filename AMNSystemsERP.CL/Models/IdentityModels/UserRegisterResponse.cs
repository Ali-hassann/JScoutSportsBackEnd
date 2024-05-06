using AMNSystemsERP.CL.Models.OrganizationModels;

namespace AMNSystemsERP.CL.Models.IdentityModels
{
    public class UserRegisterResponse : BaseResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string OutletName { get; set; }
        public long OutletId { get; set; }
        public long OrganizationId { get; set; }
        public long OrganizationRoleId { get; set; }
        public string Since { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool LockoutEnabled { get; set; }
        public string ImagePath { get; set; }
    }
}
