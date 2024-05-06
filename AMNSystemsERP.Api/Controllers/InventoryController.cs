using AMNSystemsERP.BL.Repositories.Inventory;
using AMNSystemsERP.CL.Models.InventoryModels;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/Inventory")]
    public class InventoryController : ApiController
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // ------------------ Inventory -----------------------------

        #region ItemTypes
        [HttpPost]
        [Route("AddItemType")]
        public async Task<ItemTypeRequest> AddItemType([FromBody] ItemTypeRequest request)
        {
            try
            {
                if (request?.OutletId > 0 && !string.IsNullOrEmpty(request.ItemTypeName))
                {
                    return await _inventoryService.AddItemType(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateItemType")]
        public async Task<ItemTypeRequest> UpdateItemType([FromBody] ItemTypeRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.ItemTypeId > 0
                    && !string.IsNullOrEmpty(request.ItemTypeName))
                {
                    return await _inventoryService.UpdateItemType(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveItemType")]
        public async Task<bool> RemoveItemType(long itemTypeId)
        {
            try
            {
                if (itemTypeId > 0)
                {
                    return await _inventoryService.RemoveItemType(itemTypeId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetItemTypeList")]
        public async Task<List<ItemTypeRequest>> GetItemTypeList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _inventoryService.GetItemTypeList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region ItemCategories
        [HttpPost]
        [Route("AddItemCategory")]
        public async Task<ItemCategoryRequest> AddItemCategory([FromBody] ItemCategoryRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && !string.IsNullOrEmpty(request.ItemCategoryName))
                {
                    return await _inventoryService.AddItemCategory(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateItemCategory")]
        public async Task<ItemCategoryRequest> UpdateItemCategory([FromBody] ItemCategoryRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.ItemCategoryId > 0
                    && !string.IsNullOrEmpty(request.ItemCategoryName))
                {
                    return await _inventoryService.UpdateItemCategory(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveItemCategory")]
        public async Task<bool> RemoveItemCategory(long itemCategoryId)
        {
            try
            {
                if (itemCategoryId > 0)
                {
                    return await _inventoryService.RemoveItemCategory(itemCategoryId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetItemCategoryList")]
        public async Task<List<ItemCategoryRequest>> GetItemCategoryList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _inventoryService.GetItemCategoryList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region  Units
        [HttpPost]
        [Route("AddUnit")]
        public async Task<UnitRequest> AddUnit([FromBody] UnitRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && !string.IsNullOrEmpty(request.UnitName))
                {
                    return await _inventoryService.AddUnit(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateUnit")]
        public async Task<UnitRequest> UpdateUnit([FromBody] UnitRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.UnitId > 0
                    && !string.IsNullOrEmpty(request.UnitName))
                {
                    return await _inventoryService.UpdateUnit(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveUnit")]
        public async Task<bool> RemoveUnit(int unitId)
        {
            try
            {
                if (unitId > 0)
                {
                    return await _inventoryService.RemoveUnit(unitId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetUnitList")]
        public async Task<List<UnitRequest>> GetUnitList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _inventoryService.GetUnitList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region Brand
        [HttpPost]
        [Route("AddBrand")]
        public async Task<BrandRequest> AddBrand([FromBody] BrandRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && !string.IsNullOrEmpty(request.BrandName))
                {
                    return await _inventoryService.AddBrand(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateBrand")]
        public async Task<BrandRequest> UpdateBrand([FromBody] BrandRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.BrandId > 0
                    && !string.IsNullOrEmpty(request.BrandName))
                {
                    return await _inventoryService.UpdateBrand(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveBrand")]
        public async Task<bool> RemoveBrand(long brandId)
        {
            try
            {
                if (brandId > 0)
                {
                    return await _inventoryService.RemoveBrand(brandId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetBrandList")]
        public async Task<List<BrandRequest>> GetBrandList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _inventoryService.GetBrandList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region  Item
        [HttpPost]
        [Route("AddItem")]
        public async Task<ItemRequest> AddItem([FromBody] ItemRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.ItemCategoryId > 0
                    && !string.IsNullOrEmpty(request.ItemName))
                {
                    return await _inventoryService.AddItem(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateItem")]
        public async Task<ItemRequest> UpdateItem([FromBody] ItemRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && request.ItemCategoryId > 0
                    && request.UnitId > 0
                    && !string.IsNullOrEmpty(request.ItemName))
                {
                    return await _inventoryService.UpdateItem(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveItem")]
        public async Task<bool> RemoveItem(long itemId)
        {
            try
            {
                if (itemId > 0)
                {
                    return await _inventoryService.RemoveItem(itemId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetItemList")]
        public async Task<List<ItemRequest>> GetItemList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _inventoryService.GetItemList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region Particulars
        [HttpPost]
        [Route("AddParticular")]
        public async Task<ParticularRequest> AddParticular([FromBody] ParticularRequest request)
        {
            try
            {
                if (request?.ParticularType > 0
                    && !string.IsNullOrEmpty(request.ParticularName))
                {
                    return await _inventoryService.AddParticular(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("UpdateParticular")]
        public async Task<ParticularRequest> UpdateParticular([FromBody] ParticularRequest request)
        {
            try
            {
                if (request?.ParticularId > 0
                    && request.ParticularType > 0
                    && !string.IsNullOrEmpty(request.ParticularName))
                {
                    return await _inventoryService.UpdateParticular(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveParticular")]
        public async Task<bool> RemoveParticular(long particularId)
        {
            try
            {
                if (particularId > 0)
                {
                    return await _inventoryService.RemoveParticular(particularId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetParticularList")]
        public async Task<List<ParticularRequest>> GetParticularList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _inventoryService.GetParticularList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region Bundle
        [HttpPost]
        [Route("SaveBundle")]
        public async Task<BundleRequest> SaveBundle([FromBody] BundleRequest request)
        {
            try
            {
                if (request?.OutletId > 0
                    && !string.IsNullOrEmpty(request.BundleName))
                {
                    return await _inventoryService.SaveBundle(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveBundle")]
        public async Task<bool> RemoveBundle(long BundleId)
        {
            try
            {
                if (BundleId > 0)
                {
                    return await _inventoryService.RemoveBundle(BundleId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetBundleList")]
        public async Task<List<BundleRequest>> GetBundleList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _inventoryService.GetBundleList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetBundleDetailById")]
        public async Task<List<ItemRequest>> GetBundleDetailById(long bundleId)
        {
            try
            {
                if (bundleId > 0)
                {
                    return await _inventoryService.GetBundleDetailById(bundleId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion

        #region ItemParticulars
        [HttpPost]
        [Route("SaveItemParticular")]
        public async Task<bool> SaveItemParticular([FromBody] List<ItemParticularRequest> requests)
        {
            try
            {
                if (requests?.Count > 0)
                {
                    return await _inventoryService.SaveItemParticular(requests);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpPost]
        [Route("DeleteItemParticularByItemId")]
        public async Task<bool> DeleteItemParticularByItemId([FromBody] long itemId)
        {
            try
            {
                if (itemId > 0)
                {
                    return await _inventoryService.DeleteItemParticularByItemId(itemId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetItemParticularList")]
        public async Task<List<ItemParticularRequest>> GetItemParticularList()
        {
            try
            {
                return await _inventoryService.GetItemParticularList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetItemParticularByItemId")]
        public async Task<List<ItemParticularRequest>> GetItemParticularByItemId(long itemId)
        {
            try
            {
                if (itemId > 0)
                {
                    return await _inventoryService.GetItemParticularByItemId(itemId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new List<ItemParticularRequest>();
        }
        #endregion

        #region ItemOpneing
        [HttpPost]
        [Route("SaveItemOpening")]
        public async Task<bool> SaveItemOpening([FromBody] List<ItemOpeningRequest> request)
        {
            try
            {
                if (request?.Count > 0)
                {
                    return await _inventoryService.SaveItemOpening(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        [HttpGet]
        [Route("GetItemOpeningList")]
        public async Task<List<ItemOpeningRequest>> GetItemOpeningList(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _inventoryService.GetItemOpeningList(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new List<ItemOpeningRequest>();
        }
        #endregion        
    }
}