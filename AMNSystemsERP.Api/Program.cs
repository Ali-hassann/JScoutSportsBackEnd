using AMNSystemsERP.CL.Helper;

namespace AMNSystemsERP.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Setting/Getting EnvironmentName
                var environmentName = EnvironmentHelper.ConfigureEnvironment();
                //

                if (string.IsNullOrEmpty(environmentName))
                {
                    throw new Exception("Environment Name is empty.");
                }                

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
           => Host
               .CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config
                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: true)
                       .AddJsonFile($"ReportConfig.json", optional: false, reloadOnChange: true);
               })
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
    }
}
