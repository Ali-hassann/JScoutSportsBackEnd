namespace AMNSystemsERP.CL.Models.ProductionModels
{
    public class OrderMasterRequest
    {
        public long OrderMasterId { get; set; }
        public string OrderName { get; set; }
        public string Remarks { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal OtherCost { get; set; }
        public int OrderStatus { get; set; }
        public long ParticularId { get; set; }
        public string ParticularName { get; set; }
        public long OutletId { get; set; }
        public List<OrderDetailRequest> OrderDetailsRequest { get; set; }
    }
}