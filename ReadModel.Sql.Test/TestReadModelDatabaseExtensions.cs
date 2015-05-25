// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestReadModelDatabaseExtensions.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql.Test
{
    using System;
    using System.Collections.Generic;

    internal static class TestReadModelDatabaseExtensions
    {
        public static void AddOrUpdateModel(this TestReadModelDatabase testReadModelDatabase, TestModel model)
        {
            var command = new AddOrUpdateCommand<TestModel>
            {
                Model = model,
                ModelType = "TestModel"
            };

            var commandHandler = new AddOrUpdateCommandHandler<TestReadModelDatabase, TestModel>(testReadModelDatabase);

            commandHandler.Handle(command);
        }

        public static ICollection<TestModel> AddModelItems(
            this TestReadModelDatabase testReadModelDatabase,
            string namePrefix,
            int count = 3)
        {
            var testModels = new List<TestModel>();

            for (var i = 0; i < count; i++)
            {
                var model = new TestModel()
                {
                    Id = Guid.NewGuid(),
                    Name = namePrefix + i
                };

                testModels.Add(model);

                testReadModelDatabase.AddOrUpdateModel(model);
            }

            return testModels;
        }

        public static IReadOnlyCollection<TestModel> GetAllModelItems(this TestReadModelDatabase testReadModelDatabase)
        {
            var query = new GetAllQuery<TestModel>
            {
                ModelType = "TestModel"
            };

            var queryHandler = new GetAllQueryHandler<TestReadModelDatabase, TestModel>(testReadModelDatabase);

            var results = queryHandler.Handle(query);

            return results;
        }
    }
}
