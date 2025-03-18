using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroSafar.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_unitAdmins_courseAdmins_CoursesId",
                table: "unitAdmins");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "CoursesA");

            migrationBuilder.DropIndex(
                name: "IX_unitAdmins_CoursesId",
                table: "unitAdmins");

            migrationBuilder.DropColumn(
                name: "CoursesId",
                table: "unitAdmins");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "courseAdmins",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_unitAdmins_CourseId",
                table: "unitAdmins",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_unitAdmins_courseAdmins_CourseId",
                table: "unitAdmins",
                column: "CourseId",
                principalTable: "courseAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_unitAdmins_courseAdmins_CourseId",
                table: "unitAdmins");

            migrationBuilder.DropIndex(
                name: "IX_unitAdmins_CourseId",
                table: "unitAdmins");

            migrationBuilder.AddColumn<int>(
                name: "CoursesId",
                table: "unitAdmins",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "courseAdmins",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "CoursesA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoursesId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_CoursesA_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "CoursesA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_unitAdmins_CoursesId",
                table: "unitAdmins",
                column: "CoursesId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursesA_CategoryId",
                table: "CoursesA",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_CoursesId",
                table: "Units",
                column: "CoursesId");

            migrationBuilder.AddForeignKey(
                 name: "FK_unitAdmins_courseAdmins_CoursesId",
                table: "unitAdmins",
                column: "CoursesId",
                principalTable: "courseAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
