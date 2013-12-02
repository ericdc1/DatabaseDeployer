using System;
using System.Collections.Generic;
using DatabaseDeployer.Core.Services;
using DatabaseDeployer.Core.Services.Impl;
using DatabaseDeployer.Infrastructure.DatabaseManager.DataAccess;

namespace DatabaseDeployer.Core
{
    public class Factory
    {
        public static ISqlDatabaseManager Create()
        {
            return new SqlDatabaseManager();
        }
    }
}