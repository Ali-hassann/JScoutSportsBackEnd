using AMNSystemsERP.BL.Repositories.CommonRepositories;
using AMNSystemsERP.CL.Helper;
using AMNSystemsERP.CL.Services.CurrentLogin;
using AMNSystemsERP.DL.DB;
using AMNSystemsERP.DL.DB.Base;
using AMNSystemsERP.DL.DB.DBSets.Accounts;
using AMNSystemsERP.DL.DB.DBSets.Commons;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;
using AMNSystemsERP.DL.DB.DBSets.Identity;
using AMNSystemsERP.DL.DB.DBSets.Inventory;
using AMNSystemsERP.DL.DB.DBSets.Organization;
using AMNSystemsERP.DL.DB.DBSets.Production;
using AMNSystemsERP.DL.DB.DBSets.StockManagement;
using Inventory.DL.DB.DBSets.StockManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DbOrganization = AMNSystemsERP.DL.DB.DBSets.Organization.Organization;

namespace AMNSystemsERP.BL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ICurrentLoginService _currentLoginService;
        private readonly ERPContext _context;
        private bool disposed = false;
        private readonly IConfiguration _configuration;

        public UnitOfWork(ERPContext context, ICurrentLoginService currentLoginService, IConfiguration configuration)
        {
            _context = context;
            _currentLoginService = currentLoginService;
            _configuration = configuration;
        }

        //  Dapper repository
        public DapperRepository DapperRepository => new DapperRepository(_configuration.GetConnectionString("DefaultConnection"));
        //

        #region Identity
        public ERPRepository<ApplicationUser> UserRepository => new ERPRepository<ApplicationUser>(_context);
        public ERPRepository<Rights> RightsRepository => new ERPRepository<Rights>(_context);
        public ERPRepository<OrganizationRole> OrganizationRoleRepository => new ERPRepository<OrganizationRole>(_context);
        public ERPRepository<OrgRoleRights> OrgRoleRightsRepository => new ERPRepository<OrgRoleRights>(_context);
        public ERPRepository<UserRights> UserRightsRepository => new ERPRepository<UserRights>(_context);
        #endregion

        #region Accounts
        public ERPRepository<MainHeads> MainHeadsRepository => new ERPRepository<MainHeads>(_context);
        public ERPRepository<HeadCategories> HeadCategoriesRepository => new ERPRepository<HeadCategories>(_context);
        public ERPRepository<SubCategories> SubCategoriesRepository => new ERPRepository<SubCategories>(_context);
        public ERPRepository<PostingAccounts> PostingAccountsRepository => new ERPRepository<PostingAccounts>(_context);
        public ERPRepository<VouchersMaster> VouchersMasterRepository => new ERPRepository<VouchersMaster>(_context);
        public ERPRepository<VouchersDetail> VouchersDetailRepository => new ERPRepository<VouchersDetail>(_context);
        public ERPRepository<ConfigurationSetting> ConfigurationSettingRepository => new ERPRepository<ConfigurationSetting>(_context);
        public ERPRepository<VoucherImages> VoucherImagesRepository => new ERPRepository<VoucherImages>(_context);

        #endregion

        #region  Inventory
        public ERPRepository<Item> ItemRepository => new ERPRepository<Item>(_context);
        public ERPRepository<ItemType> ItemTypeRepository => new ERPRepository<ItemType>(_context);
        public ERPRepository<Unit> UnitRepository => new ERPRepository<Unit>(_context);
        public ERPRepository<ItemCategory> ItemCategoryRepository => new ERPRepository<ItemCategory>(_context);
        public ERPRepository<ItemOpening> ItemOpeningRepository => new ERPRepository<ItemOpening>(_context);
        public ERPRepository<Brand> BrandRepository => new ERPRepository<Brand>(_context);
        public ERPRepository<Particular> ParticularRepository => new ERPRepository<Particular>(_context);
        public ERPRepository<ItemParticular> ItemParticularRepository => new ERPRepository<ItemParticular>(_context);
        public ERPRepository<Bundle> BundleRepository => new ERPRepository<Bundle>(_context);
        public ERPRepository<BundleDetail> BundleDetailRepository => new ERPRepository<BundleDetail>(_context);
        #endregion

        #region Production
        public ERPRepository<OrderMaster> OrderMasterRepository => new ERPRepository<OrderMaster>(_context);
        public ERPRepository<OrderDetail> OrderDetailRepository => new ERPRepository<OrderDetail>(_context);
        public ERPRepository<ProductionProcess> ProductionProcessRepository => new ERPRepository<ProductionProcess>(_context);
        public ERPRepository<Product> ProductRepository => new ERPRepository<Product>(_context);
        public ERPRepository<ProcessType> ProcessTypeRepository => new ERPRepository<ProcessType>(_context);
        public ERPRepository<ProductSize> ProductSizeRepository => new ERPRepository<ProductSize>(_context);
        public ERPRepository<Process> ProcessRepository => new ERPRepository<Process>(_context);
        public ERPRepository<PlaningMaster> PlaningMasterRepository => new ERPRepository<PlaningMaster>(_context);
        public ERPRepository<PlaningDetail> PlaningDetailRepository => new ERPRepository<PlaningDetail>(_context);
        public ERPRepository<ProductCategory> ProductCategoryRepository => new ERPRepository<ProductCategory>(_context);
        public ERPRepository<ProductOpening> ProductOpeningRepository => new ERPRepository<ProductOpening>(_context);
        #endregion

        #region Stock Management
        public ERPRepository<InvoiceMaster> InvoiceMasterRepository => new ERPRepository<InvoiceMaster>(_context);
        public ERPRepository<InvoiceDetail> InvoiceDetailRepository => new ERPRepository<InvoiceDetail>(_context);
        public ERPRepository<PurchaseRequisitionMaster> PurchaseRequisitionMasterRepository => new ERPRepository<PurchaseRequisitionMaster>(_context);
        public ERPRepository<PurchaseRequisitionDetail> PurchaseRequisitionDetailRepository => new ERPRepository<PurchaseRequisitionDetail>(_context);
        public ERPRepository<PurchaseOrderMaster> PurchaseOrderMasterRepository => new ERPRepository<PurchaseOrderMaster>(_context);
        public ERPRepository<PurchaseOrderDetail> PurchaseOrderDetailRepository => new ERPRepository<PurchaseOrderDetail>(_context);
        //public ERPRepository<GeneralCustomerInvoice> GeneralCustomerInvoiceRepository => new ERPRepository<GeneralCustomerInvoice>(_context);
        #endregion

        #region Organization
        public ERPRepository<DbOrganization> OrganizationRepository => new ERPRepository<DbOrganization>(_context);
        public ERPRepository<Outlet> OutletRepository => new ERPRepository<Outlet>(_context);
        #endregion

        #region Employee Payroll

        public ERPRepository<Employee> EmployeeRepository => new ERPRepository<Employee>(_context);
        public ERPRepository<EmployeeQualifications> EmployeeQualificationsRepository => new ERPRepository<EmployeeQualifications>(_context);
        public ERPRepository<EmployeeLoan> EmployeeLoanRepository => new ERPRepository<EmployeeLoan>(_context);
        public ERPRepository<AllowanceType> AllowanceTypeRepository => new ERPRepository<AllowanceType>(_context);
        public ERPRepository<EmployeeAllowances> EmployeeAllowancesRepository => new ERPRepository<EmployeeAllowances>(_context);
        public ERPRepository<EmployeeDetail> EmployeeDetailRepository => new ERPRepository<EmployeeDetail>(_context);
        public ERPRepository<EmployeeAllowances> EmployeeBenefitsRepository => new ERPRepository<EmployeeAllowances>(_context);
        public ERPRepository<Attendance> AttendanceRepository => new ERPRepository<Attendance>(_context);
        public ERPRepository<AttendanceDetail> AttendanceDetailRepository => new ERPRepository<AttendanceDetail>(_context);
        public ERPRepository<Overtime> OvertimeRepository => new ERPRepository<Overtime>(_context);
        public ERPRepository<OvertimeDetail> OvertimeDetailRepository => new ERPRepository<OvertimeDetail>(_context);
        public ERPRepository<EmployeeDocuments> EmployeeDocumentsRepository => new ERPRepository<EmployeeDocuments>(_context);
        public ERPRepository<QualificationSubject> QualificationSubjectRepository => new ERPRepository<QualificationSubject>(_context);
        public ERPRepository<Departments> DepartmentsRepository => new ERPRepository<Departments>(_context);
        public ERPRepository<Designation> DesignationRepository => new ERPRepository<Designation>(_context);
        public ERPRepository<SalarySheet> SalarySheetRepository => new ERPRepository<SalarySheet>(_context);
        public ERPRepository<Wages> WagesRepository => new ERPRepository<Wages>(_context);
        #endregion

        #region Common
        public ERPRepository<SerialConfiguration> SerialConfigurationRepository => new ERPRepository<SerialConfiguration>(_context);

        #endregion

        public async Task<bool> SaveAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = new CancellationToken())
        {
            this.ApplyAuditInformation();

            return await this._context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken) > 0;
        }

        private void ApplyAuditInformation()
        {
            var userName = this._currentLoginService.UserName;

            this._context
                 .ChangeTracker
                 .Entries()
                 .ToList()
                 .ForEach(entry =>
                 {
                     if (entry.Entity is IDeletableEntity deletableEntity)
                     {
                         if (entry.State == EntityState.Deleted)
                         {
                             deletableEntity.IsDeleted = true;
                             entry.State = EntityState.Modified;
                         }
                     }

                     if (entry.Entity is IEntity entity)
                     {
                         if (entry.State == EntityState.Added)
                         {
                             entity.CreatedDate = DateHelper.GetCurrentDate();
                             entity.CreatedBy = userName ?? entity.CreatedBy;
                             entity.ModifiedDate = DateHelper.GetCurrentDate();
                             entity.ModifiedBy = userName ?? entity.ModifiedBy;
                         }
                         else if (entry.State == EntityState.Modified)
                         {
                             entity.ModifiedDate = DateHelper.GetCurrentDate();
                             entity.ModifiedBy = userName ?? entity.ModifiedBy;
                         }
                     }
                 });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}