using AMNSystemsERP.CL.Models.OrganizationModels;

namespace AMNSystemsERP.CL.Models.IdentityModels
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public long OutletId { get; set; }
        public long OrganizationId { get; set; }
        public string Since { get; set; }
        public string RoleName { get; set; }
        public string Token { get; set; }
        public string ImagePath { get; set; }
        public long CurrentOutletId { get; set; }
        public OrganizationProfile OrganizationProfile { get; set; }
    }
}