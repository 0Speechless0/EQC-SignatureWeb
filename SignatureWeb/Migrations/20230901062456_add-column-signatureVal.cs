using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignatureWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnsignatureVal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SignatureVal",
                table: "constCheckSignatures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignatureVal",
                table: "constCheckSignatures");
        }
    }
}
