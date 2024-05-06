namespace AMNSystemsERP.CL.Models.IdentityModels
{
    public class UserRightsRequest
    {
        public long UserRightsId { get; set; }
        public long RightsId { get; set; }
        public string Id { get; set; }
        public bool HasAccess { get; set; }
    }

    public class UserRightsBaseResponse
    {
        public long RightsId { get; set; }
        public string RightsName { get; set; }
        public string Description { get; set; }
        public string RightsArea { get; set; }
        public string ParentName { get; set; }
        public long UserRightsId { get; set; }
        public string Id { get; set; }
        public bool HasMenuRights { get; set; }
        public bool HasSubMenuRights { get; set; }
        public bool HasAccess { get; set; }
    }

    public class SubMenuRightsResponse
    {
        public bool SubMenuSelectAll { get; set; }
        public string SubMenuRightsArea { get; set; }
        public UserRightsBaseResponse SubMenuRight { get; set; }
        public List<UserRightsBaseResponse> SubMenuRightsList { get; set; }
    }

    public class UserRightsResponse
    {
        public bool MenuSelectAll { get; set; }
        public string MenuRightsArea { get; set; }
        public UserRightsBaseResponse MenuRight { get; set; }
        public List<SubMenuRightsResponse> MenuRightsList { get; set; }
    }
}