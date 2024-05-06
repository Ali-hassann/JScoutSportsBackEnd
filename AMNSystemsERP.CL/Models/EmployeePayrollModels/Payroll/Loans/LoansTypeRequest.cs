using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Loans
{
    public class LoansTypeRequest : CommonBaseModel
    {        
        public long LoanTypeId { get; set; }       
        public string TypeName { get; set; }
        public string Description { get; set; }
        public string OutletName { get; set; }
    }
}

