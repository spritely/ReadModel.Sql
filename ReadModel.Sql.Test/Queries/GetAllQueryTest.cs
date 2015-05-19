// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllQueryTest.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql.Test
{
    using System;
    using NUnit.Framework;
    using Spritely.Recipes;

    [TestFixture]
    public class GetAllQueryTest
    {
        [Test]
        public void Handle_returns_expected_objects()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var i = 0;
                var models = new[]
                {
                    new TestModel()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Handle_returns_expected_objects_" + i
                    },
                    new TestModel()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Handle_returns_expected_objects_" + i
                    },
                    new TestModel()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Handle_returns_expected_objects_" + i
                    }
                };

                models.ForEach(m => testReadModelDatabase.AddOrUpdateModel(m));

                var query = new GetAllQuery<TestModel>
                {
                    ModelType = "TestModel"
                };

                var queryHandler = new GetAllQueryHandler<TestReadModelDatabase, TestModel>(testReadModelDatabase);

                var results = queryHandler.Handle(query);

                Assert.That(results.Count, Is.EqualTo(3));
            }
        }

        [Test]
        public void Handle_returns_empty_when_no_results_found()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var query = new GetAllQuery<TestModel>
                {
                    ModelType = "TestModel"
                };

                var queryHandler = new GetAllQueryHandler<TestReadModelDatabase, TestModel>(testReadModelDatabase);

                var results = queryHandler.Handle(query);

                Assert.That(results, Is.Not.Null);
                Assert.That(results.Count, Is.EqualTo(0));
            }
        }
    }
}
