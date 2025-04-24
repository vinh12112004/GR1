using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GR1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomCode = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomCode);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentCode = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentCode);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    StudentCode = table.Column<long>(type: "bigint", nullable: false),
                    RoomCode = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => new { x.StudentCode, x.RoomCode, x.Timestamp });
                    table.ForeignKey(
                        name: "FK_Logs_Rooms_RoomCode",
                        column: x => x.RoomCode,
                        principalTable: "Rooms",
                        principalColumn: "RoomCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Students_StudentCode",
                        column: x => x.StudentCode,
                        principalTable: "Students",
                        principalColumn: "StudentCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                column: "RoomCode",
                values: new object[]
                {
                    101,
                    102
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentCode", "FullName" },
                values: new object[,]
                {
                    { 20225779L, "Nguyen Van A" },
                    { 20225780L, "Le Thi B" }
                });

            migrationBuilder.InsertData(
                table: "Logs",
                columns: new[] { "RoomCode", "StudentCode", "Timestamp", "Status" },
                values: new object[,]
                {
                    { 101, 20225779L, new DateTime(2023, 1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), "IN" },
                    { 102, 20225780L, new DateTime(2023, 1, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), "OUT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_RoomCode",
                table: "Logs",
                column: "RoomCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
