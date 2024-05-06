using AMNSystemsERP.DL.DB.DBSets.Accounts;

namespace AMNSystemsERP.BL.Repositories.Configuration
{
    public interface IConfigurationSettingService
    {
        Task<List<ConfigurationSetting>> SaveConfigurationSetting(List<ConfigurationSetting> request);
        Task<List<ConfigurationSetting>> GetConfigurationSetting(long outletId);
    }
}