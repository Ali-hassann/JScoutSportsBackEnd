using AMNSystemsERP.BL.Repositories.CommonRepositories;
using AMNSystemsERP.CL.Models.AccountModels.ChartOfAccounts;
using AMNSystemsERP.DL.DB.DBSets.Accounts;
using AutoMapper;

namespace AMNSystemsERP.BL.Repositories.ChartOfAccounts
{
    public class ChartOfAccountsService : IChartOfAccountsService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly AccountsGetData accountsGetData;

        public ChartOfAccountsService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        // ------------------ Accounts Head Section Start --------------------------------------

        public async Task<List<MainHeadsResponse>> GetMainHeadsList()
        {
            try
            {
                // Getting List of Main Heads 
                var mainHeads = await _unit.MainHeadsRepository.GetAsync();
                //
                if (mainHeads?.Count() > 0)
                {
                    return _mapper.Map<List<MainHeadsResponse>>(mainHeads);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<HeadCategoriesRequest> AddHeadCategory(HeadCategoriesRequest request)
        {
            try
            {
                var headCategory = _mapper.Map<HeadCategories>(request);

                _unit.HeadCategoriesRepository.InsertSingle(headCategory);
                var isSaved = await _unit.SaveAsync();

                if (isSaved)
                {
                    // Getting Head Category by Id
                    return await GetSingleHeadCategoryById(headCategory.HeadCategoriesId);
                    //
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new HeadCategoriesRequest();
        }

        public async Task<HeadCategoriesRequest> UpdateHeadCategory(HeadCategoriesRequest request)
        {
            try
            {
                var headCategory = _mapper.Map<HeadCategories>(request);

                _unit.HeadCategoriesRepository.Update(headCategory);

                var isUpdated = await _unit.SaveAsync();
                if (isUpdated)
                {
                    // Getting Head Category by Id
                    return await GetSingleHeadCategoryById(headCategory.HeadCategoriesId);
                    //
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new HeadCategoriesRequest();
        }

        public async Task<bool> RemoveHeadCategory(long headCategoriesId)
        {
            try
            {
                _unit.HeadCategoriesRepository.DeleteById(headCategoriesId);

                var isDeleted = await _unit.SaveAsync();
                if (isDeleted)
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

        public async Task<List<HeadCategoriesRequest>> GetHeadCategoriesList(long outletId)
        {
            try
            {
                var query = @$"SELECT       
                             HC.HeadCategoriesId      
                             , HC.Name      
                             , MH.Name AS MainHeadsName      
                             , HC.MainHeadsId       
                             , HC.OutletId      
                            FROM MainHeads AS MH      
                            INNER JOIN HeadCategories AS HC      
                             ON HC.MainHeadsId = MH.MainHeadsId      
                            WHERE HC.OutletId = {outletId}";

                var headCategoriesList = await _unit
                                               .DapperRepository
                                               .GetListQueryAsync<HeadCategoriesRequest>(query);
                return headCategoriesList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HeadCategoriesRequest> GetSingleHeadCategoryById(long headCategoriesId)
        {
            try
            {
                var query = $@"SELECT       
                                 HC.HeadCategoriesId      
                                 , HC.Name      
                                 , MH.Name AS MainHeadsName      
                                 , HC.MainHeadsId         
                                 , HC.OutletId      
                                FROM MainHeads AS MH      
                                INNER JOIN HeadCategories AS HC      
                                 ON HC.MainHeadsId = MH.MainHeadsId      
                                WHERE HC.HeadCategoriesId = {headCategoriesId}";

                var headCategory = await _unit
                                         .DapperRepository
                                         .GetSingleQueryAsync<HeadCategoriesRequest>(query);
                return headCategory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SubCategoriesResponse> AddSubCategory(SubCategoriesRequest request)
        {
            try
            {
                var subCategory = _mapper.Map<SubCategories>(request);

                _unit.SubCategoriesRepository.InsertSingle(subCategory);
                var isSaved = await _unit.SaveAsync();

                if (isSaved)
                {
                    // Getting Sub Category by Id
                    return await GetSingleSubCategoryById(subCategory.SubCategoriesId);
                    //
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new SubCategoriesResponse();
        }

        public async Task<SubCategoriesResponse> UpdateSubCategory(SubCategoriesRequest request)
        {
            try
            {
                var subCategory = _mapper.Map<SubCategories>(request);

                _unit.SubCategoriesRepository.Update(subCategory);

                var isUpdated = await _unit.SaveAsync();
                if (isUpdated)
                {
                    // Getting Sub Category by Id
                    return await GetSingleSubCategoryById(subCategory.SubCategoriesId);
                    //
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new SubCategoriesResponse();
        }

        public async Task<bool> RemoveSubCategory(long subCategoriesId)
        {
            try
            {
                _unit.SubCategoriesRepository.DeleteById(subCategoriesId);

                var isDeleted = await _unit.SaveAsync();
                if (isDeleted)
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

        public async Task<List<SubCategoriesResponse>> GetSubCategoriesList(long outletId)
        {
            try
            {
                var query = $@"SELECT           
                                 HC.HeadCategoriesId      
                                 , MH.Name AS MainHeadsName          
                                 , MH.MainHeadsId            
                                 , SC.OutletId      
                                 , SC.SubCategoriesId      
                                 , SC.Name AS SubCategoriesName      
                                 , SC.Name   
                                 , HC.Name AS HeadCategoriesName      
                                FROM MainHeads AS MH          
                                INNER JOIN HeadCategories AS HC          
                                 ON HC.MainHeadsId = MH.MainHeadsId      
                                INNER JOIN SubCategories AS SC      
                                 ON HC.HeadCategoriesId = SC.HeadCategoriesId      
                                WHERE SC.OutletId = {outletId}";

                return await _unit.DapperRepository.GetListQueryAsync<SubCategoriesResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SubCategoriesResponse> GetSingleSubCategoryById(long subCategoriesId)
        {
            try
            {
                var query = $@"SELECT           
                              HC.HeadCategoriesId      
                              , MH.Name AS MainHeadsName          
                              , MH.MainHeadsId              
                              , SC.OutletId      
                              , SC.SubCategoriesId      
                              , SC.Name AS SubCategoriesName      
                              , HC.Name AS HeadCategoriesName      
                            FROM MainHeads AS MH          
                            INNER JOIN HeadCategories AS HC          
                             ON HC.MainHeadsId = MH.MainHeadsId      
                            INNER JOIN SubCategories AS SC      
                             ON HC.HeadCategoriesId = SC.HeadCategoriesId      
                            WHERE SC.SubCategoriesId = {subCategoriesId}";

                return await _unit.DapperRepository.GetSingleQueryAsync<SubCategoriesResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostingAccountsResponse> AddPostingAccount(PostingAccountsRequest request)
        {
            try
            {
                var postingAccount = _mapper.Map<PostingAccounts>(request);

                _unit.PostingAccountsRepository.InsertSingle(postingAccount);
                var isSaved = await _unit.SaveAsync();

                if (isSaved)
                {
                    // Getting Posting Account by Id
                    return await GetSinglePostingAccountById(postingAccount.PostingAccountsId);
                    //
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new PostingAccountsResponse();
        }

        public async Task<PostingAccountsResponse> UpdatePostingAccounts(PostingAccountsRequest request)
        {
            try
            {
                var postingAccount = _mapper.Map<PostingAccounts>(request);

                _unit.PostingAccountsRepository.Update(postingAccount);
                var isUpdated = await _unit.SaveAsync();

                if (isUpdated)
                {
                    // Getting Posting Account by Id
                    return await GetSinglePostingAccountById(postingAccount.PostingAccountsId);
                    //
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new PostingAccountsResponse();
        }

        public async Task<bool> RemovePostingAccount(long postingAccountsId)
        {
            try
            {
                var voucherList = await _unit.VouchersDetailRepository.GetAsync(x => x.PostingAccountsId == postingAccountsId);
                if (voucherList?.Count() == 0)
                {
                    _unit.PostingAccountsRepository.DeleteById(postingAccountsId);

                    var isDeleted = await _unit.SaveAsync();
                    if (isDeleted)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public async Task<List<PostingAccountsResponse>> GetPostingAccountList(long outletId)
        {
            try
            {

                var query = $@"SELECT      
                                 PA.PostingAccountsid      
                                 , PA.Name AS PostingAccountsName      
                                 , PA.IsActive    
                                 , HC.HeadCategoriesId        
                                 , MH.Name AS MainHeadsName            
                                 , MH.MainHeadsId            
                                 , SC.SubCategoriesId        
                                 , SC.Name AS SubCategoriesName        
                                 , HC.Name AS HeadCategoriesName    
                                 , PA.OutletId        
                                 , PA.OpeningCredit        
                                 , PA.OpeningDebit        
                                 , PA.OpeningDate
                                 , ISNULL(PA.EmployeeId, 0) AS EmployeeId
                                FROM MainHeads AS MH            
                                INNER JOIN HeadCategories AS HC            
                                 ON HC.MainHeadsId = MH.MainHeadsId        
                                INNER JOIN SubCategories AS SC        
                                 ON HC.HeadCategoriesId = SC.HeadCategoriesId      
                                INNER JOIN PostingAccounts AS PA      
                                 ON PA.SubCategoriesId = SC.SubCategoriesId      
                                WHERE PA.OutletId = {outletId}";

                return await _unit.DapperRepository.GetListQueryAsync<PostingAccountsResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PostingAccountsResponse>> GetExpensePostingAccountList(long outletId)
        {
            try
            {
                var query = $@"SELECT  
                                 COA.PostingAccountsid        
                                 , COA.PostingAccountsName        
                                 , COA.IsActive      
                                 , COA.HeadCategoriesId          
                                 , COA.MainHeadsName              
                                 , COA.MainHeadsId              
                                 , COA.SubCategoriesId          
                                 , COA.SubCategoriesName          
                                 , COA.HeadCategoriesName           
                                 , COA.OutletId          
                                 , COA.OpeningCredit          
                                 , COA.OpeningDebit   
                                FROM V_GET_CHARTOFACCOUNTS AS COA  
                                WHERE OutletId = {outletId}
                                AND MainHeadsId = 5  
                                AND IsActive = 1 ";

                return await _unit.DapperRepository.GetListQueryAsync<PostingAccountsResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostingAccountsResponse> GetSinglePostingAccountById(long postingAccountId)
        {
            try
            {
                var query = $@"SELECT      
                                 PA.PostingAccountsid      
                                 , PA.Name AS PostingAccountsName      
                                 , PA.IsActive    
                                 , HC.HeadCategoriesId        
                                 , MH.Name AS MainHeadsName            
                                 , MH.MainHeadsId            
                                 , SC.SubCategoriesId        
                                 , SC.Name AS SubCategoriesName        
                                 , HC.Name AS HeadCategoriesName              
                                 , PA.OutletId  
                                 , PA.OpeningDebit  
                                 , PA.OpeningCredit  
                                 , PA.OpeningDate  
                                FROM MainHeads AS MH            
                                INNER JOIN HeadCategories AS HC            
                                 ON HC.MainHeadsId = MH.MainHeadsId        
                                INNER JOIN SubCategories AS SC        
                                 ON HC.HeadCategoriesId = SC.HeadCategoriesId      
                                INNER JOIN PostingAccounts AS PA      
                                 ON PA.SubCategoriesId = SC.SubCategoriesId      
                                WHERE PA.PostingAccountsId = {postingAccountId}";

                return await _unit.DapperRepository.GetSingleQueryAsync<PostingAccountsResponse>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
