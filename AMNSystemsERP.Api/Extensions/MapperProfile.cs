using AMNSystemsERP.CL.Models.AccountModels.ChartOfAccounts;
using AMNSystemsERP.CL.Models.AccountModels.Vouchers;
using AMNSystemsERP.CL.Models.IdentityModels;
using AMNSystemsERP.CL.Models.InventoryModels;
using AMNSystemsERP.CL.Models.OrganizationModels;
using AMNSystemsERP.CL.Models.StockManagementModels;
using AMNSystemsERP.DL.DB.DBSets.Accounts;
using AMNSystemsERP.DL.DB.DBSets.Identity;
using AMNSystemsERP.DL.DB.DBSets.Inventory;
using DbOrganization = AMNSystemsERP.DL.DB.DBSets.Organization.Organization;
using AMNSystemsERP.DL.DB.DBSets.StockManagement;
using AutoMapper;
using AMNSystemsERP.CL.Models.OrganizationModelsS;
using AMNSystemsERP.DL.DB.DBSets.Organization;
using AMNSystemsERP.DL.DB.DBSets.EmployeePayroll;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Attendance;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Employee;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Loans;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Base;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Payroll.Allowances;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Overtime;
using Inventory.DL.DB.DBSets.StockManagement;
using AMNSystemsERP.CL.Models.ProductionModels;
using AMNSystemsERP.DL.DB.DBSets.Production;
using AMNSystemsERP.CL.Models.EmployeePayrollModels.Wages;

namespace AMNSystemsERP.Api.Extensions
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Mapper is used to Copy from Source object to Destination object
            CreateMap<ApplicationUser, ApplicationUserRequest>()
              .ReverseMap();

            CreateMap<ApplicationUser, UserRegisterResponse>()
              .ReverseMap();

            CreateMap<ApplicationUser, UserProfile>()
              .ReverseMap();

            CreateMap<Rights, RightsRequest>()
              .ReverseMap();

            CreateMap<OrgRoleRights, OrgRoleRightsRequest>()
              .ReverseMap();

            CreateMap<OrganizationRole, OrganizationRoleRequest>()
              .ReverseMap();

            CreateMap<UserRights, UserRightsRequest>()
              .ReverseMap();

            #region Accounts
            CreateMap<MainHeadsResponse, MainHeads>()
                .ReverseMap();
            CreateMap<HeadCategoriesRequest, HeadCategories>()
                .ReverseMap();
            CreateMap<SubCategoriesRequest, SubCategories>()
                .ReverseMap();
            CreateMap<PostingAccountsRequest, PostingAccounts>()
                .ReverseMap();
            CreateMap<VouchersMasterRequest, VouchersMaster>()
                .ReverseMap();
            CreateMap<VouchersDetailRequest, VouchersDetail>()
                .ReverseMap();
            CreateMap<VoucherImagesRequest, VoucherImages>()
                .ReverseMap();
            #endregion

            CreateMap<ItemTypeRequest, ItemType>()
                 .ReverseMap();
            CreateMap<ProductSizeRequest, ProductSize>()
                 .ReverseMap();
            CreateMap<ItemCategoryRequest, ItemCategory>()
                 .ReverseMap();
            CreateMap<BrandRequest, Brand>()
                 .ReverseMap();
            CreateMap<ItemOpeningRequest, ItemOpening>()
                 .ReverseMap();
            CreateMap<PurchaseRequisitionMasterRequest, PurchaseRequisitionMaster>()
                 .ReverseMap();
            CreateMap<PurchaseRequisitionDetailRequest, PurchaseRequisitionDetail >()
                 .ReverseMap();
            CreateMap<UnitRequest, Unit>()
                 .ReverseMap();
            CreateMap<ItemRequest, Item>()
                 .ReverseMap();
            CreateMap<ParticularRequest, Particular>()
                 .ReverseMap();
            CreateMap<ItemParticularRequest, ItemParticular>()
                 .ReverseMap();
            CreateMap<ProductRequest, Product>()
                 .ReverseMap();
            CreateMap<ProcessType, ProcessTypeRequest>()
                .ForMember(dest => dest.MainProcessTypeId, opt => opt.MapFrom(src => (int)src.MainProcessTypeId)) // map enum values
                 .ReverseMap();
            CreateMap<ProcessRequest, Process>()
                 .ReverseMap();
            CreateMap<PlaningMasterRequest, PlaningMaster>()
                 .ReverseMap();
            CreateMap<PlaningDetailRequest, PlaningDetail>()
                 .ReverseMap();
            CreateMap<ProductCategoryRequest, ProductCategory>()
                 .ReverseMap();
            CreateMap<OrderMasterRequest, OrderMaster>()
                 .ReverseMap();
            CreateMap<OrderDetailRequest, OrderDetail>()
                 .ReverseMap();
            CreateMap<ProductionProcessRequest, ProductionProcess>()
                 .ReverseMap();
            CreateMap<BundleRequest, Bundle>()
                 .ReverseMap();
            CreateMap<InvoiceMasterRequest, InvoiceMaster>()
                 .ReverseMap();
            CreateMap<InvoiceDetailRequest, InvoiceDetail>()
                 .ReverseMap();
            CreateMap<PurchaseOrderMasterRequest, PurchaseOrderMaster>()
                 .ReverseMap();
            CreateMap<PurchaseOrderDetailRequest, PurchaseOrderDetail>()
                 .ReverseMap();
            CreateMap<OrganizationRegRequests, DbOrganization>()
                 .ReverseMap();
            CreateMap<OutletProfileRequest, Outlet>()
                 .ReverseMap();
            CreateMap<DepartmentsRequest, Departments>()
                    .ReverseMap();
            CreateMap<DesignationRequest, Designation>()
                    .ReverseMap();
            CreateMap<EmployeeBasicRequest, Employee>()
                    .ReverseMap();
            CreateMap<EmployeeQualificationsRequest, EmployeeQualifications>()
                    .ReverseMap();
            CreateMap<AllowanceType, AllowanceTypeRequest>()
                    .ReverseMap();
            CreateMap<EmployeeLoan, EmployeeLoanRequest>()
                .ForMember(dest => dest.LoanTypeId, opt => opt.MapFrom(src => (int)src.LoanTypeId)) // map enum values
                    .ReverseMap();
            CreateMap<EmployeeDetail, EmployeeDetailRequest>()
                    .ReverseMap();
            CreateMap<EmployeeAllowances, EmployeeAllowancesRequest>()
                    .ReverseMap();
            CreateMap<AttendanceRequest, Attendance>()
                    .ReverseMap();
            CreateMap<AttendanceDetail, AttendanceDetailRequest>()
                    .ReverseMap();
            CreateMap<OvertimeRequest, Overtime>()
                    .ReverseMap();
            CreateMap<OvertimeDetail, OvertimeDetailRequest>()
                    .ReverseMap();
            CreateMap<EmployeeDocumentsRequest, EmployeeDocuments>()
                    .ReverseMap();
            CreateMap<QualificationSubjectRequest, QualificationSubject>()
                    .ReverseMap();
            CreateMap<AllowanceType, AllowanceTypeRequest>()
                    .ReverseMap();
            CreateMap<Wages, WagesRequest>()
                    .ReverseMap();
        }
    }
}
