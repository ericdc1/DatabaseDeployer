namespace DatabaseDeployer.Core.Services
{
    public interface IFileFilterService
    {
        string[] GetFilteredFilenames(string[] allFiles, string excludeFilenameContaining);
    }
}