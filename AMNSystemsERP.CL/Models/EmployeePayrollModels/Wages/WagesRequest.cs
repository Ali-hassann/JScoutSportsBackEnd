namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Wages
{
    public class WagesRequest
    {
        public long WagesId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime WagesDate { get; set; }
        public decimal WagesAmount { get; set; }
        public decimal Installment { get; set; }
        public decimal Advance { get; set; }
        public decimal NetPay => WagesAmount - (Installment + Advance);
        public long OutletId { get; set; }
        public bool IsPosted { get; set; }
        public string? Remarks { get; set; }

        /*****  Not Mapped Properties  *******/
        public string EmployeeName { get; set; }
        public long DepartmentsId { get; set; }
        public string DepartmentsName { get; set; }
        public string DesignationName { get; set; }
        public string JoiningDate { get; set; }
        public string ContactNumber { get; set; }
        public bool Selected { get; set; }
    }
}
