using Microsoft.EntityFrameworkCore.Migrations;

namespace HireMe.Data.Migrations.FeaturesDb
{
    public partial class init45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "Tasks");
        }
    }
}
