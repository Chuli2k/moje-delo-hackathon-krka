using Microsoft.EntityFrameworkCore.Migrations;

namespace krka_naloga2.Migrations
{
    public partial class AddDodatnoSeedPodjetje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Podjetja",
                columns: new[] { "Id", "Naziv", "Sifra" },
                values: new object[] { 3, "LEK", "10010" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Podjetja",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
