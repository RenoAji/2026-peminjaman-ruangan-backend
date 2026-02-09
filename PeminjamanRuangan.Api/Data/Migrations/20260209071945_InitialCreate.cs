using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PeminjamanRuangan.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ruangan",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nama_ruangan = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    lokasi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    kapasitas = table.Column<int>(type: "integer", nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ruangan", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "peminjaman",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nama_peminjam = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ruangan_id = table.Column<int>(type: "integer", nullable: false),
                    tanggal_pinjam = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tanggal_selesai = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    keperluan = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peminjaman", x => x.id);
                    table.ForeignKey(
                        name: "FK_peminjaman_ruangan_ruangan_id",
                        column: x => x.ruangan_id,
                        principalTable: "ruangan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_peminjaman_ruangan_id",
                table: "peminjaman",
                column: "ruangan_id");

            migrationBuilder.CreateIndex(
                name: "IX_ruangan_nama_ruangan",
                table: "ruangan",
                column: "nama_ruangan",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "peminjaman");

            migrationBuilder.DropTable(
                name: "ruangan");
        }
    }
}
