using AMNSystemsERP.BL.Repositories.Vouchers;
using AMNSystemsERP.CL.Enums;
using AMNSystemsERP.CL.Enums.AccountEnums;
using AMNSystemsERP.CL.Enums.InventoryEnums;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.AccountModels.Vouchers;
using AMNSystemsERP.CL.Models.Commons.Pagination;
using AMNSystemsERP.CL.Models.StockManagementModels;
using AMNSystemsERP.DL.DB.DBSets.StockManagement;
using AutoMapper;
using System.Data;
using static QRCoder.PayloadGenerator;

namespace AMNSystemsERP.BL.Repositories.StockManagement
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IVoucherService _voucherService;

        public InvoiceService(IUnitOfWork unit, IMapper mapper, IVoucherService voucherService)
        {
            _unit = unit;
            _mapper = mapper;
            _voucherService = voucherService;
        }

        // ------------------ Invoice Services--------------------------------------        

        public async Task<InvoiceMasterRequest> AddInvoice(InvoiceMasterRequest request)
        {
            try
            {
                var invoiceMaster = _mapper.Map<InvoiceMaster>(request);

                if (invoiceMaster?.OutletId > 0)
                {
                    request
                        .InvoiceDetailsRequest
                        .ForEach(invoiceDetail =>
                        {
                            var detail = _mapper.Map<InvoiceDetail>(invoiceDetail);
                            if (detail != null)
                            {
                                invoiceMaster.InvoiceDetail.Add(detail);
                            }
                        });


                    /***** Getting Serial index ***/

                    var serialDetail = await GenerateInvoiceSerialNo(request.DocumentTypeId, invoiceMaster.InvoiceDate);
                    invoiceMaster.InvoiceSerialNo = serialDetail.SerialNumber;

                    invoiceMaster.SerialIndex = serialDetail.SerialIndex;

                    _unit.InvoiceMasterRepository.InsertSingle(invoiceMaster);

                    if (await _unit.SaveAsync())
                    {
                        return await GetInvoiceById(invoiceMaster.InvoiceMasterId);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new InvoiceMasterRequest();
        }

        public async Task<InvoiceMasterRequest> UpdateInvoice(InvoiceMasterRequest request)
        {
            try
            {
                var dBInvoice = await GetInvoiceDetailById(request.InvoiceMasterId);

                if (dBInvoice?.Count > 0)
                {
                    var invoiceMaster = _mapper.Map<InvoiceMaster>(request);

                    /*Deleting existing line items against this invoice.No*/
                    dBInvoice?.ForEach(invoiceDetail =>
                    {
                        _unit.InvoiceDetailRepository.DeleteById(invoiceDetail.InvoiceDetailId);
                    });

                    /*Adding new line items ...*/

                    request.InvoiceDetailsRequest.ForEach(invoiceDetail =>
                    {
                        var detail = _mapper.Map<InvoiceDetail>(invoiceDetail);
                        if (detail?.InvoiceMasterId > 0)
                        {
                            detail.InvoiceDetailId = 0;
                            invoiceMaster.InvoiceDetail.Add(detail);
                        }
                    });

                    _unit.InvoiceMasterRepository.Update(invoiceMaster);

                    if (await _unit.SaveAsync())
                    {
                        return await GetInvoiceById(invoiceMaster.InvoiceMasterId);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new InvoiceMasterRequest();
        }

        private async Task<SerialModel> GenerateInvoiceSerialNo(InventoryDocumentType documentType, DateTime date)
        {
            var query = $@"SELECT ISNULL(MAX(SerialIndex),0) as SerialIndex
                                    FROM InvoiceMaster
                                    WHERE DocumentTypeId = {(int)documentType}
                                    AND YEAR(InvoiceDate)= {date.Year}";

            var indexNo = await _unit.DapperRepository.GetSingleQueryAsync<long>(query);

            return new SerialModel { SerialNumber = @$"{DocTypeShortName(documentType)}-{date.ToString("yy")}-{indexNo + 1}", SerialIndex = indexNo + 1 };
        }

        string DocTypeShortName(InventoryDocumentType documentType)
        {
            switch (documentType)
            {
                case InventoryDocumentType.Purchase:
                    return "P";
                case InventoryDocumentType.PurchaseReturn:
                    return "PR";
                case InventoryDocumentType.Sale:
                    return "S";
                case InventoryDocumentType.SaleReturn:
                    return "SR";
                case InventoryDocumentType.Issuance:
                    return "I";
                case InventoryDocumentType.IssuanceReturn:
                    return "IR";
                default:
                    return "P";
            }
        }

        public async Task<InvoiceMasterRequest> GetInvoiceById(long invoiceMasterId)
        {
            try
            {
                var query = $@"SELECT   
	                                InvoiceMasterId        
	                                , InvoiceDate        
	                                , IM.ParticularId        
	                                , ParticularName    
	                                , ReferenceNo        
	                                , IM.DocumentTypeId    
	                                , DocumentTypeName    
	                                , Remarks        
	                                , InvoiceStatus        
	                                , TotalAmount        
	                                , Discount        
	                                , NetAmount        
	                                , PaidReceivedAmount   
	                                , BalanceAmount        
	                                , OutletId    	                                
                                    , PaymentMode
									, SaleTax
                                FROM InvoiceMaster AS IM
                                INNER JOIN Particular AS P
	                                ON P.ParticularId = IM.ParticularId
                                INNER JOIN DocumentType AS DT
	                                ON DT.DocumentTypeId = IM.DocumentTypeId
                                WHERE InvoiceMasterId = {invoiceMasterId}";

                return await _unit.DapperRepository.GetSingleQueryAsync<InvoiceMasterRequest>(query) ?? new InvoiceMasterRequest();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InvoiceDetailRequest>> GetInvoiceDetailById(long invoiceMasterId)
        {
            try
            {
                var query = $@"SELECT                 
                                    InvoiceDetailId                        
                                    , IM.InvoiceMasterId                        
                                    , ID.ItemId                        
                                    , BarCode                        
                                    , Quantity                           
                                    , Price                         
                                    , TotalAmount                            
                                    , AverageRate                    
                                    , RunningTotal                    
                                    , ItemName                    
                                    , UnitName                    
                                    , ItemCategoryName                    
                                    , ItemTypeName                    
                                    , Description               
                                    , TransactionType              
                                    , Amount                
                                FROM InvoiceMaster AS IM                
                                INNER JOIN InvoiceDetail AS ID                
                                    ON ID.InvoiceMasterId = IM.InvoiceMasterId                
                                INNER JOIN Item AS I                
                                    ON I.ItemId = ID.ItemId                
                                INNER JOIN Unit AS U                
                                    ON U.UnitId = I.UnitId                
                                INNER JOIN ItemCategory AS IC                
                                    ON IC.ItemCategoryId = I.ItemCategoryId                
                                INNER JOIN ItemType AS IT                
                                    ON IT.ItemTypeId = IC.ItemTypeId        
                                WHERE IM.InvoiceMasterId = {invoiceMasterId}                      
                                ORDER BY InvoiceDetailId ";

                return await _unit.DapperRepository.GetListQueryAsync<InvoiceDetailRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginationResponse<InvoiceMasterRequest>> GetInvoiceListWithPagination(InvoiceParameterRequest request)
        {
            try
            {
                var pOutletId = DBHelper.GenerateDapperParameter("OUTLETID", request.OutletId, DbType.Int64);
                var pFromDate = DBHelper.GenerateDapperParameter("FROMDATE", request.FromDate ?? "", DbType.String);
                var pToDate = DBHelper.GenerateDapperParameter("TODATE", request.ToDate ?? "", DbType.String);
                var pDocumentTypeId = DBHelper.GenerateDapperParameter("INVENTORYDOCUMENTTYPESID", request.DocumentTypeId, DbType.Int64);
                var pSearchQuery = DBHelper.GenerateDapperParameter("SEARCHQUERY", request.SearchQuery ?? "", DbType.String);
                var pPageNumber = DBHelper.GenerateDapperParameter("PAGENUMBER", request.PageNumber, DbType.Int64);
                var pRecordsPerPage = DBHelper.GenerateDapperParameter("RECORDPERPAGE", request.RecordsPerPage, DbType.Int64);
                var pDocumentCode = DBHelper.GenerateDapperParameter("DOCUMENTCODE", request.DocumentCode, DbType.Int16);

                var invoiceList = await _unit
                                        .DapperRepository
                                        .GetPaginationResultsWithStoreProcedureAsync<InvoiceMasterRequest>("GET_INVOICE_LIST",
                                                                                                                 DBHelper.GetDapperParms
                                                                                                                 (
                                                                                                                     pOutletId,
                                                                                                                     pFromDate,
                                                                                                                     pToDate,
                                                                                                                     pDocumentTypeId,
                                                                                                                     pSearchQuery,
                                                                                                                     pPageNumber,
                                                                                                                     pRecordsPerPage,
                                                                                                                     pDocumentCode
                                                                                                                 ));
                return invoiceList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveInvoice(long invoiceMasterId)
        {
            try
            {
                var dBInvoiceMaster = await GetInvoiceDetailById(invoiceMasterId);

                if (dBInvoiceMaster?.Count > 0)
                {
                    dBInvoiceMaster
                        .ForEach(data =>
                        {
                            _unit.InvoiceDetailRepository.DeleteById(data.InvoiceDetailId);
                        });
                }
                _unit.InvoiceMasterRepository.DeleteById(invoiceMasterId);
                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> PostInvoice(InvoiceMasterRequest request)
        {
            try
            {
                if (request?.InvoiceMasterId > 0)
                {
                    if (request.DocumentTypeId == InventoryDocumentType.Purchase || request.DocumentTypeId == InventoryDocumentType.PurchaseReturn)
                    {
                        if (await InvoiceIntegrateToAccounts(request))
                        {
                            var queryForUpdateInvoice = $"UPDATE InvoiceMaster Set InvoiceStatus = 1 WHERE InvoiceMasterId = {request.InvoiceMasterId}";
                            await _unit.DapperRepository.ExecuteNonQuery(queryForUpdateInvoice, null, CommandType.Text);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return false;
        }

        private async Task<bool> InvoiceIntegrateToAccounts(InvoiceMasterRequest request)
        {
            try
            {
                var voucher = new VouchersMasterRequest();
                long vendorAccountId = 0;

                var configurationList = (await _unit
                                                 .ConfigurationSettingRepository
                                                 .GetAsync(x => x.OutletId == request.OutletId))?.ToList();

                var partyAccount = (await _unit
                                          .PostingAccountsRepository
                                          .GetAsync(x => x.ParticularId == request.ParticularId))?.SingleOrDefault();

                if (configurationList?.Count > 0 && partyAccount?.PostingAccountsId > 0)
                {
                    var cashAccountId = configurationList.FirstOrDefault(d => d.AccountName == "CashAccount")?.AccountValue ?? 0;

                    var purchaseAccountId = configurationList.FirstOrDefault(d => d.AccountName == "Purchase")?.AccountValue ?? 0;
                    var purchaseDiscountId = configurationList.FirstOrDefault(d => d.AccountName == "PurchaseDiscount")?.AccountValue ?? 0;
                    var purchaseReturnId = configurationList.FirstOrDefault(d => d.AccountName == "PurchaseReturn")?.AccountValue ?? 0;

                    vendorAccountId = partyAccount.PostingAccountsId;

                    /****** Creating voucher *****/
                    /**** VouchersMaster ****/
                    voucher.VoucherDate = request.InvoiceDate;
                    voucher.ReferenceNo = request.InvoiceSerialNo;
                    voucher.Remarks = request.Remarks;
                    voucher.OutletId = request.OutletId;
                    voucher.VoucherStatus = (int)VoucherStatus.Posted;
                    voucher.TransactionType = VoucherTransactionType.None.ToString();
                    voucher.VoucherTypeId = request.PaymentMode == PaymentMode.Cash ? VoucherType.CashPayment : VoucherType.Journal;

                    if (request.DocumentTypeId == InventoryDocumentType.Purchase)
                    {
                        /**** VouchersDetail ****/

                        if (request.NetAmount > 0)
                        {
                            //Dr
                            voucher.VouchersDetailRequest.Add(new VouchersDetailRequest
                            {
                                Narration = "",
                                DebitAmount = request.NetAmount,
                                PostingAccountsId = purchaseAccountId
                            });
                            //Cr
                            voucher.VouchersDetailRequest.Add(new VouchersDetailRequest
                            {
                                CreditAmount = request.NetAmount,
                                PostingAccountsId = request.PaymentMode == PaymentMode.Cash ? cashAccountId : vendorAccountId
                            });
                        }

                        /*** Discount on Purchase or Sale***/
                        if (request.Discount > 0)
                        {
                            //Dr
                            voucher.VouchersDetailRequest.Add(new VouchersDetailRequest
                            {
                                Narration = "Discount on Purchase",
                                DebitAmount = request.Discount,
                                PostingAccountsId = request.PaymentMode == PaymentMode.Cash ? cashAccountId : vendorAccountId
                            });
                            //Cr
                            voucher.VouchersDetailRequest.Add(new VouchersDetailRequest
                            {
                                CreditAmount = request.Discount,
                                PostingAccountsId = purchaseDiscountId   /* discunt*/
                            });
                        }
                    }

                    else if (request.DocumentTypeId == InventoryDocumentType.PurchaseReturn)
                    {
                        voucher.VoucherTypeId = VoucherType.Journal;

                        if (request.NetAmount > 0)
                        {
                            //Dr
                            voucher.VouchersDetailRequest.Add(new VouchersDetailRequest
                            {
                                DebitAmount = request.NetAmount,
                                PostingAccountsId = request.PaymentMode == PaymentMode.Cash ? cashAccountId : vendorAccountId
                            });
                            //Cr
                            voucher.VouchersDetailRequest.Add(new VouchersDetailRequest
                            {
                                Narration = "",
                                CreditAmount = request.NetAmount,
                                PostingAccountsId = purchaseReturnId

                            });
                        }
                    }
                }
                voucher.TotalAmount = voucher.VouchersDetailRequest.Sum(c => c.DebitAmount);
                return (await _voucherService.AddVoucher(voucher))?.VouchersMasterId > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private class SerialModel
        {
            public long SerialIndex { get; set; }
            public string SerialNumber { get; set; }
        }
    }
}
