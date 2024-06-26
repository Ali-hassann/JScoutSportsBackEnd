using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.Commons.Pagination;
using AMNSystemsERP.CL.Models.StockManagementModels;
using AMNSystemsERP.DL.DB.DBSets.StockManagement;
using AutoMapper;
using System.Data;

namespace AMNSystemsERP.BL.Repositories.StockManagement
{
    public class PurchaseRequisitionService : IPurchaseRequisitionService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public PurchaseRequisitionService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        // ------------------ Purchase Requisition Services--------------------------------------        

        public async Task<PurchaseRequisitionMasterRequest> AddPurchaseRequisition(PurchaseRequisitionMasterRequest request)
        {
            try
            {
                var requisitionMaster = _mapper.Map<PurchaseRequisitionMaster>(request);

                if (requisitionMaster != null)
                {
                    request
                        .PurchaseRequisitionDetailRequest
                        .ForEach(requisitionDetail =>
                        {
                            var detail = _mapper.Map<PurchaseRequisitionDetail>(requisitionDetail);
                            if (detail != null)
                            {
                                requisitionMaster.PurchaseRequisitionDetail.Add(detail);
                            }
                        });
                }

                _unit.PurchaseRequisitionMasterRepository.InsertSingle(requisitionMaster);

                if (await _unit.SaveAsync())
                {
                    request.PurchaseRequisitionMasterId = requisitionMaster.PurchaseRequisitionMasterId;

                    return request;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return new PurchaseRequisitionMasterRequest();
        }

        public async Task<PurchaseRequisitionMasterRequest> UpdatePurchaseRequisition(PurchaseRequisitionMasterRequest request)
        {
            try
            {
                var dBRequisition = await GetPurchaseRequisitionById(request.PurchaseRequisitionMasterId);

                var reqMaster = _mapper.Map<PurchaseRequisitionMaster>(request);

                /*Deleting existing line items against this req.No*/
                dBRequisition?.PurchaseRequisitionDetailRequest?.ForEach(reqDetail =>
                {
                    var detail = _mapper.Map<PurchaseRequisitionDetail>(reqDetail);
                    if (detail != null)
                    {
                        _unit.PurchaseRequisitionDetailRepository.DeleteByEntity(detail);
                    }
                });


                /*Adding new line items ...*/

                request.PurchaseRequisitionDetailRequest.ForEach(reqDetail =>
                {
                    var detail = _mapper.Map<PurchaseRequisitionDetail>(reqDetail);
                    if (detail != null)
                    {
                        detail.PurchaseRequisitionDetailId = 0;
                        reqMaster.PurchaseRequisitionDetail.Add(detail);
                    }
                });

                _unit.PurchaseRequisitionMasterRepository.Update(reqMaster);

                if (await _unit.SaveAsync())
                {
                    return await GetPurchaseRequisitionById(reqMaster.PurchaseRequisitionMasterId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new PurchaseRequisitionMasterRequest();
        }

        public async Task<PurchaseRequisitionMasterRequest> GetPurchaseRequisitionById(long purchaseRequisitionMasterId)
        {
            try
            {
                var reqMaster = new PurchaseRequisitionMasterRequest();

                var reqId = DBHelper.GenerateDapperParameter("PURCHASEREQUISITIONMASTERID", purchaseRequisitionMasterId, DbType.Int64);

                var data = await _unit
                                 .DapperRepository
                                 .GetMultiResultsWithStoreProcedureAsync<PurchaseRequisitionMasterRequest,
                                                                         PurchaseRequisitionDetailRequest>("GET_PURCHASEREQUISITION_BY_ID",
                                                                                                            DBHelper.GetDapperParms
                                                                                                            (
                                                                                                                reqId
                                                                                                            ), CommandType.StoredProcedure);

                if (data != null)
                {
                    reqMaster = data.Item1 == null ? new PurchaseRequisitionMasterRequest() : data.Item1;

                    if (data.Item2?.Count > 0)
                    {
                        reqMaster.PurchaseRequisitionDetailRequest = data.Item2;
                    }
                }
                return reqMaster;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginationResponse<PurchaseRequisitionMasterRequest>> GetPurchaseRequisitionListWithPagination(InvoiceParameterRequest request)
        {
            try
            {
                string fromDate = DateHelper.GetDateFormat(DateHelper.GetDateFormat(request.FromDate), false, false, DateFormats.SqlDateFormat);
                string toDate = DateHelper.GetDateFormat(DateHelper.GetDateFormat(request.ToDate), false, false, DateFormats.SqlDateFormat);

                var pFromDate = DBHelper.GenerateDapperParameter("FROMDATE", fromDate ?? "", DbType.String);
                var pToDate = DBHelper.GenerateDapperParameter("TODATE", toDate ?? "", DbType.String);
                var pSearchQuery = DBHelper.GenerateDapperParameter("SEARCHQUERY", request.SearchQuery ?? "", DbType.String);
                var pPageNumber = DBHelper.GenerateDapperParameter("PAGENUMBER", request.PageNumber, DbType.Int64);
                var pRecordsPerPage = DBHelper.GenerateDapperParameter("RECORDPERPAGE", request.RecordsPerPage, DbType.Int64);

                var invoiceList = await _unit
                                        .DapperRepository
                                        .GetPaginationResultsWithStoreProcedureAsync<PurchaseRequisitionMasterRequest>("GET_PURCHASEREQUISITION_LIST",
                                                                                                                 DBHelper.GetDapperParms
                                                                                                                 (
                                                                                                                     pSearchQuery,
                                                                                                                     pPageNumber,
                                                                                                                     pRecordsPerPage,
                                                                                                                     pFromDate,
                                                                                                                     pToDate
                                                                                                                 ));
                return invoiceList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemovePurchaseRequisition(long requisitionMasterId)
        {
            try
            {
                var dBInvoiceMaster = await GetPurchaseRequisitionById(requisitionMasterId);

                if (dBInvoiceMaster != null)
                {
                    var reqMaster = _mapper.Map<PurchaseRequisitionMaster>(dBInvoiceMaster);

                    dBInvoiceMaster.PurchaseRequisitionDetailRequest
                        .ForEach(data =>
                        {
                            var detail = _mapper.Map<PurchaseRequisitionDetail>(data);
                            if (detail != null)
                            {
                                _unit.PurchaseRequisitionDetailRepository.DeleteByEntity(detail);
                            }
                        });

                    _unit.PurchaseRequisitionMasterRepository.DeleteByEntity(reqMaster);
                }

                var isDeleted = await _unit.SaveAsync();
                if (isDeleted)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }//
            return false;
        }

        public async Task<bool> PostMultipleRequisitions(List<long> reqIds, short reqStatus)
        {
            try
            {
                var requisitions = (await _unit
                                         .PurchaseRequisitionMasterRepository
                                         .GetAsync(x => reqIds.Contains(x.PurchaseRequisitionMasterId))).ToList();

                if (requisitions?.Count > 0)
                {
                    requisitions
                        .ForEach(x => x.Status = reqStatus);
                    _unit.PurchaseRequisitionMasterRepository.UpdateList(requisitions);

                    return await _unit.SaveAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public async Task<List<PurchaseRequisitionDetailRequest>> GetRequisitionDetailByIds(List<long> reqIds)
        {
            try
            {
                var ids = string.Join(",", reqIds);

                var query = $@"SELECT 
	                                PD.ItemId
	                                , PD.Quantity
	                                , I.ItemName
	                                , I.ItemCategoryName
	                                , I.ItemTypeName
	                                , I.UnitName
	                                , I.Size
                                FROM PurchaseRequisitionDetail AS PD
                                INNER JOIN V_Item AS I
	                                ON I.ItemId = PD.ItemId
                                WHERE PD.PurchaseRequisitionMasterId IN({ids})";

                return await _unit.DapperRepository.GetListQueryAsync<PurchaseRequisitionDetailRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginationResponse<PurchaseRequisitionMasterRequest>> GetPurchaseRequisitionListByOrganization(InvoiceParameterRequest request)
        {
            try
            {
                string fromDate = DateHelper.GetDateFormat(DateHelper.GetDateFormat(request.FromDate), false, false, DateFormats.SqlDateFormat);
                string toDate = DateHelper.GetDateFormat(DateHelper.GetDateFormat(request.ToDate), false, false, DateFormats.SqlDateFormat);

                var pOrganizationId = DBHelper.GenerateDapperParameter("ORGANIZATIONID", request.OrganizationId, DbType.Int64);

                var pFromDate = DBHelper.GenerateDapperParameter("FROMDATE", fromDate ?? "", DbType.String);
                var pToDate = DBHelper.GenerateDapperParameter("TODATE", toDate ?? "", DbType.String);
                var pSearchQuery = DBHelper.GenerateDapperParameter("SEARCHQUERY", request.SearchQuery ?? "", DbType.String);
                var pPageNumber = DBHelper.GenerateDapperParameter("PAGENUMBER", request.PageNumber, DbType.Int64);
                var pRecordsPerPage = DBHelper.GenerateDapperParameter("RECORDPERPAGE", request.RecordsPerPage, DbType.Int64);

                var invoiceList = await _unit
                                        .DapperRepository
                                        .GetPaginationResultsWithStoreProcedureAsync<PurchaseRequisitionMasterRequest>("GET_PURCHASE_REQUISITION_BY_Organization_ID",
                                                                                                                 DBHelper.GetDapperParms
                                                                                                                 (
                                                                                                                     pOrganizationId,
                                                                                                                     pFromDate,
                                                                                                                     pToDate,
                                                                                                                     pSearchQuery,
                                                                                                                     pPageNumber,
                                                                                                                     pRecordsPerPage
                                                                                                                 ));
                return invoiceList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}