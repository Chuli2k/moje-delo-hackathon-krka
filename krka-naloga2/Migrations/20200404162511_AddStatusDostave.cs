using Microsoft.EntityFrameworkCore.Migrations;

namespace krka_naloga2.Migrations
{
    public partial class AddStatusDostave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Dostave",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Dostave");
        }
    }
}
