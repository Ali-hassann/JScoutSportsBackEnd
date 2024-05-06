using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Allowances
{
    public class AllowanceTypeRequest : CommonBaseModel
    {
        public long AllowanceTypeId { get; set; }
        public string Name { get; set; }
    }
}
