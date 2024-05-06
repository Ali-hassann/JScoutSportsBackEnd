using AMNSystemsERP.CL.Models.AccountModels.Reports;
using AMNSystemsERP.CL.Models.AccountModels.Vouchers;
using AMNSystemsERP.CL.Models.Commons.Pagination;
using AMNSystemsERP.CL.Models.OrganizationModels;

namespace AMNSystemsERP.BL.Repositories.Vouchers
{
    public interface IVoucherService
    {
        Task<VoucherMasterList> AddVoucher(VouchersMasterRequest request);
        Task<VoucherMasterList> UpdateVoucher(VouchersMasterRequest request);
        Task<bool> RemoveVoucher(long voucherMasterId);
        Task<VouchersMasterRequest> GetVouchersById(long voucherMasterId);
        Task<PaginationResponse<VoucherMasterList>> GetVouchersListWithPagination(VoucherListRequest request);
        Task<bool> PostMultipleVouchers(List<long> voucherIds);
        Task<int> GetUnPostedVouchersCount(long outletId);
        Task<bool> SaveGeneralExpenseVoucher(ExpenseVoucherRequest request);
        Task<List<LedgerPrintReportResponse>> GetGeneralExpenseVoucherList(ReportPrintLedgerRequest request);
    }
}