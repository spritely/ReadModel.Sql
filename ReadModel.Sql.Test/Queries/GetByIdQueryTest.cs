// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetByIdQueryTest.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class GetByIdQueryTest
    {
        [Test]
        public void Handle_returns_expected_object()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var expectedModel = new TestModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Handle_returns_expected_object"
                };

                testReadModelDatabase.AddOrUpdateModel(expectedModel);

                var query = new GetByIdQuery<Guid, TestModel>
                {
                    Id = expectedModel.Id,
                    ModelType = "TestModel"
                };

                var queryHandler = new GetOneByQueryHandler<TestReadModelDatabase, TestModel>(testReadModelDatabase);

                var actualModel = queryHandler.Handle(query);

                Assert.That(actualModel.Id, Is.EqualTo(expectedModel.Id));
                Assert.That(actualModel.Name, Is.EqualTo(expectedModel.Name));
            }
        }

        [Test]
        public void Handle_returns_null_when_no_match_found()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var model = new TestModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Handle_returns_null_when_no_match_found"
                };

                testReadModelDatabase.AddOrUpdateModel(model);

                var query = new GetByIdQuery<Guid, TestModel>
                {
                    Id = Guid.NewGuid(),
                    ModelType = "TestModel"
                };

                var queryHandler = new GetOneByQueryHandler<TestReadModelDatabase, TestModel>(testReadModelDatabase);

                var actualModel = queryHandler.Handle(query);

                Assert.That(actualModel, Is.Null);
            }
        }
    }
}
