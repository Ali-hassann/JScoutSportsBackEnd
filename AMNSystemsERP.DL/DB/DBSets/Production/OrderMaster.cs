using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Production
{
    public class OrderMaster : Entity
    {
        public OrderMaster()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        public long OrderMasterId { get; set; }
        public string OrderName { get; set; }
        public string Remarks { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal OtherCost { get; set; }
        public int OrderStatus { get; set; }
        public long ParticularId { get; set; }
        public long OutletId { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}