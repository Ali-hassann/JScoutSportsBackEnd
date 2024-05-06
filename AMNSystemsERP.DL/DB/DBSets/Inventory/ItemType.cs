using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Inventory
{
    public class ItemType
    {
        public ItemType()
        {
            ItemCategories = new HashSet<ItemCategory>();
        }

        [Key]
        public long ItemTypeId { get; set; }
        [MaxLength(80)]
        public string ItemTypeName { get; set; }
        public long OutletId { get; set; }
        public virtual ICollection<ItemCategory> ItemCategories { get; set; }
    }
}
