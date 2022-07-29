using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMe.Data.Migrations.FeaturesDb
{
    public partial class init31231 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profileViews",
                table: "Contestant");

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Views",
                table: "Company");

            migrationBuilder.AddColumn<long>(
                name: "profileViews",
                table: "Contestant",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
