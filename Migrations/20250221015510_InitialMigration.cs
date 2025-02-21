using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClusterWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Residentes_ResidenteId",
                table: "Pagos");

            migrationBuilder.AlterColumn<int>(
                name: "ResidenteId",
                table: "Pagos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Residentes_ResidenteId",
                table: "Pagos",
                column: "ResidenteId",
                principalTable: "Residentes",
                principalColumn: "ResidenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Residentes_ResidenteId",
                table: "Pagos");

            migrationBuilder.AlterColumn<int>(
                name: "ResidenteId",
                table: "Pagos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Residentes_ResidenteId",
                table: "Pagos",
                column: "ResidenteId",
                principalTable: "Residentes",
                principalColumn: "ResidenteId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
