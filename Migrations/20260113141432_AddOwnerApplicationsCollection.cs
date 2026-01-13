using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanBee.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerApplicationsCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Owner_tin_no",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Owner_tin_no",
                table: "Applications",
                column: "Owner_tin_no");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Owners_Owner_tin_no",
                table: "Applications",
                column: "Owner_tin_no",
                principalTable: "Owners",
                principalColumn: "Owner_tin_no");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Owners_Owner_tin_no",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_Owner_tin_no",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Owner_tin_no",
                table: "Applications");
        }
    }
}
