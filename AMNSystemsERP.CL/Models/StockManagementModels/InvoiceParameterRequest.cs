using AMNSystemsERP.CL.Models.Commons.Pagination;

namespace AMNSystemsERP.CL.Models.StockManagementModels
{
    public class InvoiceParameterRequest : PaginationRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int DocumentTypeId { get; set; }
        public string ItemIds { get; set; }
        public long ParticularId { get; set; }
        public long OrderMasterId { get;set; }
        public string VendorIds { get; set; }
        public string CustomersIds { get; set; }
        public int DocumentCode { get; set; }
        public bool IsIncludeZeroValue { get; set; }
    }
}