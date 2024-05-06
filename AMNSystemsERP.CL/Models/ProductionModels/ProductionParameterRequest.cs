namespace AMNSystemsERP.CL.Models.ProductionModels
{
    public class ProductionParameterRequest
    {
        public long EmployeeId { get; set; }
        public long OrderMasterId { get; set; }
        public int ProcessId { get; set; }
        public int Status { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long OutletId { get; set; }
        public long ProductId { get; set; }
        public int ProductSizeId { get; set; }
        public int IssuanceNo { get; set; }
        public int MainProcessTypeId { get; set; }
        public int ProcessTypeId { get; set; }
        public bool IsIncludeZeroValue { get; set; }
    }
}