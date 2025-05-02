using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroSafar.Migrations
{
    /// <inheritdoc />
    public partial class certificatesh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HigherSecondaryEnrollId",
                table: "Certificates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondaryEnrollId",
                table: "Certificates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_HigherSecondaryEnrollId",
                table: "Certificates",
                column: "HigherSecondaryEnrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_SecondaryEnrollId",
                table: "Certificates",
                column: "SecondaryEnrollId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_higherSecondaryEnrolls_HigherSecondaryEnrollId",
                table: "Certificates",
                column: "HigherSecondaryEnrollId",
                principalTable: "higherSecondaryEnrolls",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_secondaryEnrolls_SecondaryEnrollId",
                table: "Certificates",
                column: "SecondaryEnrollId",
                principalTable: "secondaryEnrolls",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_higherSecondaryEnrolls_HigherSecondaryEnrollId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_secondaryEnrolls_SecondaryEnrollId",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_HigherSecondaryEnrollId",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_SecondaryEnrollId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "HigherSecondaryEnrollId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "SecondaryEnrollId",
                table: "Certificates");
        }
    }
}
