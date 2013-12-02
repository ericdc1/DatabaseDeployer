using System.IO;


namespace DatabaseDeployer.Core
{
    public interface IFileStreamFactory
    {
        Stream ConstructReadFileStream(string path);
        Stream ConstructWriteFileStream(string path);
    }
}