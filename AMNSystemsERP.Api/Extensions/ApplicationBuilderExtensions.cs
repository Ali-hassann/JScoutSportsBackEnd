using AMNSystemsERP.DL.DB;

namespace AMNSystemsERP.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseDBContext(this IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();

            var dbContext = services.ServiceProvider.GetService<ERPContext>();
        }
    }
}
