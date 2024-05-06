using Microsoft.Extensions.Configuration;

namespace AMNSystemsERP.CL.Helper
{
    public static class EnvironmentHelper
    {
        public static string EnvironmentName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");        

        public static string ConfigureEnvironment()
        {
            try
            {
                // Environment Configuration
                var environmentConfig = new ConfigurationBuilder()
                                                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                                    .AddJsonFile("Environments.json", optional: false, reloadOnChange: true)
                                                    .Build();

                var environmentName = environmentConfig
                                                .GetSection("Environments")
                                                ?.GetChildren()
                                                ?.FirstOrDefault()
                                                ?.Value ?? "";

                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environmentName);
                return environmentName;
                //
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
