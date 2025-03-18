using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroSafar.Migrations
{
    /// <inheritdoc />
    public partial class remove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Drop foreign key constraint if it exists
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Registrations_RegistrationId",
                table: "Enrollments");

            // Step 2: Drop index on RegistrationId if it exists
            migrationBuilder.DropIndex(
                name: "IX_Enrollments_RegistrationId",
                table: "Enrollments");

            // Step 3: Remove RegistrationId column from Enrollments table
            migrationBuilder.DropColumn(
                name: "RegistrationId",
                table: "Enrollments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Re-add RegistrationId column in case of rollback
            migrationBuilder.AddColumn<int>(
                name: "RegistrationId",
                table: "Enrollments",
                type: "int",
                nullable: true);

            // Recreate index on RegistrationId
            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_RegistrationId",
                table: "Enrollments",
                column: "RegistrationId");

            // Recreate foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Registrations_RegistrationId",
                table: "Enrollments",
                column: "RegistrationId",
                principalTable: "Registrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
