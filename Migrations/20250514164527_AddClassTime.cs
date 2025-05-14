using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GR1.Migrations
{
    /// <inheritdoc />
    public partial class AddClassTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AddColumn<DateTime>(
        name: "StartTime",
        table: "Classes",
        nullable: false,
        defaultValue: DateTime.Now);

    migrationBuilder.AddColumn<DateTime>(
        name: "EndTime",
        table: "Classes",
        nullable: false,
        defaultValue: DateTime.Now);
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropColumn(
        name: "StartTime",
        table: "Classes");

    migrationBuilder.DropColumn(
        name: "EndTime",
        table: "Classes");
}
    }
}
