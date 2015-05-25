// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteByIdCommandTest.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql.Test
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class DeleteByCommandTest
    {
        [Test]
        public void Handle_deletes_only_expected_objects()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var models = testReadModelDatabase.AddModelItems(namePrefix: "Handle_deletes_only_expected_objects", count: 3);

                var expectedModel = models.Skip(1).First();

                var command = new DeleteByIdCommand<Guid>
                {
                    Id = expectedModel.Id,
                    ModelType = "TestModel"
                };

                var commandHandler = new DeleteByCommandHandler<TestReadModelDatabase>(testReadModelDatabase);
                commandHandler.Handle(command);

                var results = testReadModelDatabase.GetAllModelItems();

                Assert.That(results.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public void Handle_deletes_expected_objects_with_in_query()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var models = testReadModelDatabase.AddModelItems(namePrefix: "Handle_deletes_expected_objects_with_in_query_", count: 6);

                var query = new DeleteManyByIdInCommand
                {
                    ModelType = "TestModel",
                    Id = models.Skip(2).Take(2).Select(m => m.Id)
                };

                var commandHandler = new DeleteByCommandHandler<TestReadModelDatabase>(testReadModelDatabase);
                commandHandler.Handle(query);

                var results = testReadModelDatabase.GetAllModelItems();

                Assert.That(results.Count, Is.EqualTo(4));
            }
        }

        [Test]
        public void Handle_deletes_expected_objects_with_not_in_query()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var models = testReadModelDatabase.AddModelItems(namePrefix: "Handle_deletes_expected_objects_with_not_in_query_", count: 6);

                var query = new DeleteManyByIdNotInCommand
                {
                    ModelType = "TestModel",
                    NotId = models.Skip(2).Take(2).Select(m => m.Id)
                };

                var commandHandler = new DeleteByCommandHandler<TestReadModelDatabase>(testReadModelDatabase);
                commandHandler.Handle(query);

                var results = testReadModelDatabase.GetAllModelItems();

                Assert.That(results.Count, Is.EqualTo(2));
            }
        }

        private class DeleteManyByIdInCommand : DeleteByCommand
        {
            public IEnumerable<Guid> Id { get; set; }
        }

        private class DeleteManyByIdNotInCommand : DeleteByCommand
        {
            public IEnumerable<Guid> NotId { get; set; }
        }
    }
}
