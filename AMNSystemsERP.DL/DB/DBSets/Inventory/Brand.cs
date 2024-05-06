using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Inventory
{
    public class Brand
    {
        [Key]
        public long BrandId { get; set; }
        public string BrandName { get; set; }
        public long OutletId { get; set; }
    }
}