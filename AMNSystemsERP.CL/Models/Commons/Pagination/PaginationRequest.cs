using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.Commons.Pagination
{
    public class PaginationRequest : PaginationBase, IOutletBaseModel
    {
        public string SearchQuery { get; set; }
        public string OutletName { get; set; }
        public string OutletImagePath { get; set; }
        public string Address { get; set; }
        public long OutletId { get; set; }
        public long OrganizationId { get; set; }
    }
}