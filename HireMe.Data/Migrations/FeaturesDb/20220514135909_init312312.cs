using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMe.Data.Migrations.FeaturesDb
{
    public partial class init312312 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Stats",
                table: "Stats");

            migrationBuilder.RenameTable(
                name: "Stats",
                newName: "ContestantStats");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContestantStats",
                table: "ContestantStats",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CompanyStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    January = table.Column<int>(type: "int", nullable: false),
                    February = table.Column<int>(type: "int", nullable: false),
                    March = table.Column<int>(type: "int", nullable: false),
                    April = table.Column<int>(type: "int", nullable: false),
                    May = table.Column<int>(type: "int", nullable: false),
                    June = table.Column<int>(type: "int", nullable: false),
                    July = table.Column<int>(type: "int", nullable: false),
                    August = table.Column<int>(type: "int", nullable: false),
                    September = table.Column<int>(type: "int", nullable: false),
                    October = table.Column<int>(type: "int", nullable: false),
                    November = table.Column<int>(type: "int", nullable: false),
                    December = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    PosterId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyStats", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyStats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContestantStats",
                table: "ContestantStats");

            migrationBuilder.RenameTable(
                name: "ContestantStats",
                newName: "Stats");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stats",
                table: "Stats",
                column: "Id");
        }
    }
}
