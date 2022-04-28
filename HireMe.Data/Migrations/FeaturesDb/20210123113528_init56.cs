using Microsoft.EntityFrameworkCore.Migrations;

namespace HireMe.Data.Migrations.FeaturesDb
{
    public partial class init56 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ApplyCount",
                table: "Jobs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplyCount",
                table: "Jobs");
        }
    }
}
