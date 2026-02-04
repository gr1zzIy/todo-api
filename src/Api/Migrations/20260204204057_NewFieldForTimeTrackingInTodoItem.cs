using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todoApi.Migrations
{
    /// <inheritdoc />
    public partial class NewFieldForTimeTrackingInTodoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Deadline",
                table: "Todos",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstimatedTimeMinutes",
                table: "Todos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpentTimeMinutes",
                table: "Todos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "EstimatedTimeMinutes",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "SpentTimeMinutes",
                table: "Todos");
        }
    }
}
