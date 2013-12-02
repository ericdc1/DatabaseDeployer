using DatabaseDeployer.Core.Model;
using DatabaseDeployer.Core.Services;
using DatabaseDeployer.Core.Services.Impl;
using NUnit.Framework;
using Rhino.Mocks;
using DatabaseDeployer.Core;

namespace DatabaseDeployer.UnitTests.Core.DatabaseManager.Services
{
	[TestFixture]
	public class DatabaseConnectionDropperTester
	{
		[Test]
		public void Correctly_drops_connections()
		{
			string assembly = SqlDatabaseManager.SQL_FILE_ASSEMBLY;
			string sqlFile = string.Format(SqlDatabaseManager.SQL_FILE_TEMPLATE, "DropConnections");

			ConnectionSettings settings = new ConnectionSettings("server", "MyDatabase", true, null, null);

			MockRepository mocks = new MockRepository();

            ITaskObserver taskObserver = mocks.StrictMock<ITaskObserver>();
            IResourceFileLocator fileLocator = mocks.StrictMock<IResourceFileLocator>();
            ITokenReplacer replacer = mocks.StrictMock<ITokenReplacer>();
            IQueryExecutor queryExecutor = mocks.StrictMock<IQueryExecutor>();

			using (mocks.Record())
			{
				taskObserver.Log("Dropping connections for database MyDatabase\n");
				Expect.Call(fileLocator.ReadTextFile(assembly, sqlFile)).Return("Unformatted SQL");
				replacer.Text = "Unformatted SQL";
				replacer.Replace("DatabaseName", "MyDatabase");
				Expect.Call(replacer.Text).Return("Formatted SQL");
				queryExecutor.ExecuteNonQuery(settings, "Formatted SQL", false);
			}

			using (mocks.Playback())
			{
				IDatabaseConnectionDropper dropper = new DatabaseConnectionDropper(fileLocator, replacer, queryExecutor);
				dropper.Drop(settings, taskObserver);
			}

			mocks.VerifyAll();
		}
	}
}