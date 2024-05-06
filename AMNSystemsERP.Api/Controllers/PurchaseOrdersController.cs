using AMNSystemsERP.BL.Repositories.StockManagement;
using AMNSystemsERP.CL.Models.Commons.Pagination;
using AMNSystemsERP.CL.Models.StockManagementModels;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/PurchaseOrders")]
    public class PurchaseOrdersController : ApiController
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        // ------------------ Purchase Orders -----------------------------

        #region Order

        [HttpPost]
        [Route("AddPurchaseOrder")]
        public async Task<PurchaseOrderMasterRequest> AddPurchaseOrder([FromBody] PurchaseOrderMasterRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.PurchaseOrderDate != DateTime.MinValue
                    && request.PurchaseOrderDate != DateTime.MaxValue
                    && request.PurchaseOrderDetailRequest?.Count > 0)
                {
                    return await _purchaseOrderService.AddPurchaseOrder(request);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        [HttpPost]
        [Route("UpdatePurchaseOrder")]
        public async Task<PurchaseOrderMasterRequest> UpdatePurchaseOrder([FromBody] PurchaseOrderMasterRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.PurchaseOrderDate != DateTime.MinValue
                    && request.PurchaseOrderDate != DateTime.MaxValue
                    && request.PurchaseOrderDetailRequest?.Count > 0)
                {
                    return await _purchaseOrderService.UpdatePurchaseOrder(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetPurchaseOrderById")]
        public async Task<List<PurchaseOrderDetailRequest>> GetPurchaseOrderById(long purchaseOrderMasterId)
        {
            try
            {
                if (purchaseOrderMasterId > 0)
                {
                    return await _purchaseOrderService.GetPurchaseOrderDetailById(purchaseOrderMasterId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetPurchaseOrderList")]
        public async Task<PaginationResponse<PurchaseOrderMasterRequest>> GetPurchaseOrderList([FromBody] InvoiceParameterRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && !string.IsNullOrEmpty(request.FromDate)
                    && !string.IsNullOrEmpty(request.ToDate))
                {
                    return await _purchaseOrderService.GetPurchaseOrderList(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemovePurchaseOrder")]
        public async Task<bool> RemovePurchaseOrder(long purchaseOrderMasterId)
        {
            try
            {
                if (purchaseOrderMasterId > 0)
                {
                    return await _purchaseOrderService.RemovePurchaseOrder(purchaseOrderMasterId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("PostMultipleOrders")]
        public async Task<bool> PostMultipleOrders([FromBody] List<long> purchaseOrderMasterIds, short status)
        {
            try
            {
                if (purchaseOrderMasterIds?.Count > 0)
                {
                    return await _purchaseOrderService.PostMultipleOrders(purchaseOrderMasterIds, status);
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