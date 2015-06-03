// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrUpdateCommandTest.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class AddOrUpdateCommandTest
    {
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "There is nothing unsafe in here since this is just a test."), Test]
        public void Handle_adds_row_when_none_exist()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var model = new TestModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Handle_adds_row_when_none_exist"
                };

                var command = new AddOrUpdateCommand<TestModel>
                {
                    Model = model,
                    ModelType = "TestModel"
                };

                var commandHandler = new AddOrUpdateCommandHandler<TestReadModelDatabase, TestModel>(testReadModelDatabase);

                commandHandler.Handle(command);

                testReadModelDatabase.TestDatabase.ExecuteCommand(
                    c =>
                    {
                        c.CommandText = string.Format(
                            CultureInfo.InvariantCulture,
                            "select Model from TestModel where Id = '{0}'",
                            model.Id);
                        using (var dataReader = c.ExecuteReader())
                        {
                            dataReader.Read();
                            var validateModel = JsonConvert.DeserializeObject<TestModel>(dataReader.GetString(0));
                            Assert.AreEqual("Handle_adds_row_when_none_exist", validateModel.Name);
                        }
                    });
            }
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "There is nothing unsafe in here since this is just a test."), Test]
        public void Handle_updates_row_when_one_exists()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var model = new TestModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "I should be overwritten"
                };

                var command = new AddOrUpdateCommand<TestModel>
                {
                    Model = model,
                    ModelType = "TestModel"
                };

                var commandHandler = new AddOrUpdateCommandHandler<TestReadModelDatabase, TestModel>(testReadModelDatabase);
                commandHandler.Handle(command);

                // Here's the update
                model.Name = "Handle_updates_row_when_one_exists";
                commandHandler.Handle(command);

                testReadModelDatabase.TestDatabase.ExecuteCommand(
                    c =>
                    {
                        c.CommandText = string.Format(
                            CultureInfo.InvariantCulture,
                            "select Model from TestModel where Id = '{0}'",
                            model.Id);
                        using (var dataReader = c.ExecuteReader())
                        {
                            dataReader.Read();
                            var validateModel = JsonConvert.DeserializeObject<TestModel>(dataReader.GetString(0));
                            Assert.AreEqual("Handle_updates_row_when_one_exists", validateModel.Name);
                        }
                    });
            }
        }
    }
}
