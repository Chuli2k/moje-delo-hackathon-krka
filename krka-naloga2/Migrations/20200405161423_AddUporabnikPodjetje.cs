using Microsoft.EntityFrameworkCore.Migrations;

namespace krka_naloga2.Migrations
{
    public partial class AddUporabnikPodjetje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PodjetjeId",
                table: "AspNetUsers",
                column: "PodjetjeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Podjetja_PodjetjeId",
                table: "AspNetUsers",
                column: "PodjetjeId",
                principalTable: "Podjetja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Podjetja_PodjetjeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PodjetjeId",
                table: "AspNetUsers");
        }
    }
}
