using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroSafar.Migrations
{
    /// <inheritdoc />
    public partial class unitprogressadd : Migration
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
                    IsCompleted = table.Column<bool>(nullable: false),
                    ProgressPercentage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitProgresses_CourseAdmins_CourseId",
                        column: x => x.CourseId,
                        principalTable: "CourseAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict); // Prevents cascade delete

                    table.ForeignKey(
                        name: "FK_UnitProgresses_UnitAdmins_UnitId",
                        column: x => x.UnitId,
                        principalTable: "UnitAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict); // Prevents cascade delete
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
            migrationBuilder.DropTable(name: "UnitProgresses");
        }
    }
}
