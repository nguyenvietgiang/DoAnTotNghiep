using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnTotNghiep.Migrations
{
    public partial class FixOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerCounts_Option_OptionId",
                table: "AnswerCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_Questions_QuestionId",
                table: "Option");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Option",
                table: "Option");

            migrationBuilder.RenameTable(
                name: "Option",
                newName: "Options");

            migrationBuilder.RenameIndex(
                name: "IX_Option_QuestionId",
                table: "Options",
                newName: "IX_Options_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Options",
                table: "Options",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerCounts_Options_OptionId",
                table: "AnswerCounts",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "OptionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerCounts_Options_OptionId",
                table: "AnswerCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Options",
                table: "Options");

            migrationBuilder.RenameTable(
                name: "Options",
                newName: "Option");

            migrationBuilder.RenameIndex(
                name: "IX_Options_QuestionId",
                table: "Option",
                newName: "IX_Option_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Option",
                table: "Option",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerCounts_Option_OptionId",
                table: "AnswerCounts",
                column: "OptionId",
                principalTable: "Option",
                principalColumn: "OptionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Questions_QuestionId",
                table: "Option",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
