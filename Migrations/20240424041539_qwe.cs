using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DetApp3.Migrations
{
    public partial class qwe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patronym = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientID);
                });

            migrationBuilder.CreateTable(
                name: "Employers",
                columns: table => new
                {
                    EmployerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patronym = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Housenumber = table.Column<int>(type: "int", nullable: false),
                    Appartmentnumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employers", x => x.EmployerID);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Servicename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceID);
                });

            migrationBuilder.CreateTable(
                name: "Automobiles",
                columns: table => new
                {
                    AutomobileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gosnumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Automobiles", x => x.AutomobileID);
                    table.ForeignKey(
                        name: "FK_Automobiles_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientID");
                });

            migrationBuilder.CreateTable(
                name: "ProvidedServices",
                columns: table => new
                {
                    ProvidedServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    EmployerId = table.Column<int>(type: "int", nullable: true),
                    AutomobileId = table.Column<int>(type: "int", nullable: true),
                    dateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvidedServices", x => x.ProvidedServiceID);
                    table.ForeignKey(
                        name: "FK_ProvidedServices_Automobiles_AutomobileId",
                        column: x => x.AutomobileId,
                        principalTable: "Automobiles",
                        principalColumn: "AutomobileID");
                    table.ForeignKey(
                        name: "FK_ProvidedServices_Employers_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Employers",
                        principalColumn: "EmployerID");
                    table.ForeignKey(
                        name: "FK_ProvidedServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Automobiles_ClientId",
                table: "Automobiles",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProvidedServices_AutomobileId",
                table: "ProvidedServices",
                column: "AutomobileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProvidedServices_EmployerId",
                table: "ProvidedServices",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProvidedServices_ServiceId",
                table: "ProvidedServices",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProvidedServices");

            migrationBuilder.DropTable(
                name: "Automobiles");

            migrationBuilder.DropTable(
                name: "Employers");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
