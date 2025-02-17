using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroSafar.Migrations
{
    /// <inheritdoc />
    public partial class newcourseandunit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseAdminId",
                table: "Units",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "courseAdmins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courseAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_courseAdmins_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "unitAdmins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CoursesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unitAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_unitAdmins_CoursesA_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "CoursesA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Units_CourseAdminId",
                table: "Units",
                column: "CourseAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_courseAdmins_CategoryId",
                table: "courseAdmins",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_unitAdmins_CoursesId",
                table: "unitAdmins",
                column: "CoursesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_courseAdmins_CourseAdminId",
                table: "Units",
                column: "CourseAdminId",
                principalTable: "courseAdmins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_courseAdmins_CourseAdminId",
                table: "Units");

            migrationBuilder.DropTable(
                name: "courseAdmins");

            migrationBuilder.DropTable(
                name: "unitAdmins");

            migrationBuilder.DropIndex(
                name: "IX_Units_CourseAdminId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "CourseAdminId",
                table: "Units");
        }
    }
}
