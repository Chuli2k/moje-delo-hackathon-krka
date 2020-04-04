using Microsoft.EntityFrameworkCore.Migrations;

namespace krka_naloga2.Migrations
{
    public partial class AddUQDostavaSifra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Sifra",
                table: "Dostave",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dostave_Sifra",
                table: "Dostave",
                column: "Sifra",
                unique: true,
                filter: "[Sifra] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Dostave_Sifra",
                table: "Dostave");

            migrationBuilder.AlterColumn<string>(
                name: "Sifra",
                table: "Dostave",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
