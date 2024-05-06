using AMNSystemsERP.CL.Enums;

namespace AMNSystemsERP.CL.Models.ProductionModels
{
    public class ProcessRequest
    {
        public int ProcessId { get; set; }
        public int ProcessTypeId { get; set; }
        public long ProductId { get; set; }
        public int ProductSizeId { get; set; }
        public long OrderMasterId { get; set; }
        public decimal ProcessRate { get; set; }
        public decimal OtherRate { get; set; }
        public string Description { get; set; }

        // not map feilds
        public int MainProcessTypeId { get; set; }
        public string ProductName { get; set; }
        public string ProcessTypeName { get; set; }
        public string ProductCategoryName { get; set; }
        public string UnitName { get; set; }
        public string OrderName { get; set; }
        public bool Selected { get; set; }
        public EntityState EntityState { get; set; } = EntityState.Inserted;
    }
}