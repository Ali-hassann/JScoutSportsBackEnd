using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Accounts
{
    public class HeadCategories : Entity
    {
        public HeadCategories()
        {
            SubCategories = new HashSet<SubCategories>();
        }

        [Key]
        public long HeadCategoriesId { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [Required]
        public int MainHeadsId { get; set; }
        [Required]
        public long OutletId { get; set; }

        public virtual MainHeads MainHeads { get; set; }

        public virtual ICollection<SubCategories> SubCategories { get; set; }
    }
}