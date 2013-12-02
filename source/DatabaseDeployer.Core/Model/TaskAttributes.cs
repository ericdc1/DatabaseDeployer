using DatabaseDeployer.Core.Services.Impl;

namespace DatabaseDeployer.Core.Model
{
    public class TaskAttributes
    {
        public TaskAttributes(ConnectionSettings connectionSettings, string scriptDirectory)
        {
            ConnectionSettings = connectionSettings;
            ScriptDirectory = scriptDirectory;
        }

        public ConnectionSettings ConnectionSettings { get; set; }
        public string SkipFileNameContaining { get; set; }
        public string ScriptDirectory { get; set; }
        public RequestedDatabaseAction RequestedDatabaseAction { get; set; }

        public bool LogOnly = false;
    }
}