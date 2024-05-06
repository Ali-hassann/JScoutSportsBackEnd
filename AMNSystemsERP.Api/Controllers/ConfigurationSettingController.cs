using AMNSystemsERP.BL.Repositories.Configuration;
using AMNSystemsERP.DL.DB.DBSets.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.Api.Controllers
{
    [Route("v1/api/ConfigurationSetting")]
    public class ConfigurationSettingController : ApiController
    {
        private readonly IConfigurationSettingService _configurationService;

        public ConfigurationSettingController(IConfigurationSettingService configurationService)
        {
            _configurationService = configurationService;
        }

        [HttpPost]
        [Route("SaveConfigurationSetting")]
        public async Task<List<ConfigurationSetting>> SaveConfigurationSetting([FromBody] List<ConfigurationSetting> request)
        {
            try
            {
                if (request?.Count > 0)
                {
                    return await _configurationService.SaveConfigurationSetting(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("GetConfigurationSetting")]
        public async Task<List<ConfigurationSetting>> GetConfigurationSetting(long outletId)
        {
            try
            {
                if (outletId > 0)
                {
                    return await _configurationService.GetConfigurationSetting(outletId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
    }
}