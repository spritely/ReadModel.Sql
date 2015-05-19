// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetManyByIdQueryTest.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql.Test
{
    using System;
    using NUnit.Framework;

    // There isn't a good way to add this kind of data yet so these tests should be updated when
    // additional AddOrUpdateCommands are added for better querying options
    [TestFixture]
    public class GetManyByQueryTest
    {
        [Test]
        public void Handle_returns_expected_objects()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var expectedModel = new TestModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Handle_returns_expected_objects"
                };

                testReadModelDatabase.AddOrUpdateModel(expectedModel);

                var query = new GetManyById
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
                var model = new TestModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Handle_returns_empty_when_no_results_found"
                };

                testReadModelDatabase.AddOrUpdateModel(model);

                var query = new GetManyById
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

        private class GetManyById : GetManyByQuery<TestModel>
        {
            public Guid Id { get; set; }
        }
    }
}
