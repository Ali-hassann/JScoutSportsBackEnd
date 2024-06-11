using AMNSystemsERP.BL.Repositories.Inventory;
using AMNSystemsERP.BL.Repositories.StockManagement;
using AMNSystemsERP.CL.Models.Commons.Pagination;
using AMNSystemsERP.CL.Models.InventoryModels;
using AMNSystemsERP.CL.Models.StockManagementModels;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/PurchaseRequisitions")]
    public class PurchaseRequisitionsController : ApiController
    {
        private readonly IPurchaseRequisitionService _purchaseRequisitionService;

        public PurchaseRequisitionsController(IPurchaseRequisitionService purchaseRequisitionService)
        {
            _purchaseRequisitionService = purchaseRequisitionService;
        }

        // ------------------ Purchase Requisitions -----------------------------

        #region Requisition

        [HttpPost]
        [Route("AddPurchaseRequisition")]
        public async Task<PurchaseRequisitionMasterRequest> AddPurchaseRequisition([FromBody] PurchaseRequisitionMasterRequest request)
        {
            try
            {
                if (request?.PurchaseRequisitionDate != DateTime.MinValue
                    && request.PurchaseRequisitionDate != DateTime.MaxValue
                    && request.PurchaseRequisitionDetailRequest?.Count > 0)
                {
                    return await _purchaseRequisitionService.AddPurchaseRequisition(request);
                }
            }

            catch (Exception)
            {

                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdatePurchaseRequisition")]
        public async Task<PurchaseRequisitionMasterRequest> UpdatePurchaseRequisition([FromBody] PurchaseRequisitionMasterRequest request)
        {
            try
            {
                if (request?.PurchaseRequisitionDate != DateTime.MinValue
                    && request.PurchaseRequisitionDate != DateTime.MaxValue
                    && request.PurchaseRequisitionDetailRequest?.Count > 0)
                {
                    return await _purchaseRequisitionService.UpdatePurchaseRequisition(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetPurchaseRequisitionById")]
        public async Task<PurchaseRequisitionMasterRequest> GetPurchaseRequisitionById(long purchaseRequisitionMasterId)
        {
            try
            {
                if (purchaseRequisitionMasterId > 0)
                {
                    return await _purchaseRequisitionService.GetPurchaseRequisitionById(purchaseRequisitionMasterId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetPurchaseRequisitionListWithPagination")]
        public async Task<PaginationResponse<PurchaseRequisitionMasterRequest>> GetPurchaseRequisitionListWithPagination([FromBody] InvoiceParameterRequest request)
        {
            try
            {
                if (request?.OrganizationId > 0
                    && !string.IsNullOrEmpty(request.FromDate)
                    && !string.IsNullOrEmpty(request.ToDate))
                {
                    return await _purchaseRequisitionService.GetPurchaseRequisitionListWithPagination(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemovePurchaseRequisition")]
        public async Task<bool> RemovePurchaseRequisition(long purchaseRequisitionMasterId)
        {
            try
            {
                if (purchaseRequisitionMasterId > 0)
                {
                    return await _purchaseRequisitionService.RemovePurchaseRequisition(purchaseRequisitionMasterId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("PostMultipleRequisitions")]
        public async Task<bool> PostMultipleRequisitions([FromBody] List<long> reqIds, short reqStatus)
        {
            try
            {
                if (reqIds?.Count > 0)
                {
                    return await _purchaseRequisitionService.PostMultipleRequisitions(reqIds, reqStatus);
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