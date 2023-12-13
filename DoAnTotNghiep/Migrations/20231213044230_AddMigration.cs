using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnTotNghiep.Migrations
{
    public partial class AddMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountRole = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    CandidateID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descrpitons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    EducationLevel = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.CandidateID);
                    table.ForeignKey(
                        name: "FK_Candidates_Accounts_CandidateID",
                        column: x => x.CandidateID,
                        principalTable: "Accounts",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employers",
                columns: table => new
                {
                    EmployerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanySize = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employers", x => x.EmployerID);
                    table.ForeignKey(
                        name: "FK_Employers_Accounts_EmployerID",
                        column: x => x.EmployerID,
                        principalTable: "Accounts",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Employers");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
