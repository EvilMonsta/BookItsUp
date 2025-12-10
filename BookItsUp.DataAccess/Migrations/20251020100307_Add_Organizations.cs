using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookItsUp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Add_Organizations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "organizations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    time_zone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizations", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bookings_organization_id",
                table: "bookings",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "ix_organizations_name",
                table: "organizations",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_organizations_organization_id",
                table: "bookings",
                column: "organization_id",
                principalTable: "organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_customers_organizations_organization_id",
                table: "customers",
                column: "organization_id",
                principalTable: "organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_providers_organizations_organization_id",
                table: "providers",
                column: "organization_id",
                principalTable: "organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_services_organizations_organization_id",
                table: "services",
                column: "organization_id",
                principalTable: "organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bookings_organizations_organization_id",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_customers_organizations_organization_id",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "fk_providers_organizations_organization_id",
                table: "providers");

            migrationBuilder.DropForeignKey(
                name: "fk_services_organizations_organization_id",
                table: "services");

            migrationBuilder.DropTable(
                name: "organizations");

            migrationBuilder.DropIndex(
                name: "ix_bookings_organization_id",
                table: "bookings");
        }
    }
}
