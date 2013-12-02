
using DatabaseDeployer.Core.Model;

namespace DatabaseDeployer.Core.Services
{
	
	public interface ISeedScriptExecutor
	{
		void Execute(string fullFilename, ConnectionSettings settings, ITaskObserver taskObserver);
	}
}