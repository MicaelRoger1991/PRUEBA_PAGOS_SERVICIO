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
                name: "currency",
                columns: table => new
                {
                    currencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    currency = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    shortName = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    stateRecord = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false, defaultValue: "A"),
                    creationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    modificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currency", x => x.currencyId);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    customerId = table.Column<Guid>(type: "uuid", nullable: false),
                    firstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    lastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    documentNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    stateRecord = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false, defaultValue: "A"),
                    creationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    modificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.customerId);
                });

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
                    stateRecord = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false, defaultValue: "A"),
                    creationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    modificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usersMaster", x => x.usersMasterId);
                });

            migrationBuilder.CreateTable(
                name: "serviceProvider",
                columns: table => new
                {
                    serviceProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    service = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    currencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    stateRecord = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false, defaultValue: "A"),
                    creationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    modificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serviceProvider", x => x.serviceProviderId);
                    table.ForeignKey(
                        name: "FK_serviceProvider_currency_currencyId",
                        column: x => x.currencyId,
                        principalTable: "currency",
                        principalColumn: "currencyId",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "paymentsServices",
                columns: table => new
                {
                    paymentsId = table.Column<Guid>(type: "uuid", nullable: false),
                    usersMasterId = table.Column<Guid>(type: "uuid", nullable: false),
                    customerId = table.Column<Guid>(type: "uuid", nullable: false),
                    serviceProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    stateRecord = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false, defaultValue: "A"),
                    creationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    modificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentsServices", x => x.paymentsId);
                    table.ForeignKey(
                        name: "FK_paymentsServices_customers_customerId",
                        column: x => x.customerId,
                        principalTable: "customers",
                        principalColumn: "customerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_paymentsServices_serviceProvider_serviceProviderId",
                        column: x => x.serviceProviderId,
                        principalTable: "serviceProvider",
                        principalColumn: "serviceProviderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_paymentsServices_usersMaster_usersMasterId",
                        column: x => x.usersMasterId,
                        principalTable: "usersMaster",
                        principalColumn: "usersMasterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "currency",
                columns: new[] { "currencyId", "creationDate", "currency", "modificationDate", "shortName", "stateRecord" },
                values: new object[,]
                {
                    { new Guid("5d8f6a1d-9c2a-4b2f-9d2c-3189d9a21c4b"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dolares", null, "USD", "A" },
                    { new Guid("d29fba7e-1a2b-4fe7-8c2f-813a4e5ec5a0"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bolivianos", null, "BOB", "A" }
                });

            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "customerId", "creationDate", "documentNumber", "firstName", "lastName", "modificationDate", "stateRecord" },
                values: new object[,]
                {
                    { new Guid("81b7d3a3-9e4b-4f5c-9d64-3d1d8f9b1a6c"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "12345678", "Carlos", "Lopez", null, "A" },
                    { new Guid("f3dbb0f0-9c42-4fa1-8e4d-1437254a9d0f"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "87654321", "Maria", "Fernandez", null, "A" }
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

            migrationBuilder.InsertData(
                table: "serviceProvider",
                columns: new[] { "serviceProviderId", "creationDate", "currencyId", "modificationDate", "service", "stateRecord" },
                values: new object[,]
                {
                    { new Guid("10c5e8f7-7c58-4d8e-9c4d-8c4d5d7a9d1f"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d29fba7e-1a2b-4fe7-8c2f-813a4e5ec5a0"), null, "Telecomunicaciones", "A" },
                    { new Guid("6e3dce1a-148a-4bf8-9b08-4b4a1e4cbe5a"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d29fba7e-1a2b-4fe7-8c2f-813a4e5ec5a0"), null, "Electricidad", "A" },
                    { new Guid("b6b2d52c-1b8f-4ec8-9b38-0de5f5cc6e92"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d29fba7e-1a2b-4fe7-8c2f-813a4e5ec5a0"), null, "Agua", "A" },
                    { new Guid("f7f18c0a-2f04-4c8f-9d17-9f0f8c3e7e2a"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("5d8f6a1d-9c2a-4b2f-9d2c-3189d9a21c4b"), null, "Starlink", "A" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_paymentsServices_customerId",
                table: "paymentsServices",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_paymentsServices_serviceProviderId",
                table: "paymentsServices",
                column: "serviceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_paymentsServices_usersMasterId",
                table: "paymentsServices",
                column: "usersMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_serviceProvider_currencyId",
                table: "serviceProvider",
                column: "currencyId");

            migrationBuilder.CreateIndex(
                name: "IX_usersSessionsMaster_usersMasterId",
                table: "usersSessionsMaster",
                column: "usersMasterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "paymentsServices");

            migrationBuilder.DropTable(
                name: "usersSessionsMaster");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "serviceProvider");

            migrationBuilder.DropTable(
                name: "usersMaster");

            migrationBuilder.DropTable(
                name: "currency");
        }
    }
}
