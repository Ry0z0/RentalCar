using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalCar.Entity.Migrations
{
    public partial class changeSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "reference");

            migrationBuilder.RenameTable(
                name: "Model",
                schema: "instance",
                newName: "Model",
                newSchema: "reference");

            migrationBuilder.RenameTable(
                name: "Brand",
                schema: "instance",
                newName: "Brand",
                newSchema: "reference");

            migrationBuilder.RenameTable(
                name: "Admin",
                schema: "instance",
                newName: "Admin",
                newSchema: "reference");

            migrationBuilder.RenameTable(
                name: "AddressWard",
                schema: "instance",
                newName: "AddressWard",
                newSchema: "reference");

            migrationBuilder.RenameTable(
                name: "AddressDistrict",
                schema: "instance",
                newName: "AddressDistrict",
                newSchema: "reference");

            migrationBuilder.RenameTable(
                name: "AddressCity",
                schema: "instance",
                newName: "AddressCity",
                newSchema: "reference");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Model",
                schema: "reference",
                newName: "Model",
                newSchema: "instance");

            migrationBuilder.RenameTable(
                name: "Brand",
                schema: "reference",
                newName: "Brand",
                newSchema: "instance");

            migrationBuilder.RenameTable(
                name: "Admin",
                schema: "reference",
                newName: "Admin",
                newSchema: "instance");

            migrationBuilder.RenameTable(
                name: "AddressWard",
                schema: "reference",
                newName: "AddressWard",
                newSchema: "instance");

            migrationBuilder.RenameTable(
                name: "AddressDistrict",
                schema: "reference",
                newName: "AddressDistrict",
                newSchema: "instance");

            migrationBuilder.RenameTable(
                name: "AddressCity",
                schema: "reference",
                newName: "AddressCity",
                newSchema: "instance");
        }
    }
}
