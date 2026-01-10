using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanBee.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Owner_tin_no",
                table: "Businesses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Owner_tin_no",
                table: "BankAccounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Account_no",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Business_tin_no",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_Owner_tin_no",
                table: "Businesses",
                column: "Owner_tin_no");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_Owner_tin_no",
                table: "BankAccounts",
                column: "Owner_tin_no");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Account_no",
                table: "Applications",
                column: "Account_no");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Business_tin_no",
                table: "Applications",
                column: "Business_tin_no");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_BankAccounts_Account_no",
                table: "Applications",
                column: "Account_no",
                principalTable: "BankAccounts",
                principalColumn: "Account_no",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Businesses_Business_tin_no",
                table: "Applications",
                column: "Business_tin_no",
                principalTable: "Businesses",
                principalColumn: "Business_tin_no",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Owners_Owner_tin_no",
                table: "BankAccounts",
                column: "Owner_tin_no",
                principalTable: "Owners",
                principalColumn: "Owner_tin_no",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Owners_Owner_tin_no",
                table: "Businesses",
                column: "Owner_tin_no",
                principalTable: "Owners",
                principalColumn: "Owner_tin_no",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_BankAccounts_Account_no",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Businesses_Business_tin_no",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Owners_Owner_tin_no",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Owners_Owner_tin_no",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_Owner_tin_no",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_Owner_tin_no",
                table: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Applications_Account_no",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_Business_tin_no",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Owner_tin_no",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Owner_tin_no",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "Account_no",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Business_tin_no",
                table: "Applications");
        }
    }
}
