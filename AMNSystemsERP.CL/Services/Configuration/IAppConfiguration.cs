namespace AMNSystemsERP.CL.Services.Configuration
{
    public interface IAppConfiguration
    {
        string Secret { get; set; }
        string SupportEmail { get; set; }
    }
}
