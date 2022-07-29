using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMe.Data.Migrations.FeaturesDb
{
    public partial class init123e3121213 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "premiumPackage",
                table: "Promotion",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "premiumPackage",
                table: "Promotion");
        }
    }
}
