using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanBee.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    application_no = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    loan_amount = table.Column<int>(type: "int", nullable: false),
                    loan_tenor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loan_purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    application_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    application_status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.application_no);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Account_no = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Account_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Relationship_since = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Account_no);
                });

            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    Business_tin_no = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Business_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Business_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Office_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Office_zip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Business_landline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Business_mobile_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Business_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Business_website = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.Business_tin_no);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Owner_tin_no = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Owner_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Owner_gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Owner_birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Owner_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Owner_place_of_birth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Owner_citizenship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Owner_civil_status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Owner_mobile_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Owner_landline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Owner_email_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Owner_education = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Owner_tin_no);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropTable(
                name: "Owners");
        }
    }
}
