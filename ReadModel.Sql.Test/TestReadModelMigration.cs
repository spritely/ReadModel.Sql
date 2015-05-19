// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestReadModelMigration.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql.Test
{
    using System;
    using FluentMigrator;

    [Migration(0)]
    public class TestReadModelMigration : Migration
    {
        public override void Down()
        {
            throw new NotSupportedException("Downgrading the base migration is not supported.");
        }

        public override void Up()
        {
            this.Create.Table("TestModel")
                .WithColumn("Id").AsString(100).NotNullable().PrimaryKey()
                .WithColumn("CreatedUtc").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("UpdatedUtc").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("Model").AsString(int.MaxValue).NotNullable();
        }
    }
}
