using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Organization
{
    public class Organization : Entity
    {
        public Organization()
        {
            Outlets = new HashSet<Outlet>();
        }

        [Key]
        public long OrganizationId { get; set; }
        [MaxLength(50)]
        [Required]
        public string OrganizationName { get; set; }
        [MaxLength(50)]
        [Required]
        public string OwnerName { get; set; }
        public string FoundedYear { get; set; }
        [MaxLength(250)]
        [Required]
        public string ImagePath { get; set; }

        public virtual ICollection<Outlet> Outlets { get; set; }
    }
}
