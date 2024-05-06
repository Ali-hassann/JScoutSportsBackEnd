using AMNSystemsERP.CL.Models.InventoryModels;

namespace AMNSystemsERP.BL.Repositories.Inventory
{
    public interface IInventoryService
    {
        // ----------------------------------------------------------------------------
        // ------------------------------- Inventory ----------------------------------
        // ----------------------------------------------------------------------------

        #region ItemTypes
        Task<ItemTypeRequest> AddItemType(ItemTypeRequest request);
        Task<ItemTypeRequest> UpdateItemType(ItemTypeRequest request);
        Task<bool> RemoveItemType(long itemTypeId);
        Task<List<ItemTypeRequest>> GetItemTypeList(long outletId);
        #endregion

        #region  Categories
        Task<ItemCategoryRequest> AddItemCategory(ItemCategoryRequest request);
        Task<ItemCategoryRequest> UpdateItemCategory(ItemCategoryRequest request);
        Task<bool> RemoveItemCategory(long id);
        Task<List<ItemCategoryRequest>> GetItemCategoryList(long outletId);
        Task<ItemCategoryRequest> GetCategoryById(long id);
        #endregion

        #region  Unit
        Task<UnitRequest> AddUnit(UnitRequest request);
        Task<UnitRequest> UpdateUnit(UnitRequest request);
        Task<bool> RemoveUnit(int id);
        Task<List<UnitRequest>> GetUnitList(long outletId);
        #endregion

        #region Brands
        Task<BrandRequest> AddBrand(BrandRequest request);
        Task<BrandRequest> UpdateBrand(BrandRequest request);
        Task<bool> RemoveBrand(long brandId);
        Task<List<BrandRequest>> GetBrandList(long outletId);
        #endregion

        #region Item
        Task<ItemRequest> AddItem(ItemRequest request);
        Task<ItemRequest> UpdateItem(ItemRequest request);
        Task<bool> RemoveItem(long id);
        Task<List<ItemRequest>> GetItemList(long outletId);
        Task<ItemRequest> GetItemById(long id);
        #endregion

        #region Particulars
        Task<ParticularRequest> AddParticular(ParticularRequest request);
        Task<ParticularRequest> UpdateParticular(ParticularRequest request);
        Task<bool> RemoveParticular(long id);
        Task<List<ParticularRequest>> GetParticularList(long outletId);
        Task<ParticularRequest> GetParticularById(long particularId);
        #endregion

        #region Bundle
        Task<BundleRequest> SaveBundle(BundleRequest request);
        Task<bool> RemoveBundle(long bundleId);
        Task<List<ItemRequest>> GetBundleDetailById(long bundleId);
        Task<List<BundleRequest>> GetBundleList(long outletId);
        #endregion

        #region Item & Particular

        Task<bool> SaveItemParticular(List<ItemParticularRequest> requests);
        Task<bool> DeleteItemParticularByItemId(long itemId);
        Task<List<ItemParticularRequest>> GetItemParticularList();
        Task<List<ItemParticularRequest>> GetItemParticularByItemId(long itemId);
        #endregion

        #region ItemOpening
        Task<bool> SaveItemOpening(List<ItemOpeningRequest> request);
        Task<List<ItemOpeningRequest>> GetItemOpeningList(long outletId);

        #endregion
    }
}