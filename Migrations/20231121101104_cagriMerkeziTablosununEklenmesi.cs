using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CagriMerkezi2.Migrations
{
    /// <inheritdoc />
    public partial class cagriMerkeziTablosununEklenmesi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CagriMerkezis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TC = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelNo = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResimUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirimId = table.Column<int>(type: "int", nullable: true),
                    DepId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CagriMerkezis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CagriMerkezis_Birimler_BirimId",
                        column: x => x.BirimId,
                        principalTable: "Birimler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CagriMerkezis_Departmanlar_DepId",
                        column: x => x.DepId,
                        principalTable: "Departmanlar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CagriMerkezis_BirimId",
                table: "CagriMerkezis",
                column: "BirimId");

            migrationBuilder.CreateIndex(
                name: "IX_CagriMerkezis_DepId",
                table: "CagriMerkezis",
                column: "DepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CagriMerkezis");
        }
    }
}
