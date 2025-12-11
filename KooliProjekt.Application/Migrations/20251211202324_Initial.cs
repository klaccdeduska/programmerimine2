using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KooliProjekt.Application.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tootja = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mudel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Numbrimark = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperatsiooniTüübid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nimi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kirjeldus = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatsiooniTüübid", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Töötajad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nimi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Roll = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Töötajad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operatsioonid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutoId = table.Column<int>(type: "int", nullable: false),
                    TüüpId = table.Column<int>(type: "int", nullable: false),
                    TöötajaId = table.Column<int>(type: "int", nullable: false),
                    Kuupäev = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Staatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Maksumus = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operatsioonid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operatsioonid_Autos_AutoId",
                        column: x => x.AutoId,
                        principalTable: "Autos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operatsioonid_OperatsiooniTüübid_TüüpId",
                        column: x => x.TüüpId,
                        principalTable: "OperatsiooniTüübid",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operatsioonid_Töötajad_TöötajaId",
                        column: x => x.TöötajaId,
                        principalTable: "Töötajad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Autos",
                columns: new[] { "Id", "Mudel", "Numbrimark", "Tootja" },
                values: new object[,]
                {
                    { 1, "Corolla", "123ABC", "Toyota" },
                    { 2, "320", "555BMW", "BMW" }
                });

            migrationBuilder.InsertData(
                table: "OperatsiooniTüübid",
                columns: new[] { "Id", "Kirjeldus", "Nimi" },
                values: new object[,]
                {
                    { 1, "Mootoriõli vahetus", "Õlivahetus" },
                    { 2, "Rehvide vahetus komplektiga", "Rehvide vahetus" }
                });

            migrationBuilder.InsertData(
                table: "Töötajad",
                columns: new[] { "Id", "Email", "Nimi", "Roll" },
                values: new object[,]
                {
                    { 1, "admin@example.com", "Admin", "Administraator" },
                    { 2, "jaan@example.com", "Jaan", "Töötaja" }
                });

            migrationBuilder.InsertData(
                table: "Operatsioonid",
                columns: new[] { "Id", "AutoId", "Kuupäev", "Maksumus", "Staatus", "TöötajaId", "TüüpId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 12, 9, 22, 23, 24, 131, DateTimeKind.Local).AddTicks(4146), 35m, "Ootel", 2, 1 },
                    { 2, 2, new DateTime(2025, 12, 10, 22, 23, 24, 131, DateTimeKind.Local).AddTicks(4197), 50m, "Tegemisel", 2, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Autos_Numbrimark",
                table: "Autos",
                column: "Numbrimark",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operatsioonid_AutoId",
                table: "Operatsioonid",
                column: "AutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Operatsioonid_TöötajaId",
                table: "Operatsioonid",
                column: "TöötajaId");

            migrationBuilder.CreateIndex(
                name: "IX_Operatsioonid_TüüpId",
                table: "Operatsioonid",
                column: "TüüpId");

            migrationBuilder.CreateIndex(
                name: "IX_OperatsiooniTüübid_Nimi",
                table: "OperatsiooniTüübid",
                column: "Nimi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Töötajad_Email",
                table: "Töötajad",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operatsioonid");

            migrationBuilder.DropTable(
                name: "Autos");

            migrationBuilder.DropTable(
                name: "OperatsiooniTüübid");

            migrationBuilder.DropTable(
                name: "Töötajad");
        }
    }
}
