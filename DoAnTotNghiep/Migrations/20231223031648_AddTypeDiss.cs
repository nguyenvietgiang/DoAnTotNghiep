using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnTotNghiep.Migrations
{
    public partial class AddTypeDiss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Candidates_CandidateID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Discusses_Candidates_CandidateID",
                table: "Discusses");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Candidates_CandidateID",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_CandidateID",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Discusses_CandidateID",
                table: "Discusses");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CandidateID",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CandidateID",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "CandidateID",
                table: "Discusses");

            migrationBuilder.DropColumn(
                name: "CandidateID",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Discusses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Discusses");

            migrationBuilder.AddColumn<Guid>(
                name: "CandidateID",
                table: "Likes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CandidateID",
                table: "Discusses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CandidateID",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_CandidateID",
                table: "Likes",
                column: "CandidateID");

            migrationBuilder.CreateIndex(
                name: "IX_Discusses_CandidateID",
                table: "Discusses",
                column: "CandidateID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CandidateID",
                table: "Comments",
                column: "CandidateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Candidates_CandidateID",
                table: "Comments",
                column: "CandidateID",
                principalTable: "Candidates",
                principalColumn: "CandidateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Discusses_Candidates_CandidateID",
                table: "Discusses",
                column: "CandidateID",
                principalTable: "Candidates",
                principalColumn: "CandidateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Candidates_CandidateID",
                table: "Likes",
                column: "CandidateID",
                principalTable: "Candidates",
                principalColumn: "CandidateID");
        }
    }
}
