using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Identity;
using AMNSystemsERP.BL.Repositories;
using AMNSystemsERP.BL.Repositories.Identity;
using AMNSystemsERP.BL.Repositories.ChartOfAccounts;
using AMNSystemsERP.BL.Repositories.Reports;
using AMNSystemsERP.BL.Repositories.Vouchers;
using AMNSystemsERP.BL.Repositories.Configuration;
using AMNSystemsERP.BL.Repositories.Dashboard;
using AMNSystemsERP.DL.DB;
using AMNSystemsERP.BL.Repositories.Inventory;
using AMNSystemsERP.BL.Repositories.StockManagement;
using AMNSystemsERP.DL.DB.DBSets.Identity;
using AMNSystemsERP.CL.Services.CurrentLogin;
using AMNSystemsERP.CL.Services.Documents;
using AMNSystemsERP.CL.Services.Configuration;
using AMNSystemsERP.BL.Repositories.Organization;
using SMSRdlcReports.BL.Repositories.Reports;
using AMNSystemsERP.CL.Models.RDLCModels;
using AMNSystemsERP.BL.Repositories.EmployeePayroll.CommonDataRepo;
using AMNSystemsERP.BL.Repositories.EmployeePayroll.EmployeeRepo;
using AMNSystemsERP.BL.Repositories.EmployeePayroll.AttendanceRepo;
using AMNSystemsERP.BL.Repositories.EmployeePayroll.PayrollRepo;
using AMNSystemsERP.BL.Repositories.EmployeePayroll.PayrollReports;
using AMNSystemsERP.BL.Repositories.Production;
using System.Configuration;

namespace AMNSystemsERP.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static AppSettings GetApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetSection("ApplicationSettings");
            services.Configure<AppSettings>(applicationSettingsConfiguration);
            return applicationSettingsConfiguration.Get<AppSettings>();
        }

        public static IServiceCollection AddRdlcReportsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ReportsConfigs>(configuration.GetSection("ReportsConfigs"));
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            string authenticationScheme = "ERPAuthScheme";

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(authenticationScheme, x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
            => services
                .AddDbContext<ERPContext>(options => options
                    .UseSqlServer(configuration.GetDefaultConnectionString()));

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ERPContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddMapperService(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            => services
                    .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                    .AddScoped<ICurrentLoginService, CurrentLoginService>()
                    .AddTransient<IDocumentHelperService, DocumentHelperService>()
                    .AddTransient<IAppConfiguration, AppConfiguration>()
                    .AddTransient<IUnitOfWork, UnitOfWork>()
                    .AddTransient<IIdentityService, IdentityService>()
                    .AddTransient<IOrganizationService, OrganizationService>()
                    .AddTransient<IChartOfAccountsService, ChartOfAccountsService>()
                    .AddTransient<ICommonRDLCReportsService, CommonRDLCReportsService>()
                    .AddTransient<IReportsService, ReportsService>()
                    .AddTransient<IVoucherService, VoucherService>()
                    .AddTransient<IConfigurationSettingService, ConfigurationSettingService>()
                    .AddTransient<IDashboardService, DashboardService>()
                    .AddTransient<IInventoryService, InventoryService>()
                    .AddTransient<IPurchaseRequisitionService, PurchaseRequisitionService>()
                    .AddTransient<IInvoiceService, InvoiceService>()
                    .AddTransient<IEmployeeService, EmployeeService>()
                    .AddTransient<IAttendanceService, AttendanceService>()
                    .AddTransient<IPayrollService, PayrollService>()
                    .AddTransient<IPayrollReportsService, PayrollReportsService>()
                    .AddTransient<IPurchaseOrderService, PurchaseOrderService>()
                    .AddTransient<IProductionService, ProductionService>()
                    .AddTransient<ICommonDataService, CommonDataService>();

        public static IServiceCollection AddSwagger(this IServiceCollection services)
            => services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AMNSystemERP", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

        public static IMvcBuilder AddApiControllers(this IServiceCollection services)
        {
            return services.AddControllers();
        }
    }
}
