using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CagriMerkezi2.Migrations
{
    /// <inheritdoc />
    public partial class calisanTabloEkle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calisanlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TC = table.Column<int>(type: "int", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelNo = table.Column<int>(type: "int", nullable: false),
                    BirimId = table.Column<int>(type: "int", nullable: false),
                    DepId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calisanlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calisanlar_Birimler_BirimId",
                        column: x => x.BirimId,
                        principalTable: "Birimler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calisanlar_Departmanlar_DepId",
                        column: x => x.DepId,
                        principalTable: "Departmanlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calisanlar_BirimId",
                table: "Calisanlar",
                column: "BirimId");

            migrationBuilder.CreateIndex(
                name: "IX_Calisanlar_DepId",
                table: "Calisanlar",
                column: "DepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calisanlar");
        }
    }
}
