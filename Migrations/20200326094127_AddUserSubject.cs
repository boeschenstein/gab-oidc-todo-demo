using Microsoft.EntityFrameworkCore.Migrations;

namespace Bif4DotNetDemo.Migrations
{
    public partial class AddUserSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserSubject",
                table: "ToDoItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserSubject",
                table: "ToDoItems");
        }
    }
}
