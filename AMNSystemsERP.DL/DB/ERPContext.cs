using AMNSystemsERP.DL.DB.DBSets.Accounts;
using AMNSystemsERP.DL.DB.DBSets.Commons;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;
using AMNSystemsERP.DL.DB.DBSets.Identity;
using AMNSystemsERP.DL.DB.DBSets.Inventory;
using AMNSystemsERP.DL.DB.DBSets.Organization;
using AMNSystemsERP.DL.DB.DBSets.Production;
using AMNSystemsERP.DL.DB.DBSets.StockManagement;
using Inventory.DL.DB.DBSets.StockManagement;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AMNSystemsERP.DL.DB
{
    public partial class ERPContext : IdentityDbContext<ApplicationUser>
    {
        public ERPContext(DbContextOptions<ERPContext> options)
            : base(options)
        {
        }

        #region Identity
        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
        public virtual DbSet<Rights> Rights { get; set; }
        public virtual DbSet<OrganizationRole> OrganizationRole { get; set; }
        public virtual DbSet<OrgRoleRights> OrgRoleRights { get; set; }
        public virtual DbSet<UserRights> UserRights { get; set; }
        #endregion

        #region Accounts
        public virtual DbSet<MainHeads> MainHeads { get; set; }
        public virtual DbSet<HeadCategories> HeadCategories { get; set; }
        public virtual DbSet<SubCategories> SubCategories { get; set; }
        public virtual DbSet<PostingAccounts> PostingAccounts { get; set; }
        public virtual DbSet<VouchersMaster> VouchersMaster { get; set; }
        public virtual DbSet<VouchersDetail> VouchersDetail { get; set; }
        public virtual DbSet<VoucherImages> VoucherImages { get; set; }

        public virtual DbSet<ConfigurationSetting> ConfigurationSetting { get; set; }

        #endregion

        #region Inventory
        public virtual DbSet<ItemType> ItemType { get; set; }
        public virtual DbSet<ItemCategory> ItemCategory { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemOpening> ItemOpening { get; set; }
        #endregion

        #region Production
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProcessType> ProcessType { get; set; }
        public virtual DbSet<ProductSize> ProductSize { get; set; }
        public virtual DbSet<Process> Process { get; set; }
        public virtual DbSet<PlaningMaster> PlaningMaster { get; set; }
        public virtual DbSet<PlaningDetail> PlaningDetail { get; set; }
        public virtual DbSet<OrderMaster> OrderMaster { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<ProductionProcess> ProductionProcess { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductOpening> ProductOpening { get; set; }
        #endregion

        #region StockManagement
        public virtual DbSet<Particular> Customer { get; set; }
        public virtual DbSet<Particular> Particular { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetail { get; set; }
        public virtual DbSet<InvoiceMaster> InvoiceMaster { get; set; }
        public virtual DbSet<Bundle> Bundle { get; set; }
        public virtual DbSet<BundleDetail> BundleDetail { get; set; }
        public virtual DbSet<ItemParticular> ItemParticular { get; set; }
        public virtual DbSet<PurchaseRequisitionMaster> PurchaseRequisitionMaster { get; set; }
        public virtual DbSet<PurchaseRequisitionDetail> PurchaseRequisitionDetail { get; set; }
        public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }
        public virtual DbSet<PurchaseOrderMaster> PurchaseOrderMaster { get; set; }

        #endregion

        #region Organization
        //
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<Outlet> Outlet { get; set; }
        //
        #endregion

        #region Employee Payroll
        public virtual DbSet<AllowanceType> BenefitType { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeAllowances> EmployeeAllowances { get; set; }
        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<AttendanceDetail> AttendanceDetail { get; set; }
        public virtual DbSet<Overtime> Overtime { get; set; }
        public virtual DbSet<OvertimeDetail> OvertimeDetail { get; set; }
        public virtual DbSet<EmployeeDetail> EmployeeDetail { get; set; }
        public virtual DbSet<EmployeeLoan> EmployeeLoan { get; set; }
        public virtual DbSet<EmployeeQualifications> EmployeeQualifications { get; set; }
        public virtual DbSet<EmployeeDocuments> EmployeeDocuments { get; set; }
        public virtual DbSet<QualificationSubject> QualificationSubject { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Designation> Designation { get; set; }
        public virtual DbSet<AllowanceType> AllowanceType { get; set; }
        public virtual DbSet<SalarySheet> SalarySheet { get; set; }
        public virtual DbSet<Wages> Wages { get; set; }
        #endregion

        #region Common
        public virtual DbSet<SerialConfiguration> SerialConfiguration { get; set; }

        #endregion
    }
}