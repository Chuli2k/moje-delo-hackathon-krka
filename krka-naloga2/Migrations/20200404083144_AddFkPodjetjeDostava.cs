using Microsoft.EntityFrameworkCore.Migrations;

namespace krka_naloga2.Migrations
{
    public partial class AddFkPodjetjeDostava : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PodjetjeId",
                table: "Dostave",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Dostave_PodjetjeId",
                table: "Dostave",
                column: "PodjetjeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dostave_Podjetja_PodjetjeId",
                table: "Dostave",
                column: "PodjetjeId",
                principalTable: "Podjetja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dostave_Podjetja_PodjetjeId",
                table: "Dostave");

            migrationBuilder.DropIndex(
                name: "IX_Dostave_PodjetjeId",
                table: "Dostave");

            migrationBuilder.DropColumn(
                name: "PodjetjeId",
                table: "Dostave");
        }
    }
}
