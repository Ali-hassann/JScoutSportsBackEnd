using AMNSystemsERP.CL.Models.Commons.Pagination;

namespace AMNSystemsERP.CL.Models.AccountModels.Vouchers
{
    public class VoucherListRequest : PaginationRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int VoucherTypeId { get; set; }
        public int PostingStatus{ get; set; }
        public string CreatedBy { get; set; }
    }
}