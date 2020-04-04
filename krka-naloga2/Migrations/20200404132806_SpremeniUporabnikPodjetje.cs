using Microsoft.EntityFrameworkCore.Migrations;

namespace krka_naloga2.Migrations
{
    public partial class SpremeniUporabnikPodjetje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Podjetje",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "PodjetjeId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PodjetjeId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Podjetje",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
