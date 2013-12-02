using System;
using DatabaseDeployer.Core.Model;
using DatabaseDeployer.Infrastructure.DatabaseManager.DataAccess;

namespace DatabaseDeployer.Core.Services.Impl
{
	public class DatabaseCreator : IDatabaseActionExecutor
	{
		private readonly IQueryExecutor _queryExecutor;
		private readonly IScriptFolderExecutor _folderExecutor;

		public DatabaseCreator(IQueryExecutor queryExecutor, IScriptFolderExecutor folderExecutor)
		{
			_queryExecutor = queryExecutor;
			_folderExecutor = folderExecutor;
		}

	    public DatabaseCreator():this(new QueryExecutor(),new ScriptFolderExecutor())
	    {
	        
	    }

	    public void Execute(TaskAttributes taskAttributes, ITaskObserver taskObserver)
		{
            string sql = string.Format("create database [{0}]", taskAttributes.ConnectionSettings.Database);
            _queryExecutor.ExecuteNonQuery(taskAttributes.ConnectionSettings, sql, false);

            _folderExecutor.ExecuteScriptsInFolder(taskAttributes, "Create", taskObserver);
		}
	}
}