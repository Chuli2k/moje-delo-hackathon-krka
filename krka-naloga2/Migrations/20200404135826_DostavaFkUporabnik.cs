using Microsoft.EntityFrameworkCore.Migrations;

namespace krka_naloga2.Migrations
{
    public partial class DostavaFkUporabnik : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UporabnikId",
                table: "Dostave",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PodjetjeId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dostave_UporabnikId",
                table: "Dostave",
                column: "UporabnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dostave_AspNetUsers_UporabnikId",
                table: "Dostave",
                column: "UporabnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dostave_AspNetUsers_UporabnikId",
                table: "Dostave");

            migrationBuilder.DropIndex(
                name: "IX_Dostave_UporabnikId",
                table: "Dostave");

            migrationBuilder.DropColumn(
                name: "UporabnikId",
                table: "Dostave");

            migrationBuilder.AlterColumn<int>(
                name: "PodjetjeId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
