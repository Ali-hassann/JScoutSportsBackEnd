using AMNSystemsERP.CL.Models.Commons.Pagination;
using AMNSystemsERP.CL.Models.StockManagementModels;

namespace AMNSystemsERP.BL.Repositories.StockManagement
{
    public interface IPurchaseOrderService
    {
        // ----------------------------------------------------------------------------
        // ------------------ Stock management ----------------------------------------
        // ----------------------------------------------------------------------------
        Task<PurchaseOrderMasterRequest> AddPurchaseOrder(PurchaseOrderMasterRequest request);
        Task<PurchaseOrderMasterRequest> UpdatePurchaseOrder(PurchaseOrderMasterRequest request);
        Task<List<PurchaseOrderDetailRequest>> GetPurchaseOrderDetailById(long purchaseOrderMasterId);
        Task<PaginationResponse<PurchaseOrderMasterRequest>> GetPurchaseOrderList(InvoiceParameterRequest request);
        Task<bool> RemovePurchaseOrder(long purchaseOrderMasterId);
        Task<bool> PostMultipleOrders(List<long> purchaseOrderMasterIds, short status);
    }
}