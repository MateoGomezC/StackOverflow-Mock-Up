using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflow.Data.Migrations
{
    public partial class UpdatingCommentClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AnswerId",
                table: "Comments",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Answers_AnswerId",
                table: "Comments",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Answers_AnswerId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AnswerId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Comments");
        }
    }
}
