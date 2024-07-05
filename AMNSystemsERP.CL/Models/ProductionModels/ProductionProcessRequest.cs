using AMNSystemsERP.CL.Enums;

namespace AMNSystemsERP.CL.Models.ProductionModels
{
    public class ProductionProcessRequest
    {
        public long ProductionProcessId { get; set; }
        public long EmployeeId { get; set; }
        public long OrderMasterId { get; set; }
        public int ProcessId { get; set; }
        public int Status { get; set; }
		public DateTime ProductionDate { get; set; }
        public string EmployeeName { get; set; }
        public string ProcessTypeName { get; set; }
        public string Warehouse { get; set; }
        public string OrderName { get; set; }

        public long ProductId { get; set; }
        public int ProductSizeId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal IssueQuantity { get; set; }
        public decimal ReceiveQuantity { get; set; }
        public decimal BalanceQuantity { get; set; }
        public decimal WarehouseQuantity { get; set; }
        public int IssuanceNo { get; set; }
        public decimal ProcessRate { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public string UnitName { get; set; }
        public string ProductSizeName { get; set; }
        public string AlreadyReceiveQuantity { get; set; }
        public bool IsPosted { get; set; }
        public string DepartmentsName { get; set; }
        public EntityState EntityState { get; set; } = EntityState.Inserted;
    }
}