using AMNSystemsERP.DL.DB.DBSets.Inventory;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.StockManagement
{
    /*
     QuantityIn............. Purchase + Sale Return
     QuantityOut.............Sale + Purchase Return
     same as for Price IN and Price Out
     */

    public class InvoiceDetail
    {
        [Key]
        public long InvoiceDetailId { get; set; }
        public long InvoiceMasterId { get; set; }
        public string Description { get; set; }
        public long ItemId { get; set; }
        public string BarCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal AverageRate { get; set; }
        public decimal RunningTotal { get; set; }
        public int TransactionType { get; set; }

        public virtual InvoiceMaster InvoiceMaster { get; set; }
        public virtual Item Item { get; set; }
    }
}