using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Inventory
{
    public class Particular
    {
        [Key]
        public long ParticularId { get; set; }
        [MaxLength(80)]
        public string ParticularName { get; set; } = string.Empty;
        public string? RepresentativeName { get; set; }
        public string? MainProductName { get; set; }
        public int ParticularType { get; set; }
        public string ContactNo { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}