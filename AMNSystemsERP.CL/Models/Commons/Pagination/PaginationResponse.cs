namespace AMNSystemsERP.CL.Models.Commons.Pagination
{
    public class PaginationResponse<A> : PaginationBase
    {              
        public List<A> Data { get; set; }        
    }
}