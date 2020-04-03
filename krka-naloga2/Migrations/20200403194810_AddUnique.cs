using Microsoft.EntityFrameworkCore.Migrations;

namespace krka_naloga2.Migrations
{
    public partial class AddUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TockeSkladisc_SkladisceId",
                table: "TockeSkladisc");

            migrationBuilder.AlterColumn<string>(
                name: "Sifra",
                table: "TockeSkladisc",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sifra",
                table: "Skladisca",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sifra",
                table: "Podjetja",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TockeSkladisc_SkladisceId_Sifra",
                table: "TockeSkladisc",
                columns: new[] { "SkladisceId", "Sifra" },
                unique: true,
                filter: "[Sifra] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Skladisca_Sifra",
                table: "Skladisca",
                column: "Sifra",
                unique: true,
                filter: "[Sifra] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Podjetja_Sifra",
                table: "Podjetja",
                column: "Sifra",
                unique: true,
                filter: "[Sifra] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TockeSkladisc_SkladisceId_Sifra",
                table: "TockeSkladisc");

            migrationBuilder.DropIndex(
                name: "IX_Skladisca_Sifra",
                table: "Skladisca");

            migrationBuilder.DropIndex(
                name: "IX_Podjetja_Sifra",
                table: "Podjetja");

            migrationBuilder.AlterColumn<string>(
                name: "Sifra",
                table: "TockeSkladisc",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sifra",
                table: "Skladisca",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sifra",
                table: "Podjetja",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TockeSkladisc_SkladisceId",
                table: "TockeSkladisc",
                column: "SkladisceId");
        }
    }
}
