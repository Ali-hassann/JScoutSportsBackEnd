using AMNSystemsERP.CL.Models.OrganizationModels;

namespace AMNSystemsERP.CL.Models.Commons.Base
{
    public class SuccessErrorResponse<T> : BaseResponse
    {
        public List<T> Data { get; set; }
    }
}