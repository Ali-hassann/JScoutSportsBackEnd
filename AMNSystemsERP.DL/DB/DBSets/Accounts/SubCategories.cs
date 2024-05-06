using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Accounts
{
    public class SubCategories : Entity
    {
        public SubCategories()
        {
            PostingAccounts = new HashSet<PostingAccounts>();
        }

        [Key]
        public long SubCategoriesId { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [Required]
        public long HeadCategoriesId { get; set; }
        [Required]
        public long OutletId { get; set; }

        public virtual HeadCategories HeadCategories { get; set; }

        public virtual ICollection<PostingAccounts> PostingAccounts { get; set; }
    }
}