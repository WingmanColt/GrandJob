using Microsoft.EntityFrameworkCore.Migrations;

namespace HireMe.Data.Migrations.FeaturesDb
{
    public partial class init555 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsComplated",
                table: "Tasks",
                newName: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Tasks",
                newName: "IsComplated");
        }
    }
}
