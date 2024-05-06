using AMNSystemsERP.CL.Models.Commons.Base;

namespace AMNSystemsERP.CL.Models.EmployeePayrollModels
{
    public class PayrollParameterRequest : CommonBaseModel
    {
        public long OutletId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int MonthOf { get; set; }
        public int YearOf { get; set; }
        public string Ids { get; set; }
        public string EmployeeIds { get; set; }
        public string DepartmentIds { get; set; }
        public int DepartmentTypeId { get; set; }
        public int SalaryType { get; set; }
    }
}