
using DatabaseDeployer.Core.Model;

namespace DatabaseDeployer.Core.Services
{
	
	public interface IChangeScriptExecutor
	{
		void Execute(string fullFilename, ConnectionSettings settings, ITaskObserver taskObserver, bool logOnly = false);
	}
}