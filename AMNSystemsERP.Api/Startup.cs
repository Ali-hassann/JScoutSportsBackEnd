using AMNSystemsERP.Api.Extensions;
using Newtonsoft.Json.Serialization;

namespace AMNSystemsERP.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDatabase(Configuration)
                .AddIdentity()
                .AddJwtAuthentication(services.GetApplicationSettings(Configuration))
                .AddRdlcReportsConfiguration(Configuration)
                .AddApplicationServices()
                .AddSwagger()
                .AddMapperService()
                .AddApiControllers()
                .AddNewtonsoftJson
                (
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                )
                .AddNewtonsoftJson
                (
                    options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                )
                .AddJsonOptions
                (
                    options => options.JsonSerializerOptions.PropertyNamingPolicy = null
                );
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "AMN Systems ERP");
                options.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseCors
            (
                options => options.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader()
                                  .SetIsOriginAllowed(origin => true)
            );

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
