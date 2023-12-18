using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnTotNghiep.Migrations
{
    public partial class AddDiscuss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descrpitons",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Discusses",
                columns: table => new
                {
                    DiscussID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccountUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CandidateID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discusses", x => x.DiscussID);
                    table.ForeignKey(
                        name: "FK_Discusses_Accounts_AccountUserID",
                        column: x => x.AccountUserID,
                        principalTable: "Accounts",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_Discusses_Candidates_CandidateID",
                        column: x => x.CandidateID,
                        principalTable: "Candidates",
                        principalColumn: "CandidateID");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscussID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CandidateID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comments_Accounts_AccountUserID",
                        column: x => x.AccountUserID,
                        principalTable: "Accounts",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_Comments_Candidates_CandidateID",
                        column: x => x.CandidateID,
                        principalTable: "Candidates",
                        principalColumn: "CandidateID");
                    table.ForeignKey(
                        name: "FK_Comments_Discusses_DiscussID",
                        column: x => x.DiscussID,
                        principalTable: "Discusses",
                        principalColumn: "DiscussID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DiscussID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CandidateID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Likes_Accounts_AccountUserID",
                        column: x => x.AccountUserID,
                        principalTable: "Accounts",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_Likes_Candidates_CandidateID",
                        column: x => x.CandidateID,
                        principalTable: "Candidates",
                        principalColumn: "CandidateID");
                    table.ForeignKey(
                        name: "FK_Likes_Discusses_DiscussID",
                        column: x => x.DiscussID,
                        principalTable: "Discusses",
                        principalColumn: "DiscussID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AccountUserID",
                table: "Comments",
                column: "AccountUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CandidateID",
                table: "Comments",
                column: "CandidateID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DiscussID",
                table: "Comments",
                column: "DiscussID");

            migrationBuilder.CreateIndex(
                name: "IX_Discusses_AccountUserID",
                table: "Discusses",
                column: "AccountUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Discusses_CandidateID",
                table: "Discusses",
                column: "CandidateID");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_AccountUserID",
                table: "Likes",
                column: "AccountUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_CandidateID",
                table: "Likes",
                column: "CandidateID");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_DiscussID",
                table: "Likes",
                column: "DiscussID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Discusses");

            migrationBuilder.DropColumn(
                name: "Descrpitons",
                table: "Employers");
        }
    }
}
