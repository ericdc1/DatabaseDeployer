using System;
using DatabaseDeployer.Core.Model;

namespace DatabaseDeployer.Core.Services.Impl
{
	public class DatabaseSeeder : IDatabaseActionExecutor
	{
		private readonly IScriptFolderExecutor _folderExecutor;

		public DatabaseSeeder(IScriptFolderExecutor folderExecutor)
		{
			_folderExecutor = folderExecutor;
		}

        public DatabaseSeeder()
            : this(new ScriptFolderExecutor())
	    {
	        
	    }

	    public void Execute(TaskAttributes taskAttributes, ITaskObserver taskObserver)
		{
            _folderExecutor.ExecuteSeedScriptsInFolder(taskAttributes, "Seed", taskObserver);
		}
	}
}