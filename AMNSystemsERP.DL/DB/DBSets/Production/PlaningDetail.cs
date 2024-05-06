using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Production
{
    public class PlaningDetail
    {
        [Key]
        public long PlaningDetailId { get; set; }
        public long ItemId { get; set; }
        public int UnitId { get; set; }
        public long PlaningMasterId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsManualPrice { get; set; }

        public virtual PlaningMaster PlaningMaster { get; set; }
    }
}
