using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Inventory
{
    public class ItemOpening 
    {
        [Key]
        public long ItemOpeningId { get; set; }
        public long ItemId { get; set; }
        public decimal OpeningQuantity { get; set; }
        public decimal OpeningPrice { get; set; }
        public long OutletId { get; set; }

        public virtual Item Item { get; set; }
    }
}