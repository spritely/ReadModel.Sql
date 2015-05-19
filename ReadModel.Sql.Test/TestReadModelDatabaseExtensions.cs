// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestReadModelDatabaseExtensions.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql.Test
{
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
    }
}
