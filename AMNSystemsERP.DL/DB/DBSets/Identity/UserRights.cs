using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Identity
{
    public class UserRights
    {
        [Key]
        public long UserRightsId { get; set; }
        [Required]
        public long RightsId { get; set; }
        [MaxLength(50)]
        [Required]
        public string Id { get; set; }
        [Required]
        public bool HasAccess { get; set; }

        public virtual Rights Rights { get; set; }
    }
}
