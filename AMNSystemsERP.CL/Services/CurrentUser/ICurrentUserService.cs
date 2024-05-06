namespace AMNSystemsERP.CL.Services.CurrentLogin
{
    public interface ICurrentLoginService
    {
        string UserName { get; set; }
        long OrganizationId { get; set; }
        long OutletId { get; set; }
        string OutletName { get; set; }
        string OutletImage { get; set; }
        string Address { get; set; }
    }
}
