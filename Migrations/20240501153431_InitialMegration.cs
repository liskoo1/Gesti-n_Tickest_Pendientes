using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gestión_Tickest_Pendientes.Migrations
{
    /// <inheritdoc />
    public partial class InitialMegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "empresas",
                columns: table => new
                {
                    Id_Empresa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CIF_Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion_Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono_Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email_Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empresas", x => x.Id_Empresa);
                });

            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    CIF_Cliente = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre_Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion_Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Codigo_Postal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono_Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email_Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Empresa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.CIF_Cliente);
                    table.ForeignKey(
                        name: "FK_clientes_empresas_Id_Empresa",
                        column: x => x.Id_Empresa,
                        principalTable: "empresas",
                        principalColumn: "Id_Empresa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    Id_Usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Empresa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.Id_Usuario);
                    table.ForeignKey(
                        name: "FK_usuarios_empresas_Id_Empresa",
                        column: x => x.Id_Empresa,
                        principalTable: "empresas",
                        principalColumn: "Id_Empresa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    Id_Albaran = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sala = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mesa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Firma_Cliente = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CIF_Cliente = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.Id_Albaran);
                    table.ForeignKey(
                        name: "FK_tickets_clientes_CIF_Cliente",
                        column: x => x.CIF_Cliente,
                        principalTable: "clientes",
                        principalColumn: "CIF_Cliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "empresas",
                columns: new[] { "Id_Empresa", "CIF_Empresa", "Direccion_Empresa", "Email_Empresa", "Nombre_Empresa", "Telefono_Empresa" },
                values: new object[] { 1, "A04052775", "Vnta del Pobre - Níjar - Almería", "ventadelpobre@gmail.com", "Venta del Pobre Gastro Bar", "950385544" });

            migrationBuilder.InsertData(
                table: "usuarios",
                columns: new[] { "Id_Usuario", "Id_Empresa", "Name", "Password" },
                values: new object[,]
                {
                    { 1, 1, "Admin", "e9cf3f0ecd469045446f50e517eb125a733482231ab0a93ee8c3492cde823116" },
                    { 2, 1, "Restaurante", "9faf3c3c97bd95f2b15b1a3904e9cbbbe730c1ecf60818a476e83e4aa7a3b595" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_clientes_Id_Empresa",
                table: "clientes",
                column: "Id_Empresa");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_CIF_Cliente",
                table: "tickets",
                column: "CIF_Cliente");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_Id_Empresa",
                table: "usuarios",
                column: "Id_Empresa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "clientes");

            migrationBuilder.DropTable(
                name: "empresas");
        }
    }
}
