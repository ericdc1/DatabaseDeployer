using System;
using DatabaseDeployer.Core.Model;
using DatabaseDeployer.Infrastructure.DatabaseManager.DataAccess;

namespace DatabaseDeployer.Core.Services.Impl
{
	public class DatabaseUpdater : IDatabaseActionExecutor
	{
		private readonly IScriptFolderExecutor _folderExecutor;
        private readonly IQueryExecutor _queryExecutor;

		public DatabaseUpdater(IScriptFolderExecutor folderExecutor, IQueryExecutor queryExecutor)
		{
			_folderExecutor = folderExecutor;
            _queryExecutor = queryExecutor;
		}

	    public DatabaseUpdater():this(new ScriptFolderExecutor(), new QueryExecutor())
	    {
	        
	    }

	    public void Execute(TaskAttributes taskAttributes, ITaskObserver taskObserver)
		{
	        if (!_queryExecutor.CheckDatabaseExists(taskAttributes.ConnectionSettings))
	        {
                taskObserver.Log(string.Format("Database does not exist. Attempting to create database before updating."));
                string sql = string.Format("create database [{0}]", taskAttributes.ConnectionSettings.Database);
                _queryExecutor.ExecuteNonQuery(taskAttributes.ConnectionSettings, sql, false);
                _folderExecutor.ExecuteScriptsInFolder(taskAttributes, "Create", taskObserver);
	        }

            _folderExecutor.ExecuteScriptsInFolder(taskAttributes, "Update", taskObserver);
		}

	}
}