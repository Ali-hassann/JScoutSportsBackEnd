using AMNSystemsERP.BL.Repositories.Vouchers;
using AMNSystemsERP.CL.Models.AccountModels.Reports;
using AMNSystemsERP.CL.Models.AccountModels.Vouchers;
using AMNSystemsERP.CL.Models.Commons.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/Vouchers")]
    public class VouchersController : ApiController
    {
        private readonly IVoucherService _vouchersService;

        public VouchersController(IVoucherService vouchersService)
        {
            _vouchersService = vouchersService;
        }

        [HttpPost]
        [Route("AddVoucher")]
        public async Task<VoucherMasterList> AddVoucher([FromBody] VouchersMasterRequest request)
        {
            try
            {
                if (request.OutletId > 0
                    && request.VoucherTypeId > 0
                    && request.VoucherDate != DateTime.MinValue
                    && request.VoucherDate != DateTime.MaxValue
                    && request.VouchersDetailRequest?.Count > 0)
                {
                    return await _vouchersService.AddVoucher(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateVoucher")]
        public async Task<VoucherMasterList> UpdateVoucher([FromBody] VouchersMasterRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.VoucherTypeId > 0
                    && request.VouchersMasterId > 0
                    && request.VoucherDate != DateTime.MinValue
                    && request.VoucherDate != DateTime.MaxValue
                    && request.VouchersDetailRequest?.Count > 0)
                {
                    return await _vouchersService.UpdateVoucher(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveVoucher")]
        public async Task<bool> RemoveVoucher(long voucherMasterId)
        {
            try
            {
                if (voucherMasterId > 0)
                {
                    return await _vouchersService.RemoveVoucher(voucherMasterId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetVouchersById")]
        public async Task<VouchersMasterRequest> GetVouchersById(long voucherId)
        {
            try
            {
                if (voucherId > 0)
                {
                    return await _vouchersService.GetVouchersById(voucherId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("GetVouchersListWithPagination")]
        public async Task<PaginationResponse<VoucherMasterList>> GetVouchersListWithPagination([FromBody] VoucherListRequest request)
        {
            try
            {
                if (request != null
                    && request.OrganizationId > 0
                    && request.OutletId > 0
                    && request.ToDate != DateTime.MinValue
                    && request.ToDate != DateTime.MaxValue
                    && request.FromDate != DateTime.MinValue
                    && request.FromDate != DateTime.MaxValue)
                {
                    return await _vouchersService.GetVouchersListWithPagination(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("PostMultipleVouchers")]
        public async Task<bool> PostMultipleVouchers([FromBody] List<long> voucherIds)
        {
            try
            {
                if (voucherIds?.Count > 0)
                {
                    return await _vouchersService.PostMultipleVouchers(voucherIds);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetUnPostedVouchersCount")]
        public async Task<int> GetUnPostedVouchersCount(long OutletId)
        {
            try
            {
                if (OutletId > 0)
                {
                    return await _vouchersService.GetUnPostedVouchersCount(OutletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return 0;
        }

        [HttpPost]
        [Route("SaveGeneralExpenseVoucher")]
        public async Task<bool> SaveGeneralExpenseVoucher([FromBody] ExpenseVoucherRequest request)
        {
            try
            {
                if (request?.OrganizationId > 0
                    && request.OutletId > 0
                    && request.TotalAmount > 0
                    && request.PostingAccountsId > 0
                    && !string.IsNullOrEmpty(request.Remarks))
                {
                    return await _vouchersService.SaveGeneralExpenseVoucher(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("GetGeneralExpenseVoucherList")]
        public async Task<List<LedgerPrintReportResponse>> GetGeneralExpenseVoucherList([FromBody] ReportPrintLedgerRequest request)
        {
            try
            {
                if (request?.OrganizationId > 0
                    && request.OutletId > 0
                    && !string.IsNullOrEmpty(request.FromDate)
                    && !string.IsNullOrEmpty(request.ToDate))
                {
                    return await _vouchersService.GetGeneralExpenseVoucherList(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
    }
}