using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.StockManagementModels
{
    public class LedgerParameterRequest : CommonBaseModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int DocumentTypeId { get; set; }
        public string ItemIds { get; set; }
        public long ParticularId { get; set; }
        public string VendorIds { get; set; }
        public string CustomersIds { get; set; }
    }
}