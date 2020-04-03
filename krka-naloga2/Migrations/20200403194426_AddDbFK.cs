using Microsoft.EntityFrameworkCore.Migrations;

namespace krka_naloga2.Migrations
{
    public partial class AddDbFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkladisceId",
                table: "TockeSkladisc",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TockaSkladiscaId",
                table: "Dostave",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TockeSkladisc_SkladisceId",
                table: "TockeSkladisc",
                column: "SkladisceId");

            migrationBuilder.CreateIndex(
                name: "IX_Dostave_TockaSkladiscaId",
                table: "Dostave",
                column: "TockaSkladiscaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dostave_TockeSkladisc_TockaSkladiscaId",
                table: "Dostave",
                column: "TockaSkladiscaId",
                principalTable: "TockeSkladisc",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TockeSkladisc_Skladisca_SkladisceId",
                table: "TockeSkladisc",
                column: "SkladisceId",
                principalTable: "Skladisca",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dostave_TockeSkladisc_TockaSkladiscaId",
                table: "Dostave");

            migrationBuilder.DropForeignKey(
                name: "FK_TockeSkladisc_Skladisca_SkladisceId",
                table: "TockeSkladisc");

            migrationBuilder.DropIndex(
                name: "IX_TockeSkladisc_SkladisceId",
                table: "TockeSkladisc");

            migrationBuilder.DropIndex(
                name: "IX_Dostave_TockaSkladiscaId",
                table: "Dostave");

            migrationBuilder.DropColumn(
                name: "SkladisceId",
                table: "TockeSkladisc");

            migrationBuilder.DropColumn(
                name: "TockaSkladiscaId",
                table: "Dostave");
        }
    }
}
