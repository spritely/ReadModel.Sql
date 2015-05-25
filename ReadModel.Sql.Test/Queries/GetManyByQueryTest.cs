// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetManyByQueryTest.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class GetManyByQueryTest
    {
        [Test]
        public void Handle_returns_expected_objects()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var models = testReadModelDatabase.AddModelItems(namePrefix: "Handle_returns_expected_object_", count: 3);

                var expectedModel = models.Skip(1).First();

                var query = new GetManyByIdQuery
                {
                    Id = expectedModel.Id,
                    ModelType = "TestModel"
                };

                var queryHandler = new GetManyByQueryHandler<TestReadModelDatabase, TestModel>(testReadModelDatabase);

                var results = queryHandler.Handle(query);

                Assert.That(results, Is.Not.Null);
                Assert.That(results.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Handle_returns_empty_when_no_results_found()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                testReadModelDatabase.AddModelItems(namePrefix: "Handle_returns_empty_when_no_results_found_", count: 3);

                var query = new GetManyByIdQuery
                {
                    Id = Guid.NewGuid(),
                    ModelType = "TestModel"
                };

                var queryHandler = new GetManyByQueryHandler<TestReadModelDatabase, TestModel>(testReadModelDatabase);

                var results = queryHandler.Handle(query);

                Assert.That(results, Is.Not.Null);
                Assert.That(results.Count, Is.EqualTo(0));
            }
        }

        [Test]
        public void Handle_returns_expected_objects_with_in_query()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var models = testReadModelDatabase.AddModelItems(namePrefix: "Handle_returns_expected_objects_with_in_query_", count: 6);

                var query = new GetManyByIdInQuery
                {
                    ModelType = "TestModel",
                    Id = models.Skip(2).Take(2).Select(m => m.Id)
                };

                var queryHandler = new GetManyByQueryHandler<TestReadModelDatabase, TestModel>(testReadModelDatabase);

                var results = queryHandler.Handle(query);

                Assert.That(results, Is.Not.Null);
                Assert.That(results.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public void Handle_returns_expected_objects_with_not_in_query()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var models = testReadModelDatabase.AddModelItems(namePrefix: "Handle_returns_expected_objects_with_not_in_query_", count: 6);

                var query = new GetManyByIdNotInQuery
                {
                    ModelType = "TestModel",
                    NotId = models.Skip(2).Take(2).Select(m => m.Id)
                };

                var queryHandler = new GetManyByQueryHandler<TestReadModelDatabase, TestModel>(testReadModelDatabase);

                var results = queryHandler.Handle(query);

                Assert.That(results, Is.Not.Null);
                Assert.That(results.Count, Is.EqualTo(4));
            }
        }

        private class GetManyByIdInQuery : GetManyByQuery<TestModel>
        {
            public IEnumerable<Guid> Id { get; set; }
        }

        private class GetManyByIdNotInQuery : GetManyByQuery<TestModel>
        {
            public IEnumerable<Guid> NotId { get; set; }
        }

        private class GetManyByIdQuery : GetManyByQuery<TestModel>
        {
            public Guid Id { get; set; }
        }
    }
}
