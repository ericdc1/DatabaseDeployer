
using DatabaseDeployer.Core.Model;

namespace DatabaseDeployer.Core.Services
{
	
	public interface ISchemaInitializer
	{
		void EnsureSchemaCreated(ConnectionSettings settings);
        void EnsureSeedSchemaCreated(ConnectionSettings settings);
	}
}