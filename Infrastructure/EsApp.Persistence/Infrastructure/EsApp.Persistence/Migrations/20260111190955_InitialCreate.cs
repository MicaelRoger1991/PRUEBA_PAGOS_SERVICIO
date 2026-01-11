using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EsApp.Persistence.Infrastructure.EsApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usersMaster",
                columns: table => new
                {
                    usersMasterId = table.Column<Guid>(type: "uuid", nullable: false),
                    firstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    lastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    userRegistration = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    role = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    stateRecord = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    creationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    modificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usersMaster", x => x.usersMasterId);
                });

            migrationBuilder.CreateTable(
                name: "usersSessionsMaster",
                columns: table => new
                {
                    usersSessionsMasterId = table.Column<Guid>(type: "uuid", nullable: false),
                    usersMasterId = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    refreshToken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    dateGenerate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dateExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    amountToken = table.Column<int>(type: "integer", nullable: false),
                    creationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    modificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usersSessionsMaster", x => x.usersSessionsMasterId);
                    table.ForeignKey(
                        name: "FK_usersSessionsMaster_usersMaster_usersMasterId",
                        column: x => x.usersMasterId,
                        principalTable: "usersMaster",
                        principalColumn: "usersMasterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "usersMaster",
                columns: new[] { "usersMasterId", "creationDate", "firstName", "lastName", "modificationDate", "password", "role", "stateRecord", "userRegistration" },
                values: new object[,]
                {
                    { new Guid("0c7c13f1-05cd-4d5a-9ad1-8d2b8f2e1b51"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Usuario", "Caja", null, "123abc", "CAJA", "A", "caja" },
                    { new Guid("9c4932e4-1c6d-4b5f-8d7e-7f0c4b13a9b2"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Usuario", "Plataforma", null, "123abc", "PLATAFORMA", "A", "plataforma" },
                    { new Guid("a7d3a1f4-0b2c-4f91-8a86-4db5e7c7d4e6"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Usuario", "Gerente", null, "123abc", "GERENTE", "A", "gerente" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_usersSessionsMaster_usersMasterId",
                table: "usersSessionsMaster",
                column: "usersMasterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usersSessionsMaster");

            migrationBuilder.DropTable(
                name: "usersMaster");
        }
    }
}
