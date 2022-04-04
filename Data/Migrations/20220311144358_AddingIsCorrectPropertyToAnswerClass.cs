using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflow.Data.Migrations
{
    public partial class AddingIsCorrectPropertyToAnswerClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "Answers");
        }
    }
}
