using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMe.Data.Migrations.FeaturesDb
{
    public partial class init5666 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Friday",
                table: "JobStats");

            migrationBuilder.DropColumn(
                name: "Monday",
                table: "JobStats");

            migrationBuilder.DropColumn(
                name: "Saturday",
                table: "JobStats");

            migrationBuilder.DropColumn(
                name: "Sunday",
                table: "JobStats");

            migrationBuilder.DropColumn(
                name: "Thursday",
                table: "JobStats");

            migrationBuilder.DropColumn(
                name: "Wednesday",
                table: "JobStats");

            migrationBuilder.DropColumn(
                name: "Тuesday",
                table: "JobStats");

            migrationBuilder.AddColumn<string>(
                name: "ViewsPerDay",
                table: "JobStats",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewsPerDay",
                table: "JobStats");

            migrationBuilder.AddColumn<int>(
                name: "Friday",
                table: "JobStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Monday",
                table: "JobStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Saturday",
                table: "JobStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sunday",
                table: "JobStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Thursday",
                table: "JobStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Wednesday",
                table: "JobStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Тuesday",
                table: "JobStats",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
