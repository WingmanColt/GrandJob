using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HireMe.Data.Migrations.FeaturesDb
{
    public partial class init555555 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyTestId",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanyTest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isAuthentic_EIK = table.Column<bool>(type: "bit", nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Private = table.Column<bool>(type: "bit", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Linkdin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    RatingVotes = table.Column<int>(type: "int", nullable: false),
                    VotedUsers = table.Column<int>(type: "int", nullable: false),
                    PosterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Admin1_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Admin2_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Admin3_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isApproved = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Promotion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContestantTest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genders = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    payRate = table.Column<int>(type: "int", nullable: false),
                    SalaryType = table.Column<int>(type: "int", nullable: false),
                    profileVisiblity = table.Column<int>(type: "int", nullable: false),
                    WorkType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    profileViews = table.Column<long>(type: "bigint", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Portfolio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Linkdin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Github = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dribbble = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Promotion = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    RatingVotes = table.Column<int>(type: "int", nullable: false),
                    VotedUsers = table.Column<int>(type: "int", nullable: false),
                    Views = table.Column<long>(type: "bigint", nullable: false),
                    ResumeFileId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userSkillsId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguagesId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PosterID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isApproved = table.Column<int>(type: "int", nullable: false),
                    isArchived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContestantTest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobsTest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExprienceLevels = table.Column<int>(type: "int", nullable: false),
                    JobType = table.Column<int>(type: "int", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinSalary = table.Column<long>(type: "bigint", nullable: false),
                    MaxSalary = table.Column<long>(type: "bigint", nullable: false),
                    SalaryType = table.Column<int>(type: "int", nullable: false),
                    Promotion = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    RatingVotes = table.Column<int>(type: "int", nullable: false),
                    VotedUsers = table.Column<int>(type: "int", nullable: false),
                    Views = table.Column<long>(type: "bigint", nullable: false),
                    ApplyCount = table.Column<long>(type: "bigint", nullable: false),
                    resumeFilesId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosterID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isApproved = table.Column<int>(type: "int", nullable: false),
                    isArchived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobsTest", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CompanyTestId",
                table: "Jobs",
                column: "CompanyTestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_CompanyTest_CompanyTestId",
                table: "Jobs",
                column: "CompanyTestId",
                principalTable: "CompanyTest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_CompanyTest_CompanyTestId",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "CompanyTest");

            migrationBuilder.DropTable(
                name: "ContestantTest");

            migrationBuilder.DropTable(
                name: "JobsTest");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_CompanyTestId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CompanyTestId",
                table: "Jobs");
        }
    }
}
