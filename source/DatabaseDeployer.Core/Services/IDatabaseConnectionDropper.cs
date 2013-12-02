
using DatabaseDeployer.Core.Model;

namespace DatabaseDeployer.Core.Services
{
	
	public interface IDatabaseConnectionDropper
	{
		void Drop(ConnectionSettings settings, ITaskObserver taskObserver);
	}
}