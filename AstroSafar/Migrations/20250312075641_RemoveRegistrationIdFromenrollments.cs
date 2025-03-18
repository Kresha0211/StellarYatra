using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroSafar.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRegistrationIdFromenrollments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop Foreign Key Constraint if it exists
            migrationBuilder.DropForeignKey(
                name: "FK_enrollments_Registrations_RegistrationId",
                table: "enrollments");

            // Drop the RegistrationId column safely
            migrationBuilder.DropColumn(
                name: "RegistrationId",
                table: "enrollments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_enrollments_Registrations_RegistrationId",
                table: "enrollments");

           
        }
    }
}
