using AMNSystemsERP.CL.Models.AccountModels.ChartOfAccounts;

namespace AMNSystemsERP.BL.Repositories.ChartOfAccounts
{
    public interface IChartOfAccountsService
    {
        // ----------------------------------------------------------------------------
        // ------------------ Chart Of Accounts Section Start -------------------------
        // ----------------------------------------------------------------------------

        Task<List<MainHeadsResponse>> GetMainHeadsList();
        Task<HeadCategoriesRequest> AddHeadCategory(HeadCategoriesRequest request);
        Task<HeadCategoriesRequest> UpdateHeadCategory(HeadCategoriesRequest request);
        Task<bool> RemoveHeadCategory(long headCategoriesId);
        Task<List<HeadCategoriesRequest>> GetHeadCategoriesList(long outletId);
        Task<HeadCategoriesRequest> GetSingleHeadCategoryById(long headCategoriesId);
        Task<SubCategoriesResponse> AddSubCategory(SubCategoriesRequest request);
        Task<SubCategoriesResponse> UpdateSubCategory(SubCategoriesRequest request);
        Task<bool> RemoveSubCategory(long subCategoriesId);
        Task<List<SubCategoriesResponse>> GetSubCategoriesList(long outletId);
        Task<SubCategoriesResponse> GetSingleSubCategoryById(long subCategoriesId);
        Task<PostingAccountsResponse> AddPostingAccount(PostingAccountsRequest request);
        Task<PostingAccountsResponse> UpdatePostingAccounts(PostingAccountsRequest request);
        Task<bool> RemovePostingAccount(long postingAccountsId);
        Task<List<PostingAccountsResponse>> GetPostingAccountList(long outletId);
        Task<List<PostingAccountsResponse>> GetExpensePostingAccountList(long outletId);
        Task<PostingAccountsResponse> GetSinglePostingAccountById(long postingAccountId);
    }
}