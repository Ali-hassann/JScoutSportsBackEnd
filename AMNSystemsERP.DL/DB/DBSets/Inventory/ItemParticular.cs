using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Inventory
{
    public class ItemParticular
    {
        [Key]
        public long ItemParticularId { get; set; }
        public long ItemId { get; set; }
        public long ParticularId { get; set; }
        public long OutletId { get; set; }
        public decimal Price { get; set; }
    }
}