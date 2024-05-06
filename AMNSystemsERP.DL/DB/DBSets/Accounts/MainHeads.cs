using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Accounts
{
    public class MainHeads
    {
        public MainHeads()
        {
            HeadCategories = new HashSet<HeadCategories>();
        }

        [Key]
        public int MainHeadsId { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<HeadCategories> HeadCategories { get; set; }
    }
}