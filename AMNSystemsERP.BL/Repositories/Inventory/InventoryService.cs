using AMNSystemsERP.CL.Enums.InventoryEnums;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Models.InventoryModels;
using AMNSystemsERP.DL.DB.DBSets.Accounts;
using AMNSystemsERP.DL.DB.DBSets.Inventory;
using AutoMapper;
using System.Data;

namespace AMNSystemsERP.BL.Repositories.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public InventoryService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        #region  Types
        public async Task<ItemTypeRequest> AddItemType(ItemTypeRequest request)
        {
            try
            {
                var type = _mapper.Map<ItemType>(request);

                _unit.ItemTypeRepository.InsertSingle(type);

                if (await _unit.SaveAsync())
                {
                    request.ItemTypeId = type.ItemTypeId;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<ItemTypeRequest> UpdateItemType(ItemTypeRequest request)
        {
            try
            {
                var type = _mapper.Map<ItemType>(request);

                _unit.ItemTypeRepository.Update(type);

                if (await _unit.SaveAsync())
                {
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ItemTypeRequest();
        }

        public async Task<bool> RemoveItemType(long typeId)
        {
            try
            {
                _unit.ItemTypeRepository.DeleteById(typeId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ItemTypeRequest>> GetItemTypeList(long outletId)
        {
            try
            {
                var typesList = (await _unit
                                      .ItemTypeRepository
                                      .GetAsync(c => c.OutletId == outletId))
                                      ?.ToList();

                if (typesList?.Count > 0)
                {
                    return _mapper.Map<List<ItemTypeRequest>>(typesList);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region Categories
        public async Task<ItemCategoryRequest> AddItemCategory(ItemCategoryRequest request)
        {
            try
            {
                var invCategory = _mapper.Map<ItemCategory>(request);

                _unit.ItemCategoryRepository.InsertSingle(invCategory);
                var isSaved = await _unit.SaveAsync();

                if (isSaved)
                {
                    return await GetCategoryById(invCategory.ItemCategoryId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ItemCategoryRequest();
        }

        public async Task<ItemCategoryRequest> UpdateItemCategory(ItemCategoryRequest request)
        {
            try
            {
                var category = _mapper.Map<ItemCategory>(request);

                _unit.ItemCategoryRepository.Update(category);

                var isUpdated = await _unit.SaveAsync();
                if (isUpdated)
                {
                    return await GetCategoryById(category.ItemCategoryId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ItemCategoryRequest();
        }

        public async Task<bool> RemoveItemCategory(long categoryId)
        {
            try
            {
                _unit.ItemCategoryRepository.DeleteById(categoryId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ItemCategoryRequest>> GetItemCategoryList(long outletId)
        {
            try
            {
                var query = $@"SELECT 
	                                IC.ItemCategoryId
	                                , IC.ItemCategoryName
	                                , IC.OutletId
	                                , IC.ItemTypeId
	                                , IT.ItemTypeName
                                FROM ItemCategory AS IC
                                INNER JOIN ItemType AS IT
	                                ON IT.ItemTypeId = IC.ItemTypeId
                                WHERE IC.OutletId = {outletId}";

                return await _unit.DapperRepository.GetListQueryAsync<ItemCategoryRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ItemCategoryRequest> GetCategoryById(long categoriesId)
        {
            try
            {
                var query = $@"SELECT 
	                                IC.ItemCategoryId
	                                , IC.ItemCategoryName
	                                , IC.OutletId
	                                , IC.ItemTypeId
	                                , IT.ItemTypeName
                                FROM ItemCategory AS IC
                                INNER JOIN ItemType AS IT
	                                ON IT.ItemTypeId = IC.ItemTypeId
                                WHERE IC.ItemCategoryId = {categoriesId}";

                return await _unit.DapperRepository.GetSingleQueryAsync<ItemCategoryRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Units
        public async Task<UnitRequest> AddUnit(UnitRequest request)
        {
            try
            {
                var unit = _mapper.Map<Unit>(request);

                _unit.UnitRepository.InsertSingle(unit);

                if (await _unit.SaveAsync())
                {
                    request.UnitId = unit.UnitId;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new UnitRequest();
        }

        public async Task<UnitRequest> UpdateUnit(UnitRequest request)
        {
            try
            {
                var unit = _mapper.Map<Unit>(request);

                _unit.UnitRepository.Update(unit);

                if (await _unit.SaveAsync())
                {
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new UnitRequest();
        }

        public async Task<bool> RemoveUnit(int unitId)
        {
            try
            {
                _unit.UnitRepository.DeleteById(unitId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UnitRequest>> GetUnitList(long outletId)
        {
            try
            {
                var unitList = (await _unit
                                      .UnitRepository
                                      .GetAsync(c => c.OutletId == outletId)
                               )?.ToList();

                if (unitList?.Count > 0)
                {
                    return _mapper.Map<List<UnitRequest>>(unitList);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region Brands
        public async Task<BrandRequest> AddBrand(BrandRequest request)
        {
            try
            {
                var brand = _mapper.Map<Brand>(request);

                _unit.BrandRepository.InsertSingle(brand);
                if (await _unit.SaveAsync())
                {
                    request.BrandId = brand.BrandId;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new BrandRequest();
        }

        public async Task<BrandRequest> UpdateBrand(BrandRequest request)
        {
            try
            {
                _unit.BrandRepository.Update(_mapper.Map<Brand>(request));
                if (await _unit.SaveAsync())
                {
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new BrandRequest();
        }

        public async Task<bool> RemoveBrand(long brandId)
        {
            try
            {
                _unit.BrandRepository.DeleteById(brandId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BrandRequest>> GetBrandList(long outletId)
        {
            try
            {
                var brands = (await _unit
                                    .BrandRepository
                                    .GetAsync(c => c.OutletId == outletId)
                                    )?.ToList();

                if (brands?.Count > 0)
                {
                    return _mapper.Map<List<BrandRequest>>(brands);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region Items
        public async Task<ItemRequest> AddItem(ItemRequest request)
        {
            try
            {
                var item = _mapper.Map<Item>(request);

                _unit.ItemRepository.InsertSingle(item);

                if (await _unit.SaveAsync())
                {
                    return await GetItemById(item.ItemId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ItemRequest();
        }

        public async Task<ItemRequest> UpdateItem(ItemRequest request)
        {
            try
            {
                var item = _mapper.Map<Item>(request);
                _unit.ItemRepository.Update(item);
                var isUpdated = await _unit.SaveAsync();

                if (isUpdated)
                {
                    return await GetItemById(item.ItemId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ItemRequest();
        }

        public async Task<bool> RemoveItem(long itemId)
        {
            try
            {
                _unit.ItemRepository.DeleteById(itemId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ItemRequest>> GetItemList(long outletId)
        {
            try
            {
                var query = $@"SELECT 
	                                I.*
                                FROM V_Item AS I
                                WHERE I.OutletId = {outletId}";

                return await _unit.DapperRepository.GetListQueryAsync<ItemRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ItemRequest> GetItemById(long itemId)
        {
            try
            {
                var query = $@"SELECT 
	                                I.*
	                                , C.ItemCategoryName
	                                , U.UnitName
                                FROM Item AS I
                                INNER JOIN Unit AS U
	                                ON I.UnitId = U.UnitId
                                INNER JOIN ItemCategory AS C
	                                ON I.ItemCategoryId = C.ItemCategoryId
                                WHERE I.ItemId = {itemId}";

                var item = await _unit.DapperRepository.GetSingleQueryAsync<ItemRequest>(query);
                if (item?.ItemId > 0)
                {
                    return item;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ItemRequest();
        }
        #endregion

        #region Particulars
        public async Task<ParticularRequest> AddParticular(ParticularRequest request)
        {
            try
            {
                var model = _mapper.Map<Particular>(request);

                _unit.ParticularRepository.InsertSingle(model);
                var isSaved = await _unit.SaveAsync();
                if (isSaved && request.ParticularType != ParticularType.Others)
                {
                    request.ParticularId = model.ParticularId;
                    isSaved = await AddUpdateParticularAccount(request);
                }
                if (isSaved)
                {
                    return await GetParticularById(model.ParticularId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ParticularRequest();
        }

        private async Task<bool> AddUpdateParticularAccount(ParticularRequest request)
        {
            var configurationList = (await _unit
                                           .ConfigurationSettingRepository
                                           .GetAsync(c => c.OutletId == request.OutletId)
                                           )?.ToList() ?? new List<ConfigurationSetting>();
            long subCategoriesId = 0;
            var postingAccountToAdd = new PostingAccounts();
            postingAccountToAdd.IsActive = true;
            postingAccountToAdd.ParticularId = request.ParticularId;
            postingAccountToAdd.Name = request.ParticularName;
            postingAccountToAdd.OutletId = request.OutletId;
            postingAccountToAdd.OpeningDate = DateHelper.GetCurrentDate();

            if (request.ParticularType == ParticularType.Customer && configurationList?.Count > 0)
            {
                subCategoriesId = configurationList?.FirstOrDefault(s => s.AccountName == "Debtors")?.AccountValue ?? 0;
                postingAccountToAdd.OpeningDebit = request.DebitAmount;
                postingAccountToAdd.OpeningCredit = request.CreditAmount;
            }
            else
            {
                subCategoriesId = configurationList?.FirstOrDefault(s => s.AccountName == "Creditors")?.AccountValue ?? 0;
                postingAccountToAdd.OpeningCredit = request.CreditAmount;
                postingAccountToAdd.OpeningDebit = request.DebitAmount;
            }

            postingAccountToAdd.SubCategoriesId = subCategoriesId; // set from configuration

            var postingAccountAlreadyExist = await _unit
                                                   .PostingAccountsRepository
                                                   .GetSingleAsync(d => (d.ParticularId ?? 0) == request.ParticularId);
            if (postingAccountAlreadyExist?.PostingAccountsId > 0
                && (postingAccountAlreadyExist?.ParticularId ?? 0) > 0
                && (postingAccountAlreadyExist?.ParticularId ?? 0) == request.ParticularId)
            {
                postingAccountAlreadyExist.Name = postingAccountToAdd.Name;
                postingAccountAlreadyExist.OpeningCredit = postingAccountToAdd.OpeningCredit;
                postingAccountAlreadyExist.OpeningDebit = postingAccountToAdd.OpeningDebit;
                postingAccountAlreadyExist.OpeningDate = postingAccountToAdd.OpeningDate;
                postingAccountAlreadyExist.IsActive = true;
                postingAccountAlreadyExist.OutletId = request.OutletId;
                _unit.PostingAccountsRepository.Update(postingAccountAlreadyExist);
            }
            else
            {
                _unit.PostingAccountsRepository.InsertSingle(postingAccountToAdd);
            }

            return await _unit.SaveAsync();
        }

        public async Task<ParticularRequest> UpdateParticular(ParticularRequest request)
        {
            try
            {
                var model = _mapper.Map<Particular>(request);

                _unit.ParticularRepository.Update(model);

                var isUpdated = await _unit.SaveAsync();
                if (isUpdated && request.ParticularType != ParticularType.Others)
                {
                    isUpdated = await AddUpdateParticularAccount(request);
                }

                if (isUpdated)
                {
                    return await GetParticularById(model.ParticularId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new ParticularRequest();
        }

        public async Task<bool> RemoveParticular(long particularId)
        {
            try
            {
                _unit.ParticularRepository.DeleteById(particularId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ParticularRequest>> GetParticularList(long outletId)
        {
            try
            {
                var query = $@"SELECT 
	                                P.ParticularId
	                                , P.Address
	                                , P.ContactNo
	                                , P.Email
	                                , P.ParticularName
	                                , P.RepresentativeName
	                                , P.MainProductName
	                                , P.ParticularType
                                FROM Particular AS P";

                return await _unit.DapperRepository.GetListQueryAsync<ParticularRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ParticularRequest> GetParticularById(long particularId)
        {
            try
            {
                var query = $@"SELECT 
	                                P.ParticularId
	                                , P.Address
	                                , P.ContactNo
	                                , P.Email
	                                , P.ParticularName
	                                , P.RepresentativeName
	                                , P.MainProductName
	                                , P.ParticularType
                                FROM Particular AS P
                                WHERE P.ParticularId = {particularId}";

                var particular = await _unit.DapperRepository.GetSingleQueryAsync<ParticularRequest>(query);

                if (particular?.ParticularId > 0)
                {
                    return particular;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region Bundles
        public async Task<BundleRequest> SaveBundle(BundleRequest request)
        {
            try
            {
                var bundle = _mapper.Map<Bundle>(request);
                bundle.BundleDetails = new List<BundleDetail>();
                request.Items.ForEach(x =>
                {
                    var BundleDetail = new BundleDetail();
                    BundleDetail.BundleDetailId = 0;
                    BundleDetail.BundleId = request.BundleId;
                    BundleDetail.ItemId = x.ItemId;
                    BundleDetail.Quantity = x.Quantity;
                    bundle.BundleDetails.Add(BundleDetail);
                });

                if (request.BundleId > 0)
                {
                    var details = await _unit
                                        .BundleDetailRepository
                                        .GetAsync(x => x.BundleId == request.BundleId);
                    _unit.BundleDetailRepository.DeleteRangeEntities(details.ToList());

                    _unit.BundleRepository.Update(bundle);
                }
                else
                {
                    _unit.BundleRepository.InsertSingle(bundle);
                }
                if (await _unit.SaveAsync())
                {
                    request.BundleId = bundle.BundleId;
                    request.ItemCount = request.Items?.Count ?? 0;
                    return request;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<bool> RemoveBundle(long bundleId)
        {
            try
            {
                var pakagesDetailsToDelete = await _unit
                                                   .BundleDetailRepository
                                                   .GetAsync(x => x.BundleId == bundleId);

                _unit.BundleDetailRepository.DeleteRangeEntities(pakagesDetailsToDelete.ToList());
                _unit.BundleRepository.DeleteById(bundleId);

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ItemRequest>> GetBundleDetailById(long bundleId)
        {
            try
            {
                var query = $@"SELECT 
	                                BD.BundleId
	                                , BD.ItemId
	                                , BD.Quantity
	                                , I.ItemName
	                                , I.IsActive
	                                , I.PartNo
	                                , I.PurchasePrice
	                                , I.ReorderLevel
	                                , I.UnitId
	                                , I.SalePrice
	                                , I.OutletId
	                                , U.UnitName
	                                , IC.ItemCategoryName
                                FROM Bundle AS B
                                INNER JOIN BundleDetail AS BD
	                                ON BD.BundleId = B.BundleId
                                INNER JOIN Item AS I
	                                ON I.ItemId = BD.ItemId
                                INNER JOIN ItemCategory AS IC
	                                ON IC.ItemCategoryId = I.ItemCategoryId
                                INNER JOIN Unit AS U
	                                ON U.UnitId = I.UnitId
                                WHERE B.BundleId = {bundleId}";

                return await _unit.DapperRepository.GetListQueryAsync<ItemRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BundleRequest>> GetBundleList(long outletId)
        {
            try
            {
                var query = $@"SELECT 
	                                B.BundleId
	                                , B.BundleName
	                                , B.Description
	                                , B.OutletId
	                                , COUNT(BD.BundleDetailId) AS ItemCount
                                FROM Bundle AS B
                                INNER JOIN BundleDetail AS BD
	                                ON BD.BundleId = B.BundleId
                                WHERE B.OutletId = {outletId}
                                GROUP BY
	                                B.BundleId
	                                , B.BundleName
	                                , B.Description
	                                , B.OutletId";

                return await _unit.DapperRepository.GetListQueryAsync<BundleRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ItemParticulars
        public async Task<bool> SaveItemParticular(List<ItemParticularRequest> requests)
        {
            try
            {
                var itemId = requests.Select(c => c.ItemId).Distinct().Single();

                var particularsToDelete = (await _unit
                                                 .ItemParticularRepository
                                                 .GetAsync(x => x.ItemId == itemId))
                                                 ?.ToList();

                if (particularsToDelete?.Count > 0)
                {
                    _unit.ItemParticularRepository.DeleteRangeEntities(particularsToDelete);
                }

                requests.ForEach(data =>
                {
                    var itemParticular = _mapper.Map<ItemParticular>(data);
                    _unit.ItemParticularRepository.InsertSingle(itemParticular);
                });

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public async Task<bool> DeleteItemParticularByItemId(long itemId)
        {
            try
            {
                var dataToDelete = await _unit
                                         .ItemParticularRepository
                                         .GetAsync(x => x.ItemId == itemId);

                _unit.ItemParticularRepository.DeleteRangeEntities(dataToDelete.ToList());

                return await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ItemParticularRequest>> GetItemParticularList()
        {
            try
            {
                var query = @$"SELECT 
	                                I.ItemId
	                                , I.ItemName
	                                , I.OutletId
	                                , I.PurchasePrice
	                                , I.SalePrice
	                                , IC.ItemCategoryName
	                                , U.UnitName
	                                , COUNT(P.ParticularId) AS TotalParticulars
	                                , ISNULL(STRING_AGG(P.ParticularName +'('+CAST(CAST(IP.Price as decimal(18,2)) as varchar)+')', ',') WITHIN GROUP (ORDER BY I.ItemId ASC), 'No vendor') AS ParticularDetail
                                FROM Item AS I
                                INNER JOIN ItemCategory AS IC
	                                ON IC.ItemCategoryId = I.ItemCategoryId
                                INNER JOIN Unit AS U
	                                ON U.UnitId = I.UnitId
                                LEFT JOIN ItemParticular AS IP
	                                ON IP.ItemId = I.ItemId
                                LEFT JOIN Particular AS P
	                                ON P.ParticularId = IP.ParticularId
                                GROUP BY
	                                I.ItemId
	                                , I.ItemName
	                                , I.OutletId
	                                , I.PurchasePrice
	                                , I.SalePrice
	                                , IC.ItemCategoryName
	                                , U.UnitName";

                return await _unit.DapperRepository.GetListQueryAsync<ItemParticularRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ItemParticularRequest>> GetItemParticularByItemId(long itemId)
        {
            try
            {
                var query = @$"SELECT 
	                                I.ItemId
	                                , I.ItemName
	                                , I.OutletId
	                                , I.PurchasePrice
	                                , I.SalePrice
	                                , IC.ItemCategoryName
	                                , U.UnitName
	                                , IP.ItemParticularId
	                                , P.ParticularId
	                                , P.ParticularName
	                                , IP.Price
                                    , P.ContactNo
                                FROM Item AS I
                                INNER JOIN ItemCategory AS IC
	                                ON IC.ItemCategoryId = I.ItemCategoryId
                                INNER JOIN Unit AS U
	                                ON U.UnitId = I.UnitId
                                INNER JOIN ItemParticular AS IP
	                                ON IP.ItemId = I.ItemId
                                INNER JOIN Particular AS P
	                                ON P.ParticularId = IP.ParticularId
                                WHERE IP.ItemId = {itemId}";

                return await _unit.DapperRepository.GetListQueryAsync<ItemParticularRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Item Opening
        public async Task<bool> SaveItemOpening(List<ItemOpeningRequest> request)
        {
            try
            {
                var itemOpeningToInsert = new List<ItemOpening>();
                var itemOpeningToUpdate = new List<ItemOpening>();

                request.ForEach(i =>
                {

                    var itemOpening = _mapper.Map<ItemOpening>(i);
                    if (itemOpening.ItemOpeningId == 0 && (itemOpening.OpeningPrice > 0 || itemOpening.OpeningQuantity > 0))
                    {
                        itemOpeningToInsert.Add(itemOpening);
                    }
                    else if (itemOpening.ItemOpeningId > 0)
                    {
                        itemOpeningToUpdate.Add(itemOpening);
                    }
                });

                if (itemOpeningToInsert.Count > 0 || itemOpeningToUpdate.Count > 0)
                {
                    if (itemOpeningToUpdate.Count > 0)
                    {
                        _unit.ItemOpeningRepository.UpdateList(itemOpeningToUpdate);
                    }

                    if (itemOpeningToInsert.Count > 0)
                    {
                        _unit.ItemOpeningRepository.InsertList(itemOpeningToInsert);
                    }
                    return await _unit.SaveAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public async Task<List<ItemOpeningRequest>> GetItemOpeningList(long outletId)
        {
            try
            {
                var query = @$"SELECT 
	                                ISNULL(IO.ItemOpeningId,0) AS ItemOpeningId
									, ISNULL(IO.OpeningPrice,0) AS OpeningPrice
									, ISNULL(IO.OpeningQuantity,0) AS OpeningQuantity
									, I.ItemId
	                                , I.OutletId
	                                , I.ItemName
	                                , IC.ItemCategoryName
	                                , U.UnitName
                                FROM Item AS I
                                INNER JOIN ItemCategory AS IC
	                                ON IC.ItemCategoryId = I.ItemCategoryId
                                INNER JOIN Unit AS U
	                                ON U.UnitId = I.UnitId
								LEFT JOIN ItemOpening AS IO
									ON IO.ItemId = I.ItemId
									AND IO.OutletId = I.OutletId
                                WHERE I.OutletId = {outletId}";

                return await _unit.DapperRepository.GetListQueryAsync<ItemOpeningRequest>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
