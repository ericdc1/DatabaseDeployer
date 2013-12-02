using System.Collections.Generic;

using DatabaseDeployer.Core.Services.Impl;
using DatabaseDeployer.Core.Services.Impl.DatabaseDeployer.Core.Services.Impl;

namespace DatabaseDeployer.Core.Services
{
	
	public interface IDatabaseActionResolver
	{
		IEnumerable<DatabaseAction> GetActions(RequestedDatabaseAction requestedDatabaseAction);
	}
}