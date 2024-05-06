using AMNSystemsERP.CL.Models.InventoryModels;
using AMNSystemsERP.CL.Models.ProductionModels;

namespace AMNSystemsERP.BL.Repositories.Production
{
    public interface IProductionService
    {
        // ----------------------------------------------------------------------------
        // ------------------------------- Production ----------------------------------
        // ----------------------------------------------------------------------------

        #region  Categories
        Task<ProductCategoryRequest> AddProductCategory(ProductCategoryRequest request);
        Task<ProductCategoryRequest> UpdateProductCategory(ProductCategoryRequest request);
        Task<bool> RemoveProductCategory(long id);
        Task<List<ProductCategoryRequest>> GetProductCategoryList(long outletId);
        #endregion

        #region ProcessType
        Task<ProcessTypeRequest> AddProcessType(ProcessTypeRequest request);
        Task<ProcessTypeRequest> UpdateProcessType(ProcessTypeRequest request);
        Task<bool> RemoveProcessType(int processTypeId);
        Task<List<ProcessTypeRequest>> GetProcessTypeList();
        #endregion

        #region ProductSize
        Task<ProductSizeRequest> AddProductSize(ProductSizeRequest request);
        Task<ProductSizeRequest> UpdateProductSize(ProductSizeRequest request);
        Task<bool> RemoveProductSize(int productSizeId);
        Task<List<ProductSizeRequest>> GetProductSizeList();
        #endregion

        #region Process
        Task<bool> SaveProcess(List<ProcessRequest> request);
        Task<bool> TransferProcess(long fromProductId, List<long> toProductIds);
        Task<bool> RemoveProcess(int processId);
        Task<List<ProcessRequest>> GetProcessListForStore();
        Task<List<ProcessRequest>> GetProcessList();
        Task<List<ProcessRequest>> GetProcessListByProduct(ProductionFilterRequest request);
        #endregion

        #region PlaningMaster
        Task<bool> SavePlaningMaster(List<PlaningMasterRequest> requestList);
        Task<bool> RemovePlaningMaster(long planingMasterId);
        Task<List<ItemRequest>> GetPlaningDetailById(ProductionFilterRequest request);
        Task<List<PlaningMasterRequest>> GetPlaningMasterList(long outletId);
        #endregion

        #region ProductionMaster
        Task<bool> SaveProductionProcess(List<ProductionProcessRequest> request);
        Task<bool> SaveReceiveProcessList(List<ProductionProcessRequest> request);
        Task<List<ProductionProcessRequest>> GetReceiveListByOrder(ProductionParameterRequest request);
        Task<bool> RemoveProductionProcess(long productionMasterId);
        Task<bool> RemoveProductionProcessByIssueNo(int issueNo);
        Task<List<ProductionProcessRequest>> GetProductionProcessList(ProductionParameterRequest request);
        Task<List<ProductionProcessRequest>> GetProcessListByIssueNo(int issueNo);
        Task<List<ProductionProcessRequest>> GetReceivingProcessList(ProductionParameterRequest request);
        Task<List<ProductionProcessRequest>> GetIssuanceListByOrder(ProductionParameterRequest request);
        #endregion

        #region Product
        Task<ProductRequest> AddProduct(ProductRequest request);
        Task<ProductRequest> UpdateProduct(ProductRequest request);
        Task<bool> RemoveProduct(long id);
        Task<List<ProductRequest>> GetProductList(long outletId);
        Task<ProductRequest> GetProductById(long id);
        #endregion

        #region Production
        Task<OrderMasterRequest> AddOrder(OrderMasterRequest request);
        Task<OrderMasterRequest> UpdateOrder(OrderMasterRequest request);
        Task<List<OrderDetailRequest>> GetOrderDetailById(long orderMasterId);
        Task<List<OrderMasterRequest>> GetOrderList(long outletId);
        Task<bool> RemoveOrder(long orderMasterId);
        Task<bool> PostMultipleOrders(List<long> orderMasterIds, short orderStatus);
        #endregion
    }
}