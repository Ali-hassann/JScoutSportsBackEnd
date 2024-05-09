using AMNSystemsERP.BL.Repositories.ChartOfAccounts;
using AMNSystemsERP.CL.Models.AccountModels.ChartOfAccounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/ChartOfAccounts")]
    public class COAController : ApiController
    {
        private readonly IChartOfAccountsService _chartOfAccountsService;

        public COAController(IChartOfAccountsService chartOfAccountsService)
        {
            _chartOfAccountsService = chartOfAccountsService;
        }

        // ------------------ Chart Of Accounts Section Start -----------------------------

        [HttpGet]
        [Route("GetMainHeadsList")]
        public async Task<List<MainHeadsResponse>> GetMainHeadsList()
        {
            try
            {
                return await _chartOfAccountsService.GetMainHeadsList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("AddHeadCategory")]
        public async Task<HeadCategoriesRequest> AddHeadCategory([FromBody] HeadCategoriesRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.MainHeadsId > 0
                    && !string.IsNullOrEmpty(request.Name))
                {
                    return await _chartOfAccountsService.AddHeadCategory(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateHeadCategory")]
        public async Task<HeadCategoriesRequest> UpdateHeadCategory([FromBody] HeadCategoriesRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.MainHeadsId > 0
                    && request.HeadCategoriesId > 0
                    && !string.IsNullOrEmpty(request.Name))
                {
                    return await _chartOfAccountsService.UpdateHeadCategory(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveHeadCategory")]
        public async Task<bool> RemoveHeadCategory(long headCategoriesId)
        {
            try
            {
                if (headCategoriesId > 0)
                {
                    return await _chartOfAccountsService.RemoveHeadCategory(headCategoriesId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetHeadCategoriesList")]
        public async Task<List<HeadCategoriesRequest>> GetHeadCategoriesList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _chartOfAccountsService.GetHeadCategoriesList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetSingleHeadCategoryById")]
        public async Task<HeadCategoriesRequest> GetSingleHeadCategoryById(long headCategoriesId)
        {
            try
            {
                if (headCategoriesId > 0)
                {
                    return await _chartOfAccountsService.GetSingleHeadCategoryById(headCategoriesId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("AddSubCategory")]
        public async Task<SubCategoriesResponse> AddSubCategory([FromBody] SubCategoriesRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.HeadCategoriesId > 0
                    && !string.IsNullOrEmpty(request.Name))
                {
                    return await _chartOfAccountsService.AddSubCategory(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateSubCategory")]
        public async Task<SubCategoriesResponse> UpdateSubCategory([FromBody] SubCategoriesRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.SubCategoriesId > 0
                    && request.HeadCategoriesId > 0
                    && !string.IsNullOrEmpty(request.Name))
                {
                    return await _chartOfAccountsService.UpdateSubCategory(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveSubCategory")]
        public async Task<bool> RemoveSubCategory(long subCategoriesId)
        {
            try
            {
                if (subCategoriesId > 0)
                {
                    return await _chartOfAccountsService.RemoveSubCategory(subCategoriesId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetSubCategoriesList")]
        public async Task<List<SubCategoriesResponse>> GetSubCategoriesList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _chartOfAccountsService.GetSubCategoriesList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetSingleSubCategoryById")]
        public async Task<SubCategoriesResponse> GetSingleSubCategoryById(long subCategoriesId)
        {
            try
            {
                if (subCategoriesId > 0)
                {
                    return await _chartOfAccountsService.GetSingleSubCategoryById(subCategoriesId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("AddPostingAccounts")]
        public async Task<PostingAccountsResponse> AddPostingAccounts([FromBody] PostingAccountsRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.SubCategoriesId > 0
                    && !string.IsNullOrEmpty(request.Name))
                {
                    return await _chartOfAccountsService.AddPostingAccount(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdatePostingAccounts")]
        public async Task<PostingAccountsResponse> UpdatePostingAccounts([FromBody] PostingAccountsRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.PostingAccountsId > 0
                    && request.SubCategoriesId > 0
                    && !string.IsNullOrEmpty(request.Name))
                {
                    return await _chartOfAccountsService.UpdatePostingAccounts(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemovePostingAccount")]
        public async Task<bool> RemovePostingAccount(long postingAccountsId)
        {
            try
            {
                if (postingAccountsId > 0)
                {
                    return await _chartOfAccountsService.RemovePostingAccount(postingAccountsId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetPostingAccountList")]
        public async Task<List<PostingAccountsResponse>> GetPostingAccountList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _chartOfAccountsService.GetPostingAccountList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetExpensePostingAccountList")]
        public async Task<List<PostingAccountsResponse>> GetExpensePostingAccountList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _chartOfAccountsService.GetExpensePostingAccountList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetSinglePostingAccountById")]
        public async Task<PostingAccountsResponse> GetSinglePostingAccountById(long postingAccountId)
        {
            try
            {
                if (postingAccountId > 0)
                {
                    return await _chartOfAccountsService.GetSinglePostingAccountById(postingAccountId);
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