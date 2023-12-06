using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CagriMerkezi2.Migrations
{
    /// <inheritdoc />
    public partial class basvurukoduOlusturma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BasvuruKodu",
                table: "Sikayetler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BasvuruKodu",
                table: "CagriMerkezis",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasvuruKodu",
                table: "Sikayetler");

            migrationBuilder.DropColumn(
                name: "BasvuruKodu",
                table: "CagriMerkezis");
        }
    }
}
