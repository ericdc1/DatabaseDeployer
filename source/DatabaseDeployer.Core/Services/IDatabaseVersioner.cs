
using DatabaseDeployer.Core.Model;

namespace DatabaseDeployer.Core.Services
{
	
	public interface IDatabaseVersioner
	{
		void VersionDatabase(ConnectionSettings settings, ITaskObserver taskObserver);
	}
}