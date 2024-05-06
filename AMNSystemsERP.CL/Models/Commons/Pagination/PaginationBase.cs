namespace AMNSystemsERP.CL.Models.Commons.Pagination
{
    public class PaginationBase : IPaginationBase
    {
        public int PageNumber { get; set; }
        public int RecordsPerPage { get; set; }
        public string Status { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }
}