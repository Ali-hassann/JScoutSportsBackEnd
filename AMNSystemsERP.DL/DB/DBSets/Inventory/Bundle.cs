using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Inventory
{
    public class Bundle
    {
        public Bundle()
        {
            BundleDetails = new HashSet<BundleDetail>();
        }

        [Key]
        public long BundleId { get; set; }
        [Required]
        [MaxLength(100)]
        public string BundleName { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        public long OutletId { get; set; }

        public virtual ICollection<BundleDetail> BundleDetails { get; set; }
    }
}
