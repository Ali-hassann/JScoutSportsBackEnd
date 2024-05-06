using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Production
{
    public class ProductOpening : Entity
    {
        [Key]
        public long ProductOpeningId { get; set; }
        public long ProductId { get; set; }
        public decimal OpeningQuantity { get; set; }
        public decimal OpeningPrice { get; set; }
        public long OutletId { get; set; }

        public virtual Product Product { get; set; }
    }
}