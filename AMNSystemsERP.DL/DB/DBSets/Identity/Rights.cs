using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Identity
{
    public class Rights
    {
        [Key]
        public long RightsId { get; set; }
        [MaxLength(50)]
        [Required]
        public string RightsName { get; set; }
        [MaxLength(50)]
        [Required]
        public string Description { get; set; }
        [MaxLength(30)]
        [Required]
        public string RightsArea { get; set; }
    }
}
