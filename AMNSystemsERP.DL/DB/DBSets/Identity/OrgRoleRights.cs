namespace AMNSystemsERP.DL.DB.DBSets.Identity
{
    public partial class OrgRoleRights
    {
        public long OrgRoleRightsId { get; set; }
        public long RightsId { get; set; }
        public long OrganizationRoleId { get; set; }
        public long OrganizationId { get; set; }

        public virtual OrganizationRole OrganizationRole { get; set; }
    }
}