using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Production
{
    public class ProductSize
    {
        [Key]
        public int ProductSizeId { get; set; }
        [MaxLength(20)]
        public string ProductSizeName { get; set; } = string.Empty;
    }
}
