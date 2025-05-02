using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroSafar.Migrations
{
    /// <inheritdoc />
    public partial class newFkEnrollExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_enrollments_EnrollmentId",
                table: "ExamResults");

            migrationBuilder.AlterColumn<int>(
                name: "EnrollmentId",
                table: "ExamResults",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "HigherSecondaryEnrollId",
                table: "ExamResults",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondaryEnrollId",
                table: "ExamResults",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_HigherSecondaryEnrollId",
                table: "ExamResults",
                column: "HigherSecondaryEnrollId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_SecondaryEnrollId",
                table: "ExamResults",
                column: "SecondaryEnrollId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_enrollments_EnrollmentId",
                table: "ExamResults",
                column: "EnrollmentId",
                principalTable: "enrollments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_higherSecondaryEnrolls_HigherSecondaryEnrollId",
                table: "ExamResults",
                column: "HigherSecondaryEnrollId",
                principalTable: "higherSecondaryEnrolls",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_secondaryEnrolls_SecondaryEnrollId",
                table: "ExamResults",
                column: "SecondaryEnrollId",
                principalTable: "secondaryEnrolls",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_enrollments_EnrollmentId",
                table: "ExamResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_higherSecondaryEnrolls_HigherSecondaryEnrollId",
                table: "ExamResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_secondaryEnrolls_SecondaryEnrollId",
                table: "ExamResults");

            migrationBuilder.DropIndex(
                name: "IX_ExamResults_HigherSecondaryEnrollId",
                table: "ExamResults");

            migrationBuilder.DropIndex(
                name: "IX_ExamResults_SecondaryEnrollId",
                table: "ExamResults");

            migrationBuilder.DropColumn(
                name: "HigherSecondaryEnrollId",
                table: "ExamResults");

            migrationBuilder.DropColumn(
                name: "SecondaryEnrollId",
                table: "ExamResults");

            migrationBuilder.AlterColumn<int>(
                name: "EnrollmentId",
                table: "ExamResults",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_enrollments_EnrollmentId",
                table: "ExamResults",
                column: "EnrollmentId",
                principalTable: "enrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}