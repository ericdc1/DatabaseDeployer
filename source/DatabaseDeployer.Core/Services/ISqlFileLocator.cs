namespace DatabaseDeployer.Core.Services
{
    public interface ISqlFileLocator
    {
        string[] GetSqlFilenames(string scriptBaseFolder, string scriptFolder);
    }
}