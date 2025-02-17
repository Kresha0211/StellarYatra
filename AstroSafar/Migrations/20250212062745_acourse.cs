using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroSafar.Migrations
{
    /// <inheritdoc />
    public partial class acourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Categories_CategoryId",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Course_CourseId",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Courses");

            migrationBuilder.RenameIndex(
                name: "IX_Course_CategoryId",
                table: "Courses",
                newName: "IX_Courses_CategoryId");

            migrationBuilder.AddColumn<int>(
                name: "CourseAId",
                table: "Units",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "CourseID");

            migrationBuilder.CreateTable(
                name: "CoursesA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursesA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoursesA_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Units_CourseAId",
                table: "Units",
                column: "CourseAId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursesA_CategoryId",
                table: "CoursesA",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Categories_CategoryId",
                table: "Courses",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_CoursesA_CourseAId",
                table: "Units",
                column: "CourseAId",
                principalTable: "CoursesA",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Courses_CourseId",
                table: "Units",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Categories_CategoryId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_CoursesA_CourseAId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Courses_CourseId",
                table: "Units");

            migrationBuilder.DropTable(
                name: "CoursesA");

            migrationBuilder.DropIndex(
                name: "IX_Units_CourseAId",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseAId",
                table: "Units");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_CategoryId",
                table: "Course",
                newName: "IX_Course_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Categories_CategoryId",
                table: "Course",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Course_CourseId",
                table: "Units",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
