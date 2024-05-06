namespace AMNSystemsERP.CL.Models.IdentityModels
{
    public class OrganizationRoleRequest 
    {
        public long OrganizationRoleId { get; set; }
        public string RoleName { get; set; }
        public string NormalizedName { get; set; }
        public long OrganizationId { get; set; }
        public bool IsDefault { get; set; }
        public int RightsCount { get; set; }
    }
}