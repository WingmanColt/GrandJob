using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMe.Data.Migrations.FeaturesDb
{
    public partial class init9999 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Notification",
                newName: "SenderId");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Notification",
                newName: "UserId");
        }
    }
}
