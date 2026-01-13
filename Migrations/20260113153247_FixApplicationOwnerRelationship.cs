using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanBee.Migrations
{
    /// <inheritdoc />
    public partial class FixApplicationOwnerRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Owners_Owner_tin_no",
                table: "Applications");

            migrationBuilder.AlterColumn<string>(
                name: "Owner_tin_no",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Owners_Owner_tin_no",
                table: "Applications",
                column: "Owner_tin_no",
                principalTable: "Owners",
                principalColumn: "Owner_tin_no",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Owners_Owner_tin_no",
                table: "Applications");

            migrationBuilder.AlterColumn<string>(
                name: "Owner_tin_no",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Owners_Owner_tin_no",
                table: "Applications",
                column: "Owner_tin_no",
                principalTable: "Owners",
                principalColumn: "Owner_tin_no");
        }
    }
}
