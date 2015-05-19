// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadModelDatabase.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql
{
    using System.Data;
    using Spritely.Cqrs;

    /// <summary>
    ///     Class representing a read model database. Commands and queries accessing the read model require this.
    /// </summary>
    public class ReadModelDatabase<T> : IDatabase where T : ReadModelDatabase<T>
    {
        /// <summary>
        ///     Gets or sets the connection settings.
        /// </summary>
        /// <value>
        ///     The connection settings.
        /// </value>
        public DatabaseConnectionSettings ConnectionSettings { get; set; }

        /// <summary>
        ///     Creates a database connection (user is expected to properly dispose of instance).
        /// </summary>
        /// <returns>A new database connection (user is expected to properly dispose of instance)</returns>
        public virtual IDbConnection CreateConnection()
        {
            var connection = this.ConnectionSettings.CreateSqlConnection();

            connection.Open();

            return connection;
        }

        public virtual string ReadModel(dynamic instance, string modelType)
        {
            return instance.Model;
        }

        public virtual string ReadId<TModel>(TModel instance)
        {
            return (instance as dynamic).Id.ToString();
        }

        public virtual string GetIdColumnName(string modelType)
        {
            return "Id";
        }

        public virtual string GetModelColumnName(string modelType)
        {
            return "Model";
        }

        public virtual string GetUpdatedColumnName(string modelType)
        {
            return "UpdatedUtc";
        }
    }
}
