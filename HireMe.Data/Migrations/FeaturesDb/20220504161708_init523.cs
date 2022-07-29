using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMe.Data.Migrations.FeaturesDb
{
    public partial class init523 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResumeType",
                table: "Resume",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumeType",
                table: "Resume");
        }
    }
}
