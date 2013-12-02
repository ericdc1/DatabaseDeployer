using NUnit.Framework;
using Rhino.Mocks;
using DatabaseDeployer.Core.Model;
using DatabaseDeployer.Core.Services;
using DatabaseDeployer.Core.Services.Impl;

namespace DatabaseDeployer.UnitTests.Core.DatabaseManager.Services
{
	[TestFixture]
	public class DatabaseCreatorTester
	{
		[Test]
		public void Creates_database()
		{
			var settings = new ConnectionSettings("server", "db", true, null, null);
            var taskAttributes = new TaskAttributes(settings, "c:\\scripts");

			var mocks = new MockRepository();
            var queryExecutor = mocks.StrictMock<IQueryExecutor>();
            var executor = mocks.StrictMock<IScriptFolderExecutor>();
            var taskObserver = mocks.StrictMock<ITaskObserver>();
			
			using (mocks.Record())
			{
				queryExecutor.ExecuteNonQuery(settings, "create database [db]", false);
				executor.ExecuteScriptsInFolder(taskAttributes, "Create", taskObserver);
			}

			using (mocks.Playback())
			{
				IDatabaseActionExecutor creator = new DatabaseCreator(queryExecutor, executor);
				creator.Execute(taskAttributes, taskObserver);
			}

			mocks.VerifyAll();
		}
	}
}