using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace krka_naloga2.Migrations
{
    public partial class AddInitialDbModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dostave",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Sifra = table.Column<string>(nullable: true),
                    Termin = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dostave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Podjetja",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Naziv = table.Column<string>(nullable: true),
                    Sifra = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Podjetja", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skladisca",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Sifra = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skladisca", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TockeSkladisc",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Sifra = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TockeSkladisc", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dostave");

            migrationBuilder.DropTable(
                name: "Podjetja");

            migrationBuilder.DropTable(
                name: "Skladisca");

            migrationBuilder.DropTable(
                name: "TockeSkladisc");
        }
    }
}
