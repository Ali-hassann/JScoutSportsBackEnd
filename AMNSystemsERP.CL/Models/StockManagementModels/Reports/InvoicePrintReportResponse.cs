using AMNSystemsERP.CL.Enums;
using System.Drawing;



namespace AMNSystemsERP.CL.Models.StockManagementModels.Reports
{
    public class InvoicePrintReportResponse
    {
        public long InvoiceMasterId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long ParticularId { get; set; }
        public string ParticularName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string ReferenceNo { get; set; }
        public InventoryDocumentType DocumentTypeId { get; set; }
        public string DocumentTypeName { get { return DocumentTypeId.ToString(); } }
        public string Remarks { get; set; }
        public string InvoiceStatus { get; set; }
        public long OrderMasterId { get; set; }
        public string OrderName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidReceivedAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public long OutletId { get; set; }
        public string OutletName { get; set; }
        public string OutletAddress { get; set; }
        public long OrganizationId { get; set; }
        public string InvoiceSerialNo { get; set; }
        public long SerialIndex { get; set; }

        /*Details*/
        public long InvoiceDetailId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string BarCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal OpeningQuantity { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal AverageRate { get; set; }
        public decimal RunningTotal { get; set; }
        public decimal RunningAmount { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string OrderQuantity { get; set; }

        public Color ItemColor
        {
            get
            {
                return System.Drawing.ColorTranslator.FromHtml(Color);
            }
        }

        public string PartNo { get; set; }

        public long TypeId { get; set; }
        public string TypeName { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal LastPurchasePrice { get; set; }
        public decimal ReorderLevel { get; set; }

        // Purchase Order Feilds
        public long PurchaseOrderMasterId { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        //
    }
}
