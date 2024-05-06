using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Production
{
    public class PlaningMaster : Entity
    {
        public PlaningMaster()
        {
            PlaningDetails = new HashSet<PlaningDetail>();
        }

        [Key]
        public long PlaningMasterId { get; set; }
        public long ProductId { get; set; }
        public int ProductSizeId { get; set; }
        public long OrderMasterId { get; set; }
        public decimal Amount { get; set; }
        public long OutletId { get; set; }

        public virtual ICollection<PlaningDetail> PlaningDetails { get; set; }
    }
}
