using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsApp.Persistence.Infrastructure.EsApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultValuesStateRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "stateRecord",
                table: "usersMaster",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "A",
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "stateRecord",
                table: "serviceProvider",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "A",
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "stateRecord",
                table: "paymentsServices",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "A",
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "stateRecord",
                table: "customers",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "A",
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);

            migrationBuilder.AddColumn<string>(
                name: "documentNumber",
                table: "customers",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "stateRecord",
                table: "currency",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "A",
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "customerId",
                keyValue: new Guid("81b7d3a3-9e4b-4f5c-9d64-3d1d8f9b1a6c"),
                column: "documentNumber",
                value: "12345678");

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "customerId",
                keyValue: new Guid("f3dbb0f0-9c42-4fa1-8e4d-1437254a9d0f"),
                column: "documentNumber",
                value: "87654321");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "documentNumber",
                table: "customers");

            migrationBuilder.AlterColumn<string>(
                name: "stateRecord",
                table: "usersMaster",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1,
                oldDefaultValue: "A");

            migrationBuilder.AlterColumn<string>(
                name: "stateRecord",
                table: "serviceProvider",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1,
                oldDefaultValue: "A");

            migrationBuilder.AlterColumn<string>(
                name: "stateRecord",
                table: "paymentsServices",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1,
                oldDefaultValue: "A");

            migrationBuilder.AlterColumn<string>(
                name: "stateRecord",
                table: "customers",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1,
                oldDefaultValue: "A");

            migrationBuilder.AlterColumn<string>(
                name: "stateRecord",
                table: "currency",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1,
                oldDefaultValue: "A");
        }
    }
}
