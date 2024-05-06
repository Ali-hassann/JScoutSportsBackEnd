using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Production
{
    public class ProductCategory
    {
        [Key]
        public long ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public long OutletId { get; set; }
    }
}