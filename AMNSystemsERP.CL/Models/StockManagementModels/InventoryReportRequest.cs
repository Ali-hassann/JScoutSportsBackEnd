namespace AMNSystemsERP.CL.Models.StockManagementModels
{
    public class InventoryReportRequest
    {
        public long OutletId { get; set; }
        public long OrganizationId { get; set; }
        public string OutletName { get; set; }
        public string OutletImagePath { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int DocumentTypeId { get; set; }
        public string ItemIds { get; set; }
        public long ParticularId { get; set; }
        public string VendorIds { get; set; }
        public string CustomersIds { get; set; }
        public string PersonIds { get; set; }
        public int DocumentCode { get; set; }
        public bool IncludeZeroValues { get; set; }
        public string OutletIds { get; set; }
    }
}
