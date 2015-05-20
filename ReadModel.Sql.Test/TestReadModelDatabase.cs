// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestReadModelDatabase.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql.Test
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;
    using Spritely.Cqrs;
    using Spritely.Test.FluentMigratorSqlDatabase;

    public class TestReadModelDatabase : ReadModelDatabase<TestReadModelDatabase>, IDisposable
    {
        public TestReadModelDatabase()
        {
            this.TestDatabase = new TestDatabase("Create_creates_runner_capable_of_populating_database.mdf");

            this.TestDatabase.Create();

            this.ConnectionSettings = new DatabaseConnectionSettings();

            // Use TestMigration class in this assembly
            var runner = FluentMigratorRunnerFactory.Create(Assembly.GetExecutingAssembly(), this.TestDatabase.ConnectionString);
            runner.MigrateUp(0);
        }

        public TestDatabase TestDatabase { get; set; }

        public override IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(this.TestDatabase.ConnectionString);

            connection.Open();

            return connection;
        }

        public void Dispose()
        {
            this.TestDatabase.Dispose();
        }
    }
}
