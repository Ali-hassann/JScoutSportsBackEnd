using AMNSystemsERP.BL.Repositories.Configuration;
using AMNSystemsERP.BL.Repositories.Reports;
using AMNSystemsERP.CL.Enums;
using AMNSystemsERP.CL.Enums.AccountEnums;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.AccountModels.Reports;
using AMNSystemsERP.CL.Models.AccountModels.Vouchers;
using AMNSystemsERP.CL.Models.Commons.DocumentHelper;
using AMNSystemsERP.CL.Models.Commons.Pagination;
using AMNSystemsERP.CL.Models.OrganizationModels;
using AMNSystemsERP.CL.Services.Documents;
using AMNSystemsERP.DL.DB.DBSets.Accounts;
using AutoMapper;
using Core.CL.Enums;
using System;
using System.Data;

namespace AMNSystemsERP.BL.Repositories.Vouchers
{
    public class VoucherService : IVoucherService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IDocumentHelperService _documentHelperService;
        private readonly IConfigurationSettingService _configurationService;
        private readonly IReportsService _reportsService;

        public VoucherService(IUnitOfWork unit
        , IMapper mapper
        , IDocumentHelperService documentHelperService
        , IConfigurationSettingService configurationService
        , IReportsService reportsService)
        {
            _unit = unit;
            _mapper = mapper;
            _documentHelperService = documentHelperService;
            _configurationService = configurationService;
            _reportsService = reportsService;
        }

        /* 
        ----------------------------------------------------------------------------
        ---------- Adding VouchersMaster, VouchersDetail & PersonsExpenseDetail ----
        ----------------------------------------------------------------------------
        */

        public async Task<VoucherMasterList> AddVoucher(VouchersMasterRequest request)
        {
            var documentRequestList = new List<DocumentRequest>();

            try
            {
                var vouchersMaster = _mapper.Map<VouchersMaster>(request);
                if (vouchersMaster != null)
                {
                    request
                        .VouchersDetailRequest
                        .ForEach(voucherDetail =>
                        {
                            var detail = _mapper.Map<VouchersDetail>(voucherDetail);
                            if (detail != null)
                            {
                                // Adding Voucher Master Date
                                vouchersMaster.VouchersDetail.Add(detail);

                            }
                        });

                    /**********************/
                    var serialDetail = await GenerateVoucherSerialNo(request.VoucherTypeId, request.VoucherDate);

                    vouchersMaster.SerialNumber = serialDetail.SerialNumber;
                    vouchersMaster.SerialIndex = serialDetail.SerialIndex;

                    // Inserting/Saving Voucher Master/Detail Data
                    _unit.VouchersMasterRepository.InsertSingle(vouchersMaster);
                    var isSaved = await _unit.SaveAsync();
                    //

                    if (isSaved)
                    {
                        if (request.VoucherImagesRequest?.Count > 0)
                        {
                            // Getting DocumentRequest List for SaveMultipleDocument
                            foreach (var item in request.VoucherImagesRequest)
                            {
                                if (DocumentHelper.IsBase64String(item.Imagepath))
                                {
                                    var documentRequest = new DocumentRequest();
                                    documentRequest.OutletId = request.OutletId;
                                    documentRequest.IsSkipPersonId = true;
                                    documentRequest.FolderType = FolderType.Voucher;
                                    documentRequest.FileType = FileType.Images;
                                    documentRequest.PersonType = PersonType.Voucher.ToString();
                                    documentRequest.Base64 = item.Imagepath;
                                    documentRequest.FileName = $"{vouchersMaster.VouchersMasterId}_{Guid.NewGuid()}.jpg";
                                    documentRequestList.Add(documentRequest);
                                }
                            }
                            //

                            var imagesList = await _documentHelperService.SaveMultipleDoc(documentRequestList);
                            // Generating VoucherImages Object
                            var voucherImages = new List<VoucherImages>();
                            if (imagesList?.Count > 0)
                            {
                                foreach (var image in imagesList)
                                {
                                    voucherImages.Add(new VoucherImages()
                                    {
                                        Imagepath = image,
                                        VouchersMasterId = vouchersMaster.VouchersMasterId
                                    });
                                }
                            }
                            //

                            _unit.VoucherImagesRepository.InsertList(voucherImages);
                            await _unit.SaveAsync();
                        }

                        return await GetVoucherMasterById(vouchersMaster.VouchersMasterId);
                    }
                }
            }
            catch (Exception ex)
            {
                if (documentRequestList.Count > 0)
                {
                    await _documentHelperService.DeleteMultipleDoc(documentRequestList);
                }
                throw;
            }
            return new VoucherMasterList();
        }

        public async Task<VoucherMasterList> UpdateVoucher(VouchersMasterRequest request)
        {
            var documentListToAdd = new List<DocumentRequest>();
            var documentListToRemove = new List<DocumentRequest>();

            try
            {
                var dBVoucherMaster = await GetVouchersById(request.VouchersMasterId);

                if (dBVoucherMaster?.VouchersMasterId > 0)
                {
                    var vouchersMaster = _mapper.Map<VouchersMaster>(dBVoucherMaster);

                    // remove all existing records if exist 
                    dBVoucherMaster.VouchersDetailRequest.ForEach(voucherDetail =>
                    {
                        var detail = _mapper.Map<VouchersDetail>(voucherDetail);
                        if (detail != null)
                        {
                            _unit.VouchersDetailRepository.DeleteByEntity(detail);
                        }
                    });
                    //

                    // Add and Remove Voucher Images
                    var imagesRequest = request.VoucherImagesRequest;
                    var imagesToAdd = new List<VoucherImages>();
                    var imagesToRemove = new List<VoucherImages>();
                    if (imagesRequest?.Count > 0)
                    {
                        foreach (var image in imagesRequest)
                        {
                            if (image.IsDeleted)
                            {
                                imagesToRemove.Add(_mapper.Map<VoucherImages>(image));
                            }
                            else if (image.Imagepath.Contains($"{request.OutletId}/Voucher/"))
                            {
                            }
                            else if (DocumentHelper.IsBase64String(image.Imagepath))
                            {
                                imagesToAdd.Add(_mapper.Map<VoucherImages>(image));
                            }
                        }
                    }
                    if (imagesToRemove.Count > 0)
                    {
                        // Getting DocumentRequest List for RemoveMultipleDocument
                        foreach (var image in imagesToRemove)
                        {
                            var fileName = DocumentHelper.GetFileNameFromPath(image.Imagepath);
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                var documentRequest = new DocumentRequest();
                                documentRequest.OutletId = request.OutletId;
                                documentRequest.IsSkipPersonId = true;
                                documentRequest.FolderType = FolderType.Voucher;
                                documentRequest.FileType = FileType.Images;
                                documentRequest.PersonType = PersonType.Voucher.ToString();
                                documentRequest.FileName = fileName;
                                documentListToRemove.Add(documentRequest);
                            }
                        }
                        //
                        _unit.VoucherImagesRepository.DeleteRangeEntities(imagesToRemove);
                    }

                    //

                    // Add new record
                    request.VouchersDetailRequest.ForEach(voucherDetail =>
                    {
                        var detail = _mapper.Map<VouchersDetail>(voucherDetail);
                        if (detail != null)
                        {
                            detail.VouchersDetailId = 0;
                            vouchersMaster.VouchersDetail.Add(detail);
                        }
                    });
                    //

                    // Adding new Voucher Images
                    if (imagesToAdd.Count > 0)
                    {
                        foreach (var image in imagesToAdd)
                        {
                            if (DocumentHelper.IsBase64String(image.Imagepath))
                            {
                                documentListToAdd.Add(new DocumentRequest()
                                {
                                    OutletId = request.OutletId,
                                    IsSkipPersonId = true,
                                    FolderType = FolderType.Voucher,
                                    FileType = FileType.Images,
                                    PersonType = PersonType.Voucher.ToString(),
                                    Base64 = image.Imagepath,
                                    FileName = $"{vouchersMaster.VouchersMasterId}_{Guid.NewGuid()}.jpg"
                                });
                            }
                        }
                    }
                    //                  

                    // Generating voucherImagesToAdd object 
                    var voucherImagesToAdd = new List<VoucherImages>();
                    var savedImages = await _documentHelperService.SaveMultipleDoc(documentListToAdd);
                    if (savedImages?.Count > 0)
                    {
                        foreach (var path in savedImages)
                        {
                            voucherImagesToAdd.Add(new VoucherImages()
                            {
                                VouchersMasterId = vouchersMaster.VouchersMasterId,
                                Imagepath = path,
                            });
                        }
                        vouchersMaster.VoucherImages = voucherImagesToAdd;
                    }
                    //

                    // Updating/Saving Voucher Master/Detail Data
                    vouchersMaster.ReferenceNo = request.ReferenceNo;
                    vouchersMaster.VoucherDate = request.VoucherDate;
                    vouchersMaster.Remarks = request.Remarks;
                    vouchersMaster.VoucherTypeId = (int)request.VoucherTypeId;
                    vouchersMaster.TotalAmount = request.TotalAmount;

                    _unit.VouchersMasterRepository.Update(vouchersMaster);
                    var isUpdated = await _unit.SaveAsync();
                    //

                    if (isUpdated)
                    {
                        // Remove Image files
                        if (documentListToRemove.Count > 0)
                        {
                            await _documentHelperService.DeleteMultipleDoc(documentListToRemove);
                        }
                        //

                        return await GetVoucherMasterById(vouchersMaster.VouchersMasterId);
                    }
                }
            }
            catch (Exception)
            {
                await _documentHelperService.DeleteMultipleDoc(documentListToAdd);
                throw;
            }
            return new VoucherMasterList();
        }

        public async Task<bool> RemoveVoucher(long voucherMasterId)
        {
            try
            {
                var dBVoucherMaster = await GetVouchersById(voucherMasterId);

                if (dBVoucherMaster?.VouchersMasterId > 0 && dBVoucherMaster.VouchersDetailRequest.Count > 0)
                {
                    var vouchersMaster = _mapper.Map<VouchersMaster>(dBVoucherMaster);

                    // remove all records if exist 
                    var detailsToRemove = _mapper.Map<List<VouchersDetail>>(dBVoucherMaster.VouchersDetailRequest);
                    _unit.VouchersDetailRepository.DeleteRangeEntities(detailsToRemove);

                    if (dBVoucherMaster.VoucherImagesRequest?.Count > 0)
                    {
                        // Remove  Voucher Images 
                        var voucherImages = _mapper.Map<List<VoucherImages>>(dBVoucherMaster.VoucherImagesRequest);
                        _unit.VoucherImagesRepository.DeleteRangeEntities(voucherImages);
                        //
                    }
                    //

                    _unit.VouchersMasterRepository.DeleteById(voucherMasterId);

                    return await _unit.SaveAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public async Task<VouchersMasterRequest> GetVouchersById(long voucherMasterId)
        {
            try
            {
                VouchersMasterRequest vouchersMaster = new VouchersMasterRequest();
                var pVoucherMasterId = DBHelper.GenerateDapperParameter("VOUCHERSMASTERID", voucherMasterId, DbType.Int64);

                var voucher = await _unit
                                    .DapperRepository
                                    .GetMultiResultsWithParentChildAndDetailAsync<VouchersMasterRequest,
                                                                                  VouchersDetailRequest,
                                                                                  VoucherImagesRequest>
                                                                                  ("GET_VOUCHERS_BY_ID",
                                                                                   DBHelper.GetDapperParms
                                                                                   (
                                                                                       pVoucherMasterId
                                                                                   ));
                if (voucher != null)
                {
                    vouchersMaster = voucher.Item1;

                    if (voucher.Item2?.Count > 0)
                    {
                        vouchersMaster.VouchersDetailRequest = voucher.Item2;
                    }

                    if (voucher.Item3?.Count > 0)
                    {
                        vouchersMaster.VoucherImagesRequest = voucher.Item3;
                    }
                }
                return vouchersMaster;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginationResponse<VoucherMasterList>> GetVouchersListWithPagination(VoucherListRequest request)
        {
            try
            {
                var pOutletId = DBHelper.GenerateDapperParameter("OUTLETID", request.OutletId, DbType.Int64);
                var pVoucherTypeId = DBHelper.GenerateDapperParameter("VOUCHERTYPEID", request.VoucherTypeId, DbType.Int64);
                var pSearchQuery = DBHelper.GenerateDapperParameter("SEARCHQUERY", request.SearchQuery ?? "", DbType.String);
                var pPageNumber = DBHelper.GenerateDapperParameter("PAGENUMBER", request.PageNumber, DbType.Int64);
                var pRecordsPerPage = DBHelper.GenerateDapperParameter("RECORDPERPAGE", request.RecordsPerPage, DbType.Int64);
                var pFromDate = DBHelper.GenerateDapperParameter("FROMDATE", request.FromDate.Date.ToString("yyyy-MM-dd"), DbType.String);
                var pToDate = DBHelper.GenerateDapperParameter("TODATE", request.ToDate.Date.ToString("yyyy-MM-dd"), DbType.String);
                var pPostingStatus = DBHelper.GenerateDapperParameter("POSTINGSTATUS", request.PostingStatus, DbType.Int16);
                var pCreatedBy = DBHelper.GenerateDapperParameter("CREATEDBY", request.CreatedBy, DbType.String);


                var voucherList = await _unit
                                        .DapperRepository
                                        .GetPaginationResultsWithStoreProcedureAsync<VoucherMasterList>("GET_VOUCHER_LIST_WITH_PAGINATION",
                                                                                                         DBHelper.GetDapperParms
                                                                                                         (
                                                                                                             pOutletId,
                                                                                                             pVoucherTypeId,
                                                                                                             pSearchQuery,
                                                                                                             pPageNumber,
                                                                                                             pRecordsPerPage,
                                                                                                             pFromDate,
                                                                                                             pToDate,
                                                                                                             pPostingStatus,
                                                                                                             pCreatedBy
                                                                                                         ));
                return voucherList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> PostMultipleVouchers(List<long> voucherIds)
        {
            try
            {
                if (voucherIds?.Count > 0)
                {
                    var vouchers = (
                                        await _unit
                                             .VouchersMasterRepository
                                             .GetAsync(x => voucherIds.Contains(x.VouchersMasterId))
                                    )?.ToList()
                                    ?? new List<VouchersMaster>();

                    if (vouchers?.Count > 0)
                    {
                        vouchers.ForEach(x => { x.VoucherStatus = (int)VoucherStatus.Posted; });

                        _unit.VouchersMasterRepository.UpdateList(vouchers);

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

        private async Task<VoucherMasterList> GetVoucherMasterById(long voucherMasterId)
        {
            try
            {
                var query = $@"SELECT           
                                 VM.VouchersMasterId          
                                 , VM.ReferenceNo          
                                 , VM.VoucherTypeId          
                                 , VT.Name AS VoucherTypeName          
                                 , VM.VoucherDate          
                                 , VM.Remarks          
                                 , VM.TotalAmount          
                                 , VM.VoucherStatus     
                                 , VM.SerialNumber     
                                 , vs.StatusName as VoucherStatusName    
                                 , VM.OutletId              
                                 , COUNT(VI.VoucherImagesId) AS TotalImages        
                                FROM VouchersMaster AS VM          
                                INNER JOIN VoucherType AS VT          
                                 ON VT.VoucherTypeId = VM.VoucherTypeId        
                                INNER JOIN VouchersDetail AS VD                
                                 ON VD.VouchersMasterId = VM.VouchersMasterId        
                                LEFT JOIN VoucherImages AS VI        
                                 ON VI.VouchersMasterId = VM.VouchersMasterId         
                                 INNER JOIN VoucherStatus vs ON vm.VoucherStatus=vs.StatusId    
                                WHERE VM.VouchersMasterId = {voucherMasterId}        
                                GROUP BY        
                                 VM.VouchersMasterId          
                                 , VM.ReferenceNo          
                                 , VM.VoucherTypeId          
                                 , VT.Name          
                                 , VM.VoucherDate          
                                 , VM.Remarks          
                                 , VM.TotalAmount          
                                 , VM.VoucherStatus     
                                 , VM.SerialNumber     
                                 , vs.StatusName    
                                 , VM.OutletId      ";
                return await _unit.DapperRepository.GetSingleQueryAsync<VoucherMasterList>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetUnPostedVouchersCount(long outletId)
        {
            try
            {
                var countUnPostedVouchers = await _unit
                                                  .VouchersMasterRepository
                                                  .GetAsync(x => x.OutletId == outletId
                                                            && x.VoucherStatus == 0);
                return countUnPostedVouchers?.Count() ?? 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LedgerPrintReportResponse>> GetGeneralExpenseVoucherList(ReportPrintLedgerRequest request)
        {
            try
            {
                return await _reportsService.GetAccountsLedger(request);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SaveGeneralExpenseVoucher(ExpenseVoucherRequest request)
        {
            try
            {
                var configuration = await _configurationService.GetConfigurationSetting(request.OutletId);
                var vouchersMasterToAdd = new VouchersMasterRequest()
                {
                    VouchersMasterId = request.VouchersMasterId,
                    TotalAmount = request.TotalAmount,
                    OutletId = request.OutletId,
                    VoucherDate = request.VoucherDate,
                    Remarks = request.Remarks,
                    VoucherTypeId = VoucherType.CashPayment,
                    VoucherStatus = 2,
                    TransactionType = VoucherTransactionType.Cash.ToString(),

                    VouchersDetailRequest = new List<VouchersDetailRequest>()
                    {
                            new VouchersDetailRequest()
                            {
                                CreditAmount = 0,
                                DebitAmount = request.TotalAmount,
                                PostingAccountsId = request.PostingAccountsId,
                            },
                            new VouchersDetailRequest()
                            {
                                CreditAmount = request.TotalAmount,
                                DebitAmount = 0,
                                PostingAccountsId = configuration.FirstOrDefault(s=>s.AccountName == "CashAccount")?.AccountValue ?? 0,
                            }
                    }
                };
                var response = new VoucherMasterList();
                if (vouchersMasterToAdd.VouchersMasterId > 0)
                {
                    response = await UpdateVoucher(vouchersMasterToAdd);
                }
                else
                {
                    response = await AddVoucher(vouchersMasterToAdd);
                }
                if (response?.VouchersMasterId > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        async Task<SerialModel> GenerateVoucherSerialNo(VoucherType voucherType, DateTime date)
        {
            var query = @$"SELECT ISNULL(MAX(SerialIndex),0) FROM VouchersMaster 
                            WHERE YEAR(VoucherDate) = {date.Year} 
                            AND MOnth(VoucherDate) = {date.Month} 
                            AND VoucherTypeId = {(int)voucherType}";


            var maxId = await _unit.DapperRepository.GetSingleQueryAsync<long>(query);

            return new SerialModel { SerialNumber = @$"{DocTypeShortName(voucherType)}-{date.ToString("yy")}-{date.ToString("MMM")}-{maxId + 1}", SerialIndex = maxId + 1 };

        }
        string DocTypeShortName(VoucherType vtype)
        {
            string type = string.Empty;

            if (vtype == VoucherType.CashPayment)
                type = "CP";
            else if (vtype == VoucherType.CashReceipt)
                type = "CR";
            else if (vtype == VoucherType.BankPayment)
                type = "BP";
            else if (vtype == VoucherType.BankReceipt)
                type = "BR";
            else if (vtype == VoucherType.Journal)
                type = "JV";

            return type;
        }

        private class SerialModel
        {
            public long SerialIndex { get; set; }
            public string SerialNumber { get; set; }
        }
    }
}