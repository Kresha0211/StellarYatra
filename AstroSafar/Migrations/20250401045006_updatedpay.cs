using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroSafar.Migrations
{
    /// <inheritdoc />
    public partial class updatedpay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_enrollments_EnrollmentId",
                table: "payments");

            migrationBuilder.RenameColumn(
                name: "EnrollmentId",
                table: "payments",
                newName: "RegistrationId");

            migrationBuilder.RenameIndex(
                name: "IX_payments_EnrollmentId",
                table: "payments",
                newName: "IX_payments_RegistrationId");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_payments_CourseId",
                table: "payments",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_Registrations_RegistrationId",
                table: "payments",
                column: "RegistrationId",
                principalTable: "Registrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_payments_courseAdmins_CourseId",
                table: "payments",
                column: "CourseId",
                principalTable: "courseAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_Registrations_RegistrationId",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "FK_payments_courseAdmins_CourseId",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_payments_CourseId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "payments");

            migrationBuilder.RenameColumn(
                name: "RegistrationId",
                table: "payments",
                newName: "EnrollmentId");

            migrationBuilder.RenameIndex(
                name: "IX_payments_RegistrationId",
                table: "payments",
                newName: "IX_payments_EnrollmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_enrollments_EnrollmentId",
                table: "payments",
                column: "EnrollmentId",
                principalTable: "enrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
