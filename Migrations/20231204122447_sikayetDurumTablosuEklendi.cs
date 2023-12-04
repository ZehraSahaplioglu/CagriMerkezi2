using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CagriMerkezi2.Migrations
{
    /// <inheritdoc />
    public partial class sikayetDurumTablosuEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sikayetler_Departmanlar_DepId",
                table: "Sikayetler");

            migrationBuilder.AlterColumn<int>(
                name: "DepId",
                table: "Sikayetler",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DurumId",
                table: "Sikayetler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurumId",
                table: "CagriMerkezis",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SikayetDurums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SikayetDurums", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sikayetler_DurumId",
                table: "Sikayetler",
                column: "DurumId");

            migrationBuilder.CreateIndex(
                name: "IX_CagriMerkezis_DurumId",
                table: "CagriMerkezis",
                column: "DurumId");

            migrationBuilder.AddForeignKey(
                name: "FK_CagriMerkezis_SikayetDurums_DurumId",
                table: "CagriMerkezis",
                column: "DurumId",
                principalTable: "SikayetDurums",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sikayetler_Departmanlar_DepId",
                table: "Sikayetler",
                column: "DepId",
                principalTable: "Departmanlar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sikayetler_SikayetDurums_DurumId",
                table: "Sikayetler",
                column: "DurumId",
                principalTable: "SikayetDurums",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CagriMerkezis_SikayetDurums_DurumId",
                table: "CagriMerkezis");

            migrationBuilder.DropForeignKey(
                name: "FK_Sikayetler_Departmanlar_DepId",
                table: "Sikayetler");

            migrationBuilder.DropForeignKey(
                name: "FK_Sikayetler_SikayetDurums_DurumId",
                table: "Sikayetler");

            migrationBuilder.DropTable(
                name: "SikayetDurums");

            migrationBuilder.DropIndex(
                name: "IX_Sikayetler_DurumId",
                table: "Sikayetler");

            migrationBuilder.DropIndex(
                name: "IX_CagriMerkezis_DurumId",
                table: "CagriMerkezis");

            migrationBuilder.DropColumn(
                name: "DurumId",
                table: "Sikayetler");

            migrationBuilder.DropColumn(
                name: "DurumId",
                table: "CagriMerkezis");

            migrationBuilder.AlterColumn<int>(
                name: "DepId",
                table: "Sikayetler",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sikayetler_Departmanlar_DepId",
                table: "Sikayetler",
                column: "DepId",
                principalTable: "Departmanlar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
