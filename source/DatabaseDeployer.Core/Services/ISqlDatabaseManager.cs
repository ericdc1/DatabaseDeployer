using DatabaseDeployer.Core.Model;

namespace DatabaseDeployer.Core.Services
{
	public interface ISqlDatabaseManager
	{
		void Upgrade(TaskAttributes taskAttributes, ITaskObserver taskObserver);
	}
}