using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignatureWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class initMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "constCheckSignatures",
                columns: table => new
                {
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SignatureImgeBase64 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConstCheckSeq = table.Column<int>(type: "int", nullable: false),
                    EngSeq = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_constCheckSignatures", x => x.Seq);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "constCheckSignatures");
        }
    }
}
