using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rentaly.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehicleTypeId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VehicleType",
                columns: table => new
                {
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleType", x => x.VehicleTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_VehicleTypeId",
                table: "Cars",
                column: "VehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_VehicleType_VehicleTypeId",
                table: "Cars",
                column: "VehicleTypeId",
                principalTable: "VehicleType",
                principalColumn: "VehicleTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_VehicleType_VehicleTypeId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "VehicleType");

            migrationBuilder.DropIndex(
                name: "IX_Cars_VehicleTypeId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "VehicleTypeId",
                table: "Cars");
        }
    }
}
