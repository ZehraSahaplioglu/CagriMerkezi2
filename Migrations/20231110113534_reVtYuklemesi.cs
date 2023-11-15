using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CagriMerkezi2.Migrations
{
    /// <inheritdoc />
    public partial class reVtYuklemesi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Birimler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Birimler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departmanlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirimId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departmanlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departmanlar_Birimler_BirimId",
                        column: x => x.BirimId,
                        principalTable: "Birimler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Sikayetler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TC = table.Column<int>(type: "int", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelNo = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResimUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirimId = table.Column<int>(type: "int", nullable: false),
                    DepId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sikayetler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sikayetler_Birimler_BirimId",
                        column: x => x.BirimId,
                        principalTable: "Birimler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Sikayetler_Departmanlar_DepId",
                        column: x => x.DepId,
                        principalTable: "Departmanlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departmanlar_BirimId",
                table: "Departmanlar",
                column: "BirimId");

            migrationBuilder.CreateIndex(
                name: "IX_Sikayetler_BirimId",
                table: "Sikayetler",
                column: "BirimId");

            migrationBuilder.CreateIndex(
                name: "IX_Sikayetler_DepId",
                table: "Sikayetler",
                column: "DepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sikayetler");

            migrationBuilder.DropTable(
                name: "Departmanlar");

            migrationBuilder.DropTable(
                name: "Birimler");
        }
    }
}
