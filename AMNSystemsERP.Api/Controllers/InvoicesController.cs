using AMNSystemsERP.BL.Repositories.StockManagement;
using AMNSystemsERP.CL.Models.Commons.Pagination;
using AMNSystemsERP.CL.Models.StockManagementModels;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/Invoices")]
    public class InvoicesController : ApiController
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // ------------------ Invoices -----------------------------

        #region Invoices

        [HttpPost]
        [Route("AddInvoice")]
        public async Task<InvoiceMasterRequest> AddInvoice([FromBody] InvoiceMasterRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.DocumentTypeId > 0
                    && request.InvoiceDate != DateTime.MinValue
                    && request.InvoiceDate != DateTime.MaxValue
                    && request.InvoiceDetailsRequest?.Count > 0)
                {
                    return await _invoiceService.AddInvoice(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateInvoice")]
        public async Task<InvoiceMasterRequest> UpdateInvoice([FromBody] InvoiceMasterRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                     && request.DocumentTypeId > 0
                     && request.InvoiceDate != DateTime.MinValue
                     && request.InvoiceDate != DateTime.MaxValue
                     && request.InvoiceDetailsRequest?.Count > 0)
                {
                    return await _invoiceService.UpdateInvoice(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetInvoiceDetailById")]
        public async Task<List<InvoiceDetailRequest>> GetInvoiceDetailById(long invoiceMasterId)
        {
            try
            {
                if (invoiceMasterId > 0)
                {
                    return await _invoiceService.GetInvoiceDetailById(invoiceMasterId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetInvoiceListWithPagination")]
        public async Task<PaginationResponse<InvoiceMasterRequest>> GetInvoiceListWithPagination([FromBody] InvoiceParameterRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && !string.IsNullOrEmpty(request.FromDate)
                    && !string.IsNullOrEmpty(request.ToDate))
                {
                    return await _invoiceService.GetInvoiceListWithPagination(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveInvoice")]
        public async Task<bool> RemoveInvoice(long invoiceMasterId)
        {
            try
            {
                if (invoiceMasterId > 0)
                {
                    return await _invoiceService.RemoveInvoice(invoiceMasterId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("PostInvoice")]
        public async Task<bool> PostInvoice([FromBody] InvoiceMasterRequest request)
        {
            try
            {
                if (request?.InvoiceMasterId > 0)
                {
                    return await _invoiceService.PostInvoice(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion
    }
}