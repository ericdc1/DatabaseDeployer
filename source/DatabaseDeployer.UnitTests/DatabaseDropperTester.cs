using System;
using NUnit.Framework;
using Rhino.Mocks;
using DatabaseDeployer.Core.Model;
using DatabaseDeployer.Core.Services;
using DatabaseDeployer.Core.Services.Impl;

namespace DatabaseDeployer.UnitTests.Core.DatabaseManager.Services
{
	[TestFixture]
	public class DatabaseDropperTester
	{
		[Test]
		public void Drops_database()
		{
			var settings = new ConnectionSettings("server", "db", true, null, null);
            var taskAttributes = new TaskAttributes(settings, null);

			var mocks = new MockRepository();
            var connectionDropper = mocks.StrictMock<IDatabaseConnectionDropper>();
            var taskObserver = mocks.StrictMock<ITaskObserver>();
            var queryExecutor = mocks.StrictMock<IQueryExecutor>();
            
			using (mocks.Record())
			{
				connectionDropper.Drop(settings, taskObserver);
                
                queryExecutor.ExecuteNonQuery(settings, "ALTER DATABASE [db] SET SINGLE_USER WITH ROLLBACK IMMEDIATE drop database [db]", false);
			    
			}

			using (mocks.Playback())
			{
				IDatabaseActionExecutor dropper = new DatabaseDropper(connectionDropper, queryExecutor);
				dropper.Execute(taskAttributes, taskObserver);
			}

			mocks.VerifyAll();
		}

		[Test]
		public void Should_not_fail_if_datebase_does_not_exist()
		{
			var settings = new ConnectionSettings("server", "db", true, null, null);
            var taskAttributes = new TaskAttributes(settings, null);

			var mocks = new MockRepository();
			var connectionDropper = mocks.DynamicMock<IDatabaseConnectionDropper>();
            var taskObserver = mocks.StrictMock<ITaskObserver>();
            var queryExecutor = mocks.StrictMock<IQueryExecutor>();

			using (mocks.Record())
			{
                Expect.Call(() => queryExecutor.ExecuteNonQuery(settings, "ALTER DATABASE [db] SET SINGLE_USER WITH ROLLBACK IMMEDIATE drop database [db]", false))
					.Throw(new Exception("foo message"));
				Expect.Call(() => taskObserver.Log("Database 'db' could not be dropped."));
			}

			using (mocks.Playback())
			{
				IDatabaseActionExecutor dropper = new DatabaseDropper(connectionDropper, queryExecutor);
                dropper.Execute(taskAttributes, taskObserver);
			}

			mocks.VerifyAll();
		}
	}
}