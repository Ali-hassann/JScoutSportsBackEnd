using AMNSystemsERP.BL.Repositories.Production;
using AMNSystemsERP.CL.Models.InventoryModels;
using AMNSystemsERP.CL.Models.ProductionModels;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/Production")]
    public class ProductionController : ApiController
    {
        private readonly IProductionService _productionService;

        public ProductionController(IProductionService ProductionService)
        {
            _productionService = ProductionService;
        }

        // ------------------ Production -----------------------------

        #region ProductCategories
        [HttpPost]
        [Route("AddProductCategory")]
        public async Task<ProductCategoryRequest> AddProductCategory([FromBody] ProductCategoryRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && !string.IsNullOrEmpty(request.ProductCategoryName))
                {
                    return await _productionService.AddProductCategory(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateProductCategory")]
        public async Task<ProductCategoryRequest> UpdateProductCategory([FromBody] ProductCategoryRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.ProductCategoryId > 0
                    && !string.IsNullOrEmpty(request.ProductCategoryName))
                {
                    return await _productionService.UpdateProductCategory(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveProductCategory")]
        public async Task<bool> RemoveProductCategory(long productCategoryId)
        {
            try
            {
                if (productCategoryId > 0)
                {
                    return await _productionService.RemoveProductCategory(productCategoryId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetProductCategoryList")]
        public async Task<List<ProductCategoryRequest>> GetProductCategoryList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _productionService.GetProductCategoryList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region ProcessType
        [HttpPost]
        [Route("AddProcessType")]
        public async Task<ProcessTypeRequest> AddProcessType([FromBody] ProcessTypeRequest request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.ProcessTypeName))
                {
                    return await _productionService.AddProcessType(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateProcessType")]
        public async Task<ProcessTypeRequest> UpdateProcessType([FromBody] ProcessTypeRequest request)
        {
            try
            {
                if (request?.ProcessTypeId > 0
                    && !string.IsNullOrEmpty(request.ProcessTypeName))
                {
                    return await _productionService.UpdateProcessType(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveProcessType")]
        public async Task<bool> RemoveProcessType(int processTypeId)
        {
            try
            {
                if (processTypeId > 0)
                {
                    return await _productionService.RemoveProcessType(processTypeId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetProcessTypeList")]
        public async Task<List<ProcessTypeRequest>> GetProcessTypeList()
        {
            try
            {
                return await _productionService.GetProcessTypeList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ProductSizes
        [HttpPost]
        [Route("AddProductSize")]
        public async Task<ProductSizeRequest> AddProductSize([FromBody] ProductSizeRequest request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request?.ProductSizeName))
                {
                    return await _productionService.AddProductSize(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateProductSize")]
        public async Task<ProductSizeRequest> UpdateProductSize([FromBody] ProductSizeRequest request)
        {
            try
            {
                if (request?.ProductSizeId > 0
                    && !string.IsNullOrEmpty(request.ProductSizeName))
                {
                    return await _productionService.UpdateProductSize(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveProductSize")]
        public async Task<bool> RemoveProductSize(int productSizeId)
        {
            try
            {
                if (productSizeId > 0)
                {
                    return await _productionService.RemoveProductSize(productSizeId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetProductSizeList")]
        public async Task<List<ProductSizeRequest>> GetProductSizeList()
        {
            try
            {
                return await _productionService.GetProductSizeList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Process
        [HttpPost]
        [Route("SaveProcess")]
        public async Task<bool> SaveProcess([FromBody] List<ProcessRequest> request)
        {
            try
            {
                if (request?.Count > 0)
                {
                    return await _productionService.SaveProcess(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("TransferProcess")]
        public async Task<bool> TransferProcess(long fromProductId, [FromBody] List<long> toProductIds)
        {
            try
            {
                if (fromProductId > 0
                    && toProductIds.Count > 0)
                {
                    return await _productionService.TransferProcess(fromProductId, toProductIds);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("RemoveProcess")]
        public async Task<bool> RemoveProcess(int processId)
        {
            try
            {
                if (processId > 0)
                {
                    return await _productionService.RemoveProcess(processId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetProcessListForStore")]
        public async Task<List<ProcessRequest>> GetProcessListForStore()
        {
            try
            {
                return await _productionService.GetProcessListForStore();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetProcessList")]
        public async Task<List<ProcessRequest>> GetProcessList()
        {
            try
            {
                return await _productionService.GetProcessList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("GetProcessListByProduct")]
        public async Task<List<ProcessRequest>> GetProcessListByProduct([FromBody] ProductionFilterRequest request)
        {
            try
            {
                if (request.ProductId > 0)
                {
                    return await _productionService.GetProcessListByProduct(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region PlaningMaster
        [HttpPost]
        [Route("SavePlaningMaster")]
        public async Task<bool> SavePlaningMaster([FromBody] List<PlaningMasterRequest> requestList)
        {
            try
            {
                if (requestList?.Count > 0)
                {
                    return await _productionService.SavePlaningMaster(requestList);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("RemovePlaningMaster")]
        public async Task<bool> RemovePlaningMaster(long planingMasterId)
        {
            try
            {
                if (planingMasterId > 0)
                {
                    return await _productionService.RemovePlaningMaster(planingMasterId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetPlaningMasterList")]
        public async Task<List<PlaningMasterRequest>> GetPlaningMasterList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _productionService.GetPlaningMasterList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetPlaningDetailById")]
        public async Task<List<ItemRequest>> GetPlaningDetailById([FromBody] ProductionFilterRequest request)
        {
            try
            {
                if (request.OrderMasterId > 0)
                {
                    return await _productionService.GetPlaningDetailById(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region ProductionProcess
        [HttpPost]
        [Route("SaveProductionProcess")]
        public async Task<bool> SaveProductionProcess([FromBody] List<ProductionProcessRequest> request)
        {
            try
            {
                if (request?.Count > 0)
                {
                    return await _productionService.SaveProductionProcess(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("SaveReceiveProcessList")]
        public async Task<bool> SaveReceiveProcessList([FromBody] List<ProductionProcessRequest> request)
        {
            try
            {
                if (request?.Count > 0)
                {
                    return await _productionService.SaveReceiveProcessList(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("GetReceiveListByOrder")]
        public async Task<List<ProductionProcessRequest>> GetReceiveListByOrder([FromBody] ProductionParameterRequest request)
        {
            try
            {
                if (request?.OrderMasterId > 0
                    && request.EmployeeId > 0
                    && request.OutletId > 0)
                {
                    return await _productionService.GetReceiveListByOrder(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveProductionProcess")]
        public async Task<bool> RemoveProductionProcess(long productionProcessId)
        {
            try
            {
                if (productionProcessId > 0)
                {
                    return await _productionService.RemoveProductionProcess(productionProcessId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("RemoveProductionProcessByIssueNo")]
        public async Task<bool> RemoveProductionProcessByIssueNo(int issueNo)
        {
            try
            {
                if (issueNo > 0)
                {
                    return await _productionService.RemoveProductionProcessByIssueNo(issueNo);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("GetProductionProcessList")]
        public async Task<List<ProductionProcessRequest>> GetProductionProcessList([FromBody] ProductionParameterRequest request)
        {
            try
            {
                if (request?.OrderMasterId > 0
                    && request.EmployeeId > 0
                    && request.OutletId > 0)
                {
                    return await _productionService.GetProductionProcessList(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetProcessListByIssueNo")]
        public async Task<List<ProductionProcessRequest>> GetProcessListByIssueNo(int issueNo)
        {
            try
            {
                if (issueNo > 0)
                {
                    return await _productionService.GetProcessListByIssueNo(issueNo);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetReceivingProcessList")]
        public async Task<List<ProductionProcessRequest>> GetReceivingProcessList([FromBody] ProductionParameterRequest request)
        {
            try
            {
                if (request?.OrderMasterId > 0
                    && request.EmployeeId > 0
                    && request.OutletId > 0)
                {
                    return await _productionService.GetReceivingProcessList(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetIssuanceListByOrder")]
        public async Task<List<ProductionProcessRequest>> GetIssuanceListByOrder([FromBody] ProductionParameterRequest request)
        {
            try
            {
                if (request?.OrderMasterId > 0
                    && request.OutletId > 0)
                {
                    return await _productionService.GetIssuanceListByOrder(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region Product
        [HttpPost]
        [Route("AddProduct")]
        public async Task<ProductRequest> AddProduct([FromBody] ProductRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.ProductCategoryId > 0
                    && !string.IsNullOrEmpty(request.ProductName))
                {
                    return await _productionService.AddProduct(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateProduct")]
        public async Task<ProductRequest> UpdateProduct([FromBody] ProductRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.ProductCategoryId > 0
                    && request.UnitId > 0
                    && !string.IsNullOrEmpty(request.ProductName))
                {
                    return await _productionService.UpdateProduct(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveProduct")]
        public async Task<bool> RemoveProduct(long productId)
        {
            try
            {
                if (productId > 0)
                {
                    return await _productionService.RemoveProduct(productId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetProductList")]
        public async Task<List<ProductRequest>> GetProductList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _productionService.GetProductList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        // ------------------ Orders -----------------------------

        #region Orders

        [HttpPost]
        [Route("AddOrder")]
        public async Task<OrderMasterRequest> AddOrder([FromBody] OrderMasterRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.OrderDate != DateTime.MinValue
                    && request.OrderDate != DateTime.MaxValue
                    && request.OrderDetailsRequest?.Count > 0)
                {
                    return await _productionService.AddOrder(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateOrder")]
        public async Task<OrderMasterRequest> UpdateOrder([FromBody] OrderMasterRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                     && request.OrderDate != DateTime.MinValue
                     && request.OrderDate != DateTime.MaxValue
                     && request.OrderDetailsRequest?.Count > 0)
                {
                    return await _productionService.UpdateOrder(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetOrderDetailById")]
        public async Task<List<OrderDetailRequest>> GetOrderDetailById(long orderMasterId)
        {
            try
            {
                if (orderMasterId > 0)
                {
                    return await _productionService.GetOrderDetailById(orderMasterId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetOrderList")]
        public async Task<List<OrderMasterRequest>> GetOrderList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _productionService.GetOrderList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveOrder")]
        public async Task<bool> RemoveOrder(long orderMasterId)
        {
            try
            {
                if (orderMasterId > 0)
                {
                    return await _productionService.RemoveOrder(orderMasterId);
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
        public async Task<bool> PostMultipleOrders([FromBody] List<long> orderMasterIds, short orderStatus)
        {
            try
            {
                if (orderMasterIds?.Count > 0 && orderStatus > 0)
                {
                    return await _productionService.PostMultipleOrders(orderMasterIds, orderStatus);
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