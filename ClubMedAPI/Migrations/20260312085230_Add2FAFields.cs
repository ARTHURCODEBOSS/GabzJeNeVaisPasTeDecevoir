using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubMedAPI.Migrations
{
    /// <inheritdoc />
    public partial class Add2FAFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "a2f_method",
                table: "client",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "totp_secret",
                table: "client",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "a2f_method",
                table: "client");

            migrationBuilder.DropColumn(
                name: "totp_secret",
                table: "client");
        }
    }
}
