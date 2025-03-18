using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroSafar.Migrations
{
    /// <inheritdoc />
    public partial class unitprogress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnitProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: false),
                    CourseId = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitProgresses_courseAdmins_CourseId",
                        column: x => x.CourseId,
                        principalTable: "courseAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict); // ❌ No Cascade Delete

                    table.ForeignKey(
                        name: "FK_UnitProgresses_unitAdmins_UnitId",
                        column: x => x.UnitId,
                        principalTable: "unitAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict); // ❌ No Cascade Delete
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitProgresses_CourseId",
                table: "UnitProgresses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitProgresses_UnitId",
                table: "UnitProgresses",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitProgresses");
        }
    }
}
