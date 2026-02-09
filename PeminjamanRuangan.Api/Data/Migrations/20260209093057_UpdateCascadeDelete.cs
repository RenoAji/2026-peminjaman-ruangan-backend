using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeminjamanRuangan.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_peminjaman_ruangan_ruangan_id",
                table: "peminjaman");

            migrationBuilder.AddForeignKey(
                name: "FK_peminjaman_ruangan_ruangan_id",
                table: "peminjaman",
                column: "ruangan_id",
                principalTable: "ruangan",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_peminjaman_ruangan_ruangan_id",
                table: "peminjaman");

            migrationBuilder.AddForeignKey(
                name: "FK_peminjaman_ruangan_ruangan_id",
                table: "peminjaman",
                column: "ruangan_id",
                principalTable: "ruangan",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
