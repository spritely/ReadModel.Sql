// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrUpdateCommandTest.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql.Test
{
    using System;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class AddOrUpdateCommandTest
    {
        [Test]
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
                        c.CommandText = string.Format("select Model from TestModel where Id = '{0}'", model.Id);
                        using (var dataReader = c.ExecuteReader())
                        {
                            dataReader.Read();
                            var validateModel = JsonConvert.DeserializeObject<TestModel>(dataReader.GetString(0));
                            Assert.AreEqual("Handle_adds_row_when_none_exist", validateModel.Name);
                        }
                    });
            }
        }

        [Test]
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
                        c.CommandText = string.Format("select Model from TestModel where Id = '{0}'", model.Id);
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
