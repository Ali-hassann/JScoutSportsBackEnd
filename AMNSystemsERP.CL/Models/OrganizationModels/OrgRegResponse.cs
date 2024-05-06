namespace AMNSystemsERP.CL.Models.OrganizationModels
{
    public class BaseResponse
    {
        public readonly static string RequestError = "Invalid Request Data";
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
