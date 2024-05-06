using AMNSystemsERP.CL.Enums.InventoryEnums;

namespace AMNSystemsERP.CL.Models.StockManagementModels
{
    public class InvoiceDetailRequest
    {
        public long InvoiceDetailId { get; set; }
        public long InvoiceMasterId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string UnitName { get; set; }
        public string ItemCategoryName { get; set; }
        public string ItemTypeName { get; set; }
        public string BarCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }

        public decimal AverageRate { get; set; }
        public decimal RunningTotal { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
