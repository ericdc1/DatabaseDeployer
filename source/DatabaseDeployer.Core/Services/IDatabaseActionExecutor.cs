
using DatabaseDeployer.Core.Model;

namespace DatabaseDeployer.Core.Services
{
	public interface IDatabaseActionExecutor
	{
		void Execute(TaskAttributes taskAttributes, ITaskObserver taskObserver);
	}
}