using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Inventory
{
    public class Unit
    {
        [Key]
        public int UnitId { get; set; }
        public long OutletId { get; set; }
        [MaxLength(80)]
        public string UnitName { get; set; } = string.Empty;
    }
}
