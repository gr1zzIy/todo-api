using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todoApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTodoItemStruct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Todos",
                newName: "Summary");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Todos",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "Todos",
                newName: "Title");
        }
    }
}
