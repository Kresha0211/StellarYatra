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
            // Drop the problematic foreign key only if it exists
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_unitAdmins_CoursesA_CoursesId')
        ALTER TABLE [unitAdmins] DROP CONSTRAINT [FK_unitAdmins_CoursesA_CoursesId];
    ");

            // Drop Index only if it exists
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_unitAdmins_CoursesId' AND object_id = OBJECT_ID('unitAdmins'))
        DROP INDEX [IX_unitAdmins_CoursesId] ON [unitAdmins];
    ");

            // Now it's safe to drop the column
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT 1 FROM sys.columns WHERE name = 'CoursesId' AND object_id = OBJECT_ID('unitAdmins'))
        ALTER TABLE [unitAdmins] DROP COLUMN [CoursesId];
    ");

            // Modify column
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "courseAdmins",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Create new index
            migrationBuilder.CreateIndex(
                name: "IX_unitAdmins_CourseId",
                table: "unitAdmins",
                column: "CourseId");

            // Add new foreign key
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
            // Drop new foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_unitAdmins_courseAdmins_CourseId",
                table: "unitAdmins");

            // Drop new index
            migrationBuilder.DropIndex(
                name: "IX_unitAdmins_CourseId",
                table: "unitAdmins");

            // Restore old column (if needed)
            migrationBuilder.AddColumn<int>(
                name: "CoursesId",
                table: "unitAdmins",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Revert column change
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "courseAdmins",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            // Recreate tables if they were dropped
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

            // Restore index
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

            // Restore old foreign key
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
