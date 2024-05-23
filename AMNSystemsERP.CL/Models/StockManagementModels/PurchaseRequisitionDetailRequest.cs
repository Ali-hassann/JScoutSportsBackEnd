namespace AMNSystemsERP.CL.Models.StockManagementModels
{
    public class PurchaseRequisitionDetailRequest
    {
        public long PurchaseRequisitionDetailId { get; set; }

        public long PurchaseRequisitionMasterId { get; set; }

        public long ItemId { get; set; }

        public string ItemName { get; set; } 

        public string UnitName { get; set; }

        public string CategoryName { get; set; }

        public string TypeName { get; set; }

        public string BarCode { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Total { get; set; }
    }
}