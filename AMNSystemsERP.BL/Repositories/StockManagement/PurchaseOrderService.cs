using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.Commons.Pagination;
using AMNSystemsERP.CL.Models.StockManagementModels;
using AutoMapper;
using Inventory.DL.DB.DBSets.StockManagement;
using System.Data;

namespace AMNSystemsERP.BL.Repositories.StockManagement
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public PurchaseOrderService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        // ------------------ Purchase Order Services--------------------------------------        

        public async Task<PurchaseOrderMasterRequest> AddPurchaseOrder(PurchaseOrderMasterRequest request)
        {
            try
            {
                var purchaseOrderMaster = _mapper.Map<PurchaseOrderMaster>(request);

                request
                    .PurchaseOrderDetailRequest
                    .ForEach(OrderDetail =>
                    {
                        var detail = _mapper.Map<PurchaseOrderDetail>(OrderDetail);

                        purchaseOrderMaster.PurchaseOrderDetail.Add(detail);
                    });
                _unit.PurchaseOrderMasterRepository.InsertSingle(purchaseOrderMaster);

                if (await _unit.SaveAsync())
                {
                    request.PurchaseOrderMasterId = purchaseOrderMaster.PurchaseOrderMasterId;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new PurchaseOrderMasterRequest();
        }

        public async Task<PurchaseOrderMasterRequest> UpdatePurchaseOrder(PurchaseOrderMasterRequest request)
        {
            try
            {
                var purchaseOrderMaster = _mapper.Map<PurchaseOrderMaster>(request);

                var dBInvoiceDetails = await GetPurchaseOrderDetailById(request.PurchaseOrderMasterId);
                if (dBInvoiceDetails?.Count > 0)
                {
                    /*Deleting existing line items against this req.No*/
                    dBInvoiceDetails?.ForEach(detail =>
                    {
                        if (detail?.PurchaseOrderDetailId > 0)
                        {
                            _unit.PurchaseOrderDetailRepository.DeleteById(detail.PurchaseOrderDetailId);
                        }
                    });
                }

                /*Adding new line items ...*/
                request.PurchaseOrderDetailRequest.ForEach(reqDetail =>
                {
                    var detail = _mapper.Map<PurchaseOrderDetail>(reqDetail);
                    if (detail?.PurchaseOrderMasterId > 0)
                    {
                        detail.PurchaseOrderDetailId = 0;
                        purchaseOrderMaster.PurchaseOrderDetail.Add(detail);
                    }
                });

                _unit.PurchaseOrderMasterRepository.Update(purchaseOrderMaster);

                return await _unit.SaveAsync() ? request : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PurchaseOrderDetailRequest>> GetPurchaseOrderDetailById(long purchaseOrderMasterId)
        {
            try
            {
                var query = @$"
                                SELECT   
                                 PD.PurchaseOrderDetailId  
                                 , PD.PurchaseOrderMasterId  
                                 , PD.Quantity  
                                 , PD.BarCode  
                                 , PD.ItemId  
                                 , PD.Price  
                                 , PD.Amount  
                                 , I.ItemName  
                                 , U.UnitName  
                                FROM PurchaseOrderMaster AS PO  
                                INNER JOIN PurchaseOrderDetail AS PD  
                                 ON PD.PurchaseOrderMasterId = PO.PurchaseOrderMasterId  
                                 AND PD.PurchaseOrderMasterId = {purchaseOrderMasterId}  
                                INNER JOIN Item AS I  
                                 ON I.ItemId = PD.ItemId  
                                INNER JOIN Unit AS U  
                                 ON U.UnitId = I.UnitId  
                                WHERE PO.PurchaseOrderMasterId = {purchaseOrderMasterId}";

                return await _unit
                             .DapperRepository
                             .GetListQueryAsync<PurchaseOrderDetailRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginationResponse<PurchaseOrderMasterRequest>> GetPurchaseOrderList(InvoiceParameterRequest request)
        {
            try
            {
                string fromDate = DateHelper.GetDateFormat(DateHelper.GetDateFormat(request.FromDate), false, false, DateFormats.SqlDateFormat);
                string toDate = DateHelper.GetDateFormat(DateHelper.GetDateFormat(request.ToDate), false, false, DateFormats.SqlDateFormat);

                var pOutletId = DBHelper.GenerateDapperParameter("OUTLETID", request.OutletId, DbType.Int64);
                var pFromDate = DBHelper.GenerateDapperParameter("FROMDATE", fromDate ?? "", DbType.String);
                var pToDate = DBHelper.GenerateDapperParameter("TODATE", toDate ?? "", DbType.String);
                var pSearchQuery = DBHelper.GenerateDapperParameter("SEARCHQUERY", request.SearchQuery ?? "", DbType.String);
                var pPageNumber = DBHelper.GenerateDapperParameter("PAGENUMBER", request.PageNumber, DbType.Int64);
                var pRecordsPerPage = DBHelper.GenerateDapperParameter("RECORDPERPAGE", request.RecordsPerPage, DbType.Int64);

                return await _unit
                             .DapperRepository
                             .GetPaginationResultsWithStoreProcedureAsync<PurchaseOrderMasterRequest>("GET_PURCHASE_ORDER_LIST",
                                                                                                        DBHelper.GetDapperParms
                                                                                                        (
                                                                                                            pOutletId,
                                                                                                            pFromDate,
                                                                                                            pToDate,
                                                                                                            pSearchQuery,
                                                                                                            pPageNumber,
                                                                                                            pRecordsPerPage
                                                                                                        ));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemovePurchaseOrder(long purchaseOrderMasterId)
        {
            try
            {
                var purchaseOrderDetailsToDelete = await GetPurchaseOrderDetailById(purchaseOrderMasterId);

                if (purchaseOrderDetailsToDelete?.Count > 0)
                {
                    purchaseOrderDetailsToDelete.ForEach(detail =>
                    {
                        if (detail?.PurchaseOrderDetailId > 0)
                        {
                            _unit.PurchaseOrderDetailRepository.DeleteById(detail.PurchaseOrderDetailId);
                        }
                    });
                }

                _unit.PurchaseOrderMasterRepository.DeleteById(purchaseOrderMasterId);
                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> PostMultipleOrders(List<long> purchaseMasterIds, short status)
        {
            try
            {
                if (purchaseMasterIds?.Count > 0)
                {
                    var purchaseOrderMasterList = (await _unit
                                                        .PurchaseOrderMasterRepository
                                                        .GetAsync(x => purchaseMasterIds.Contains(x.PurchaseOrderMasterId)))
                                                        .ToList();

                    if (purchaseOrderMasterList?.Count > 0)
                    {
                        purchaseOrderMasterList
                            .ForEach(x => x.Status = status);
                        _unit.PurchaseOrderMasterRepository.UpdateList(purchaseOrderMasterList);

                        return await _unit.SaveAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
    }
}