using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroSafar.Migrations
{
    public partial class UpdateProgresscourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ Add the 'CourseId' column if it does not exist
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "UnitProgresses",
                nullable: false,
                defaultValue: 0);

            // 2️⃣ Create an index for faster lookups
            migrationBuilder.CreateIndex(
                name: "IX_UnitProgresses_CourseId",
                table: "UnitProgresses",
                column: "CourseId");

            // 3️⃣ Add Foreign Key without cascade delete
            migrationBuilder.AddForeignKey(
                name: "FK_UnitProgresses_courseAdmins_CourseId",
                table: "UnitProgresses",
                column: "CourseId",
                principalTable: "courseAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction // Prevents multiple cascade paths
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove foreign key first
            migrationBuilder.DropForeignKey(
                name: "FK_UnitProgresses_courseAdmins_CourseId",
                table: "UnitProgresses");

            // Remove index
            migrationBuilder.DropIndex(
                name: "IX_UnitProgresses_CourseId",
                table: "UnitProgresses");

            // Remove the column
            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "UnitProgresses");
        }
    }
}
