﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllQueryTest.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class GetAllQueryTest
    {
        [Test]
        public void Handle_returns_expected_objects()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var expectedCount = 3;
                testReadModelDatabase.AddModelItems(namePrefix: "Handle_returns_expected_objects_", count: expectedCount);

                var results = testReadModelDatabase.GetAllModelItems();

                Assert.That(results.Count, Is.EqualTo(expectedCount));
            }
        }

        [Test]
        public void Handle_returns_empty_when_no_results_found()
        {
            using (var testReadModelDatabase = new TestReadModelDatabase())
            {
                var results = testReadModelDatabase.GetAllModelItems();

                Assert.That(results, Is.Not.Null);
                Assert.That(results.Count, Is.EqualTo(0));
            }
        }
    }
}
