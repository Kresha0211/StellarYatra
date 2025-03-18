using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroSafar.Migrations
{
    /// <inheritdoc />
    public partial class TestRegT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // First add the column if it doesn't exist
            migrationBuilder.AddColumn<int>(
                name: "RegistrationId",
                table: "enrollments",
                nullable: true);

            // Then create the foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_enrollments_Registrations_RegistrationId",
                table: "enrollments",
                column: "RegistrationId",
                principalTable: "Registrations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddForeignKey(
                name: "FK_enrollments_Registrations_RegistrationId",
                table: "enrollments",
                column: "RegistrationId",
                principalTable: "Registrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
