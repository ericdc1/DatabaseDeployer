namespace DatabaseDeployer.Core
{
    public interface IApplicationConfiguration
    {
        string GetSetting(string settingName);
        object GetSection(string sectionName);
    }
}