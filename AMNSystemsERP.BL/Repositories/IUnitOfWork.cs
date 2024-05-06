using AMNSystemsERP.BL.Repositories.CommonRepositories;
using AMNSystemsERP.DL.DB.DBSets.Accounts;
using AMNSystemsERP.DL.DB.DBSets.Commons;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;
using AMNSystemsERP.DL.DB.DBSets.Identity;
using AMNSystemsERP.DL.DB.DBSets.Inventory;
using AMNSystemsERP.DL.DB.DBSets.Organization;
using AMNSystemsERP.DL.DB.DBSets.Production;
using AMNSystemsERP.DL.DB.DBSets.StockManagement;
using Inventory.DL.DB.DBSets.StockManagement;
using DbOrganization = AMNSystemsERP.DL.DB.DBSets.Organization.Organization;

namespace AMNSystemsERP.BL.Repositories
{
    public interface IUnitOfWork
    {
        #region Identity
        ERPRepository<ApplicationUser> UserRepository { get; }
        ERPRepository<Rights> RightsRepository { get; }
        ERPRepository<OrganizationRole> OrganizationRoleRepository { get; }
        ERPRepository<OrgRoleRights> OrgRoleRightsRepository { get; }
        ERPRepository<UserRights> UserRightsRepository { get; }
        #endregion

        #region Accounts
        ERPRepository<MainHeads> MainHeadsRepository { get; }
        ERPRepository<HeadCategories> HeadCategoriesRepository { get; }
        ERPRepository<SubCategories> SubCategoriesRepository { get; }
        ERPRepository<PostingAccounts> PostingAccountsRepository { get; }
        ERPRepository<VouchersMaster> VouchersMasterRepository { get; }
        ERPRepository<VouchersDetail> VouchersDetailRepository { get; }
        ERPRepository<ConfigurationSetting> ConfigurationSettingRepository { get; }
        ERPRepository<VoucherImages> VoucherImagesRepository { get; }
        //
        #endregion

        #region Inventory
        ERPRepository<Item> ItemRepository { get; }
        ERPRepository<ItemType> ItemTypeRepository { get; }
        ERPRepository<ItemCategory> ItemCategoryRepository { get; }
        ERPRepository<Unit> UnitRepository { get; }
        ERPRepository<Brand> BrandRepository { get; }
        ERPRepository<Particular> ParticularRepository { get; }
        ERPRepository<Bundle> BundleRepository { get; }
        ERPRepository<BundleDetail> BundleDetailRepository { get; }
        ERPRepository<ItemParticular> ItemParticularRepository { get; }
        ERPRepository<ItemOpening> ItemOpeningRepository { get; }
        #endregion

        #region Stock management
        ERPRepository<InvoiceMaster> InvoiceMasterRepository { get; }
        ERPRepository<InvoiceDetail> InvoiceDetailRepository { get; }
        ERPRepository<PurchaseRequisitionMaster> PurchaseRequisitionMasterRepository { get; }
        ERPRepository<PurchaseRequisitionDetail> PurchaseRequisitionDetailRepository { get; }
        ERPRepository<PurchaseOrderMaster> PurchaseOrderMasterRepository { get; }
        ERPRepository<PurchaseOrderDetail> PurchaseOrderDetailRepository { get; }
        #endregion

        #region Production
        ERPRepository<OrderMaster> OrderMasterRepository { get; }
        ERPRepository<OrderDetail> OrderDetailRepository { get; }
        ERPRepository<ProductionProcess> ProductionProcessRepository { get; }
        ERPRepository<Product> ProductRepository { get; }
        ERPRepository<ProcessType> ProcessTypeRepository { get; }
        ERPRepository<ProductSize> ProductSizeRepository { get; }
        ERPRepository<Process> ProcessRepository { get; }
        ERPRepository<PlaningDetail> PlaningDetailRepository { get; }
        ERPRepository<PlaningMaster> PlaningMasterRepository { get; }
        ERPRepository<ProductCategory> ProductCategoryRepository { get; }
        ERPRepository<ProductOpening> ProductOpeningRepository { get; }
        #endregion

        #region Organization
        ERPRepository<DbOrganization> OrganizationRepository { get; }
        ERPRepository<Outlet> OutletRepository { get; }
        #endregion

        #region Employee Payroll

        ERPRepository<Employee> EmployeeRepository { get; }
        ERPRepository<EmployeeQualifications> EmployeeQualificationsRepository { get; }
        ERPRepository<EmployeeLoan> EmployeeLoanRepository { get; }
        ERPRepository<AllowanceType> AllowanceTypeRepository { get; }
        ERPRepository<EmployeeAllowances> EmployeeAllowancesRepository { get; }
        ERPRepository<EmployeeDetail> EmployeeDetailRepository { get; }
        ERPRepository<Attendance> AttendanceRepository { get; }
        ERPRepository<AttendanceDetail> AttendanceDetailRepository { get; }
        ERPRepository<Overtime> OvertimeRepository { get; }
        ERPRepository<OvertimeDetail> OvertimeDetailRepository { get; }
        ERPRepository<EmployeeDocuments> EmployeeDocumentsRepository { get; }
        ERPRepository<QualificationSubject> QualificationSubjectRepository { get; }
        ERPRepository<Departments> DepartmentsRepository { get; }
        ERPRepository<Designation> DesignationRepository { get; }
        ERPRepository<SalarySheet> SalarySheetRepository { get; }
        ERPRepository<Wages> WagesRepository { get; }

        #endregion

        #region Common
        ERPRepository<SerialConfiguration> SerialConfigurationRepository { get; }

        #endregion
        DapperRepository DapperRepository { get; }

        Task<bool> SaveAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = new CancellationToken());
    }
}
