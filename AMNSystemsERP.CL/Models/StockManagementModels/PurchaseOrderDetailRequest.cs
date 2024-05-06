namespace AMNSystemsERP.CL.Models.StockManagementModels
{
    public class PurchaseOrderDetailRequest
    {
        public long PurchaseOrderDetailId { get; set; }

        public long PurchaseOrderMasterId { get; set; }

        public long ItemId { get; set; }

        public string ItemName { get; set; }

        public string UnitName { get; set; }

        public string ItemCategoryName { get; set; }

        public string ItemTypeName { get; set; }

        public string? BarCode { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }
        public decimal Amount { get { return Price * Quantity; } }
    }
}