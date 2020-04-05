using Microsoft.EntityFrameworkCore.Migrations;

namespace krka_naloga2.Migrations
{
    public partial class AddSkladisceUporabnika : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkladisceId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SkladisceId",
                table: "AspNetUsers",
                column: "SkladisceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Skladisca_SkladisceId",
                table: "AspNetUsers",
                column: "SkladisceId",
                principalTable: "Skladisca",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Skladisca_SkladisceId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SkladisceId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SkladisceId",
                table: "AspNetUsers");
        }
    }
}
