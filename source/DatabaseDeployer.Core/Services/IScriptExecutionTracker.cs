
using DatabaseDeployer.Core.Model;

namespace DatabaseDeployer.Core.Services
{
	
	public interface IScriptExecutionTracker
	{
		void MarkScriptAsExecuted(ConnectionSettings settings, string scriptFilename, ITaskObserver task);
		bool ScriptAlreadyExecuted(ConnectionSettings settings, string scriptFilename);
  
		void MarkSeedScriptAsExecuted(ConnectionSettings settings, string scriptFilename, ITaskObserver task);
		bool SeedScriptAlreadyExecuted(ConnectionSettings settings, string scriptFilename);
	}
}