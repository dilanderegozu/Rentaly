using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rentaly.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_VehicleType_VehicleTypeId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType");

            migrationBuilder.RenameTable(
                name: "VehicleType",
                newName: "VehicleTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleTypes",
                table: "VehicleTypes",
                column: "VehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_VehicleTypes_VehicleTypeId",
                table: "Cars",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "VehicleTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_VehicleTypes_VehicleTypeId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleTypes",
                table: "VehicleTypes");

            migrationBuilder.RenameTable(
                name: "VehicleTypes",
                newName: "VehicleType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType",
                column: "VehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_VehicleType_VehicleTypeId",
                table: "Cars",
                column: "VehicleTypeId",
                principalTable: "VehicleType",
                principalColumn: "VehicleTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
