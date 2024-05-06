using AMNSystemsERP.CL.Models.Commons.Pagination;
using AMNSystemsERP.CL.Models.StockManagementModels;

namespace AMNSystemsERP.BL.Repositories.StockManagement
{
    public interface IInvoiceService
    {
        // ----------------------------------------------------------------------------
        // ------------------ Stock management ----------------------------------------
        // ----------------------------------------------------------------------------
        Task<InvoiceMasterRequest> AddInvoice(InvoiceMasterRequest request);
        Task<InvoiceMasterRequest> UpdateInvoice(InvoiceMasterRequest request);
        Task<List<InvoiceDetailRequest>> GetInvoiceDetailById(long invoiceMasterId);
        Task<PaginationResponse<InvoiceMasterRequest>> GetInvoiceListWithPagination(InvoiceParameterRequest request);
        Task<bool> RemoveInvoice(long invoiceMasterId);
        Task<bool> PostInvoice(InvoiceMasterRequest request);
    }
}