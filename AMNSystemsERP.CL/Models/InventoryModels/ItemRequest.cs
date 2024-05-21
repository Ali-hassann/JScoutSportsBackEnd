namespace AMNSystemsERP.CL.Models.InventoryModels
{
    public class ItemRequest
    {
        public long ItemId { get; set; }
        public long ItemCategoryId { get; set; }
        public int UnitId { get; set; }
        public string ItemName { get; set; }
        public string PartNo { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string GSM { get; set; }
        public int RackNo { get; set; }
        public string RowNo { get; set; }
        public string BinNo { get; set; }
        public string LineNumber { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal ReorderLevel { get; set; }
        public decimal BalanceQuantity { get; set; }
        public decimal LastPrice { get; set; }
        public bool IsActive { get; set; }
        public string ItemCategoryName { get; set; }
        public string UnitName { get; set; }
        public long OutletId { get; set; }
        public long BundleId { get; set; }
        public long ItemTypeId { get; set; }
        public string ItemTypeName { get; set; }
        public long PlaningMasterId { get; set; } // for planing and bundle detail
        public decimal Quantity { get; set; } // for planing and bundle detail
        public decimal Price { get; set; } // for planing
        public long ProductId { get; set; } // for production detail
        public bool IsManualPrice { get; set; } // for production detail
    }
}