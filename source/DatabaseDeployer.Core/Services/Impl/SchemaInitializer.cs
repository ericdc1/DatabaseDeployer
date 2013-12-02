using System;
using DatabaseDeployer.Core.Model;
using DatabaseDeployer.Core;
using DatabaseDeployer.Infrastructure.DatabaseManager.DataAccess;

namespace DatabaseDeployer.Core.Services.Impl
{
	
	public class SchemaInitializer : ISchemaInitializer
	{
		private readonly IQueryExecutor _executor;
		private readonly IResourceFileLocator _locator;

		public SchemaInitializer(IResourceFileLocator locator, IQueryExecutor executor)
		{
			_locator = locator;
			_executor = executor;
		}

	    public SchemaInitializer():this(new ResourceFileLocator(), new QueryExecutor())
	    {
	        
	    }

	    public void EnsureSchemaCreated(ConnectionSettings settings)
		{
			string assembly = SqlDatabaseManager.SQL_FILE_ASSEMBLY;
			string sqlFile = string.Format(SqlDatabaseManager.SQL_FILE_TEMPLATE, "CreateSchema");

			string sql = _locator.ReadTextFile(assembly, sqlFile);

			_executor.ExecuteNonQuery(settings, sql, true);
		}
        public void EnsureSeedSchemaCreated(ConnectionSettings settings)
        {
            string assembly = SqlDatabaseManager.SQL_FILE_ASSEMBLY;
            string sqlFile = string.Format(SqlDatabaseManager.SQL_FILE_TEMPLATE, "CreateSeedSchema");

            string sql = _locator.ReadTextFile(assembly, sqlFile);

            _executor.ExecuteNonQuery(settings, sql, true);
        }
	}
}