namespace AMNSystemsERP.CL.Models.AccountModels.ChartOfAccounts
{
    public class ChartOfAccountsMigrationRequest
    {
        public long FromOutletId { get; set; }
        public long ToOutletId { get; set; }
        public long OrganizationId { get; set; }
    }
}