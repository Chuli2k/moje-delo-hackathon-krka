using Microsoft.EntityFrameworkCore.Migrations;

namespace krka_naloga2.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Podjetja",
                columns: new[] { "Id", "Naziv", "Sifra" },
                values: new object[,]
                {
                    { 1, "Krka", "10001" },
                    { 2, "Bayer", "10002" }
                });

            migrationBuilder.InsertData(
                table: "Skladisca",
                columns: new[] { "Id", "Sifra" },
                values: new object[,]
                {
                    { 1, "1" },
                    { 2, "2" },
                    { 3, "3" }
                });

            migrationBuilder.InsertData(
                table: "TockeSkladisc",
                columns: new[] { "Id", "Sifra", "SkladisceId" },
                values: new object[,]
                {
                    { 1, "1", 1 },
                    { 2, "2", 1 },
                    { 3, "3", 1 },
                    { 4, "4", 1 },
                    { 5, "5", 1 },
                    { 6, "1", 2 },
                    { 7, "2", 2 },
                    { 8, "3", 2 },
                    { 9, "4", 2 },
                    { 10, "5", 2 },
                    { 11, "1", 3 },
                    { 12, "2", 3 },
                    { 13, "3", 3 },
                    { 14, "4", 3 },
                    { 15, "5", 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Podjetja",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Podjetja",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "TockeSkladisc",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Skladisca",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Skladisca",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Skladisca",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
