namespace AMNSystemsERP.CL.Models.IdentityModels
{
    public class OrgRoleRightsRequest
    {
        public long OrgRoleRightsId { get; set; }
        public long RightsId { get; set; }
        public long OrganizationRoleId { get; set; }
        public long OrganizationId { get; set; }
        public string RoleName { get; set; }
        public string RightsName { get; set; }
    }

    public class OrgRoleRightsBaseResponse : OrgRoleRightsRequest
    {
        public string Description { get; set; }
        public string RightsArea { get; set; }
        public string ParentName { get; set; }
        public bool HasMenuRights { get; set; }
        public bool HasSubMenuRights { get; set; }
        public bool HasAccess { get; set; }
    }

    public class SubMenuOrgRoleRightsResponse
    {
        public bool SubMenuSelectAll { get; set; }
        public string SubMenuRightsArea { get; set; }
        public OrgRoleRightsBaseResponse SubMenuRight { get; set; }
        public List<OrgRoleRightsBaseResponse> SubMenuRightsList { get; set; }
    }

    public class OrgRoleRightsResponse
    {
        public bool MenuSelectAll { get; set; }
        public string MenuRightsArea { get; set; }
        public OrgRoleRightsBaseResponse MenuRight { get; set; }
        public List<SubMenuOrgRoleRightsResponse> MenuRightsList { get; set; }
    }
}