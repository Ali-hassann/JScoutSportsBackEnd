using AMNSystemsERP.DL.DB.DBSets.Accounts;

namespace AMNSystemsERP.BL.Repositories.Configuration
{
    public class ConfigurationSettingService : IConfigurationSettingService
    {
        private readonly IUnitOfWork _unit;

        public ConfigurationSettingService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<List<ConfigurationSetting>> SaveConfigurationSetting(List<ConfigurationSetting> request)
        {
            try
            {
                _unit.ConfigurationSettingRepository.UpdateList(request);
                await _unit.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return request;
        }

        public async Task<List<ConfigurationSetting>> GetConfigurationSetting(long outletId)
        {
            try
            {
                return (
                        await _unit.ConfigurationSettingRepository.GetAsync(s => s.OutletId == outletId)
                       )?.ToList()
                       ?? new List<ConfigurationSetting>();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}