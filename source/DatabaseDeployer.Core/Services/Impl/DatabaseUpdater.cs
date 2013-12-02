using System;
using DatabaseDeployer.Core.Model;

namespace DatabaseDeployer.Core.Services.Impl
{
	public class DatabaseUpdater : IDatabaseActionExecutor
	{
		private readonly IScriptFolderExecutor _folderExecutor;

		public DatabaseUpdater(IScriptFolderExecutor folderExecutor)
		{
			_folderExecutor = folderExecutor;
		}

	    public DatabaseUpdater():this(new ScriptFolderExecutor())
	    {
	        
	    }

	    public void Execute(TaskAttributes taskAttributes, ITaskObserver taskObserver)
		{
            _folderExecutor.ExecuteScriptsInFolder(taskAttributes, "Update", taskObserver);
		}
	}
}