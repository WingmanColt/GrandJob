using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMe.Data.Migrations.FeaturesDb
{
    public partial class init37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContestantDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Education_Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Education_StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Education_EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Work = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Work_Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Work_StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Work_EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Award = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Award_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Award_StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Award_EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PosterId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContestantDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContestantDetails");
        }
    }
}
