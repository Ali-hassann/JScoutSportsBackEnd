namespace AMNSystemsERP.DL.DB.DBSets.EmployeePayroll
{
    public class Wages
    {
        public long WagesId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime WagesDate { get; set; }
        public decimal WagesAmount { get; set; }
        public decimal Installment { get; set; }
        public decimal Advance { get; set; }
        public decimal NetPay { get; set; }
        public long OutletId { get; set; }
        public bool IsPosted { get; set; }
        public string? Remarks { get; set; }
    }
}
