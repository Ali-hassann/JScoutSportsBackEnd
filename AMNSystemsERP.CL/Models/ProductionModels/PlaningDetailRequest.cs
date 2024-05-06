namespace AMNSystemsERP.CL.Models.ProductionModels
{
    public class PlaningDetailRequest
    {
        public long PlaningDetailId { get; set; }
        public long ItemId { get; set; }
        public int UnitId { get; set; }
        public long PlaningMasterId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsManualPrice { get; set; }
        public string ItemName { get; set; }
    }
}