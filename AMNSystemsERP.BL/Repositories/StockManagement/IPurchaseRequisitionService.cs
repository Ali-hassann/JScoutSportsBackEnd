using AMNSystemsERP.CL.Models.Commons.Pagination;
using AMNSystemsERP.CL.Models.StockManagementModels;

namespace AMNSystemsERP.BL.Repositories.StockManagement
{
    public interface IPurchaseRequisitionService
    {
        // ----------------------------------------------------------------------------
        // ------------------ Stock management ----------------------------------------
        // ----------------------------------------------------------------------------
        Task<PurchaseRequisitionMasterRequest> AddPurchaseRequisition(PurchaseRequisitionMasterRequest request);
        Task<PurchaseRequisitionMasterRequest> UpdatePurchaseRequisition(PurchaseRequisitionMasterRequest request);
        Task<PurchaseRequisitionMasterRequest> GetPurchaseRequisitionById(long purchaseRequisitionMasterId);
        Task<PaginationResponse<PurchaseRequisitionMasterRequest>> GetPurchaseRequisitionListWithPagination(InvoiceParameterRequest request);
        Task<bool> RemovePurchaseRequisition(long requisitionMasterId);
        Task<bool> PostMultipleRequisitions(List<long> reqIds, short reqStatus);
        Task<PaginationResponse<PurchaseRequisitionMasterRequest>> GetPurchaseRequisitionListByOrganization(InvoiceParameterRequest request);
    }
}