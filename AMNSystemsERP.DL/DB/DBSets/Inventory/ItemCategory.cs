using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Inventory
{
    public class ItemCategory
    {
        [Key]
        public long ItemCategoryId { get; set; }
        public long ItemTypeId { get; set; }
        [MaxLength(80)]
        public string ItemCategoryName { get; set; }
        public long OutletId { get; set; }
    }
}