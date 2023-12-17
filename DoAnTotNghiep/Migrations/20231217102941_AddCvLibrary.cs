using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnTotNghiep.Migrations
{
    public partial class AddCvLibrary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CvLibraries",
                columns: table => new
                {
                    CvID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CvName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CvType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CvImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CvFile = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CvLibraries", x => x.CvID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CvLibraries");
        }
    }
}
