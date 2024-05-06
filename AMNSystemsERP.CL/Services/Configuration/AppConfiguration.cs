using AMNSystemsERP.CL.Models.Commons;
using Microsoft.Extensions.Configuration;

namespace AMNSystemsERP.CL.Services.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        private readonly IConfiguration _configuration;

        public AppConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            
            var appSettings = configuration.GetSection("ApplicationSettings").Get<AppSettings>();
            
            _secret = appSettings.Secret;
            _supportEmail = appSettings.SupportEmail;
        }

        private string _secret { get; set; }
        public string Secret
        {
            get => _secret;
            set => _secret = value;
        }

        private string _supportEmail { get; set; }
        public string SupportEmail
        {
            get => _supportEmail;
            set => _supportEmail = value;
        }        
    }
}