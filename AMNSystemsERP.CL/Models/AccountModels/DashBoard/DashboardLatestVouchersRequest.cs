namespace AMNSystemsERP.CL.Models.AccountModels.DashBoard
{
    public class DashboardLatestVouchersRequest 
    {
        public long VouchersMasterId { get; set; }
        public string  VouchersTypeName { get; set; }
        public decimal TotalAmount { get; set; }
        public string CreatedDate { get; set; }
    }
}