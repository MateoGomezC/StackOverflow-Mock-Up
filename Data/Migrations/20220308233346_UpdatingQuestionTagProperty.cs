using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflow.Data.Migrations
{
    public partial class UpdatingQuestionTagProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionTag");

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TagId",
                table: "Questions",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Tags_TagId",
                table: "Questions",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Tags_TagId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TagId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Questions");

            migrationBuilder.CreateTable(
                name: "QuestionTag",
                columns: table => new
                {
                    QuestionsId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTag", x => new { x.QuestionsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_QuestionTag_Questions_QuestionsId",
                        column: x => x.QuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTag_TagsId",
                table: "QuestionTag",
                column: "TagsId");
        }
    }
}
