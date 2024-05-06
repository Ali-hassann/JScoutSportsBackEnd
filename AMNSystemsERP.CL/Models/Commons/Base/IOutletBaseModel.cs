namespace AMNSystemsERP.CL.Models.Commons.Base
{
    public interface IOutletBaseModel
    {
        public string OutletName { get; set; }
        public string OutletImagePath { get; set; }
        public long OutletId { get; set; }
        public long OrganizationId { get; set; }
    }
}