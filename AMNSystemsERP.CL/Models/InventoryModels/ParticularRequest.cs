using AMNSystemsERP.CL.Enums.InventoryEnums;

namespace AMNSystemsERP.CL.Models.InventoryModels
{
    public class ParticularRequest
    {
        public long ParticularId { get; set; }
        public string ParticularName { get; set; }
        public string RepresentativeName { get; set; }
        public string MainProductName { get; set; }
        public ParticularType ParticularType { get; set; }
        public string ParticularTypeName { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public long OutletId { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
    }
}