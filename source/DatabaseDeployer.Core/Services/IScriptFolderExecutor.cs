
using DatabaseDeployer.Core.Model;

namespace DatabaseDeployer.Core.Services
{
	
	public interface IScriptFolderExecutor
	{
		void ExecuteScriptsInFolder(TaskAttributes taskAttributes, string scriptDirectory, ITaskObserver taskObserver);
        void ExecuteSeedScriptsInFolder(TaskAttributes taskAttributes, string scriptDirectory, ITaskObserver taskObserver);

	}
}