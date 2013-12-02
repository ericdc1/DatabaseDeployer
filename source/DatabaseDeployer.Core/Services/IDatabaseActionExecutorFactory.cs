using System.Collections.Generic;

using DatabaseDeployer.Core.Services.Impl;

namespace DatabaseDeployer.Core.Services
{
	
	public interface IDatabaseActionExecutorFactory
	{
		IEnumerable<IDatabaseActionExecutor> GetExecutors(RequestedDatabaseAction requestedDatabaseAction);
	}
}