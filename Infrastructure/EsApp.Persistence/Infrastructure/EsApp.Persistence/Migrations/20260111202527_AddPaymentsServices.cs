using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EsApp.Persistence.Infrastructure.EsApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentsServices : Migration
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
                    stateRecord = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
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
                    stateRecord = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    creationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    modificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.customerId);
                });

            migrationBuilder.CreateTable(
                name: "serviceProvider",
                columns: table => new
                {
                    serviceProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    service = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    currencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    stateRecord = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
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
                name: "paymentsServices",
                columns: table => new
                {
                    paymentsId = table.Column<Guid>(type: "uuid", nullable: false),
                    usersMasterId = table.Column<Guid>(type: "uuid", nullable: false),
                    customerId = table.Column<Guid>(type: "uuid", nullable: false),
                    serviceProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    stateRecord = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
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
                columns: new[] { "customerId", "creationDate", "firstName", "lastName", "modificationDate", "stateRecord" },
                values: new object[,]
                {
                    { new Guid("81b7d3a3-9e4b-4f5c-9d64-3d1d8f9b1a6c"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Carlos", "Lopez", null, "A" },
                    { new Guid("f3dbb0f0-9c42-4fa1-8e4d-1437254a9d0f"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Maria", "Fernandez", null, "A" }
                });

            migrationBuilder.InsertData(
                table: "serviceProvider",
                columns: new[] { "serviceProviderId", "creationDate", "currencyId", "modificationDate", "service", "stateRecord" },
                values: new object[,]
                {
                    { new Guid("10c5e8f7-7c58-4d8e-9c4d-8c4d5d7a9d1f"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d29fba7e-1a2b-4fe7-8c2f-813a4e5ec5a0"), null, "Telecomunicaciones", "A" },
                    { new Guid("6e3dce1a-148a-4bf8-9b08-4b4a1e4cbe5a"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d29fba7e-1a2b-4fe7-8c2f-813a4e5ec5a0"), null, "Electricidad", "A" },
                    { new Guid("b6b2d52c-1b8f-4ec8-9b38-0de5f5cc6e92"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d29fba7e-1a2b-4fe7-8c2f-813a4e5ec5a0"), null, "Agua", "A" }
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "paymentsServices");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "serviceProvider");

            migrationBuilder.DropTable(
                name: "currency");
        }
    }
}
