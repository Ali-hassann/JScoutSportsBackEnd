using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.IdentityModels
{
    public class JwtTokenResponse : CommonBaseModel
    {
        public string UserName { get; set; }
        public string Secret { get; set; }
    }
}
