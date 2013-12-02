
using DatabaseDeployer.Core.Model;

namespace DatabaseDeployer.Core.Services
{
	
	public interface IConnectionStringGenerator
	{
		string GetConnectionString(ConnectionSettings settings, bool includeDatabaseName);
	}
}