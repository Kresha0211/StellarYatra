using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroSafar.Migrations
{
    /// <inheritdoc />
    public partial class finalexamcourseid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "ExamQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_CourseId",
                table: "ExamQuestions",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestions_courseAdmins_CourseId",
                table: "ExamQuestions",
                column: "CourseId",
                principalTable: "courseAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestions_courseAdmins_CourseId",
                table: "ExamQuestions");

            migrationBuilder.DropIndex(
                name: "IX_ExamQuestions_CourseId",
                table: "ExamQuestions");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "ExamQuestions");
        }
    }
}
