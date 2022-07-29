using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMe.Data.Migrations.FeaturesDb
{
    public partial class init99999 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contestant_Category_CategoryId",
                table: "Contestant");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Category_CategoryId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_CategoryId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Contestant_CategoryId",
                table: "Contestant");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "CandidatesCount",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JobsCount",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CandidatesCount",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "JobsCount",
                table: "Category");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CategoryId",
                table: "Jobs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Contestant_CategoryId",
                table: "Contestant",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contestant_Category_CategoryId",
                table: "Contestant",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Category_CategoryId",
                table: "Jobs",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
