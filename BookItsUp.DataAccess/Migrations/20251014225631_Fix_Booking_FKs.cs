using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookItsUp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Booking_FKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bookings_customer_customer_id",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_bookings_customers_customer_id1",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_bookings_provider_provider_id",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_bookings_providers_provider_id1",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_bookings_service_service_id",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_bookings_services_service_id1",
                table: "bookings");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "provider");

            migrationBuilder.DropTable(
                name: "service");

            migrationBuilder.DropIndex(
                name: "ix_bookings_customer_id1",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "ix_bookings_provider_id1",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "ix_bookings_service_id1",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "customer_id1",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "provider_id1",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "service_id1",
                table: "bookings");

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_customers_customer_id",
                table: "bookings",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_providers_provider_id",
                table: "bookings",
                column: "provider_id",
                principalTable: "providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_services_service_id",
                table: "bookings",
                column: "service_id",
                principalTable: "services",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bookings_customers_customer_id",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_bookings_providers_provider_id",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_bookings_services_service_id",
                table: "bookings");

            migrationBuilder.AddColumn<Guid>(
                name: "customer_id1",
                table: "bookings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "provider_id1",
                table: "bookings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "service_id1",
                table: "bookings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "provider",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    time_zone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_provider", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "service",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    buffer_after_minutes = table.Column<int>(type: "integer", nullable: false),
                    buffer_before_minutes = table.Column<int>(type: "integer", nullable: false),
                    duration_minutes = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price_cents = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bookings_customer_id1",
                table: "bookings",
                column: "customer_id1");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_provider_id1",
                table: "bookings",
                column: "provider_id1");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_service_id1",
                table: "bookings",
                column: "service_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_customer_customer_id",
                table: "bookings",
                column: "customer_id",
                principalTable: "customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_customers_customer_id1",
                table: "bookings",
                column: "customer_id1",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_provider_provider_id",
                table: "bookings",
                column: "provider_id",
                principalTable: "provider",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_providers_provider_id1",
                table: "bookings",
                column: "provider_id1",
                principalTable: "providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_service_service_id",
                table: "bookings",
                column: "service_id",
                principalTable: "service",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_services_service_id1",
                table: "bookings",
                column: "service_id1",
                principalTable: "services",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
