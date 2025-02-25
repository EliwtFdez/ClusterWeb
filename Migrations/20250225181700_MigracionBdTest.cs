using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClusterWeb.Migrations
{
    /// <inheritdoc />
    public partial class MigracionBdTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Casas",
                columns: table => new
                {
                    IdCasa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroCasa = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Casas", x => x.IdCasa);
                });

            migrationBuilder.CreateTable(
                name: "Residentes",
                columns: table => new
                {
                    IdResidente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdCasa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residentes", x => x.IdResidente);
                    table.ForeignKey(
                        name: "FK_Residentes_Casas_IdCasa",
                        column: x => x.IdCasa,
                        principalTable: "Casas",
                        principalColumn: "IdCasa",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cuotas",
                columns: table => new
                {
                    IdCuota = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCuota = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Pendiente"),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IdCasa = table.Column<int>(type: "int", nullable: false),
                    IdResidente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuotas", x => x.IdCuota);
                    table.ForeignKey(
                        name: "FK_Cuotas_Casas_IdCasa",
                        column: x => x.IdCasa,
                        principalTable: "Casas",
                        principalColumn: "IdCasa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cuotas_Residentes_IdResidente",
                        column: x => x.IdResidente,
                        principalTable: "Residentes",
                        principalColumn: "IdResidente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCasa = table.Column<int>(type: "int", nullable: false),
                    IdResidente = table.Column<int>(type: "int", nullable: true),
                    IdCuota = table.Column<int>(type: "int", nullable: false),
                    MontoPagado = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    MetodoPago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.IdPago);
                    table.ForeignKey(
                        name: "FK_Pagos_Casas_IdCasa",
                        column: x => x.IdCasa,
                        principalTable: "Casas",
                        principalColumn: "IdCasa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_Cuotas_IdCuota",
                        column: x => x.IdCuota,
                        principalTable: "Cuotas",
                        principalColumn: "IdCuota",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_Residentes_IdResidente",
                        column: x => x.IdResidente,
                        principalTable: "Residentes",
                        principalColumn: "IdResidente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "idx_numero_casa",
                table: "Casas",
                column: "NumeroCasa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cuotas_IdCasa",
                table: "Cuotas",
                column: "IdCasa");

            migrationBuilder.CreateIndex(
                name: "IX_Cuotas_IdResidente",
                table: "Cuotas",
                column: "IdResidente");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdCasa",
                table: "Pagos",
                column: "IdCasa");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdCuota",
                table: "Pagos",
                column: "IdCuota");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdResidente",
                table: "Pagos",
                column: "IdResidente");

            migrationBuilder.CreateIndex(
                name: "idx_email",
                table: "Residentes",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Residentes_IdCasa",
                table: "Residentes",
                column: "IdCasa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Cuotas");

            migrationBuilder.DropTable(
                name: "Residentes");

            migrationBuilder.DropTable(
                name: "Casas");
        }
    }
}
