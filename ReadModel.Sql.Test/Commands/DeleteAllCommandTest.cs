// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteAllCommandTest.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class DeleteAllCommandTest
    {
        [Test]
        public void Handle_deletes_all_objects()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                testReadModelDatabase.AddModelItems(namePrefix: "Handle_deletes_all_objects_", count: 3);

                var command = new DeleteAllCommand
                {
                    ModelType = "TestModel"
                };

                var commandHandler = new DeleteAllCommandHandler<TestReadModelDatabase>(testReadModelDatabase);
                commandHandler.Handle(command);

                var results = testReadModelDatabase.GetAllModelItems();

                Assert.That(results.Count, Is.EqualTo(0));
            }
        }
    }
}
