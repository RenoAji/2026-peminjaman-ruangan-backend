using PeminjamanRuangan.Api.Models;

namespace PeminjamanRuangan.Api.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Seed Ruangan
        if (!context.Ruangan.Any())
        {
            var ruangan = new List<Ruangan>
            {
                new() { NamaRuangan = "R101", Lokasi = "Gedung A Lantai 1", Kapasitas = 30, IsAvailable = true },
                new() { NamaRuangan = "R102", Lokasi = "Gedung A Lantai 1", Kapasitas = 40, IsAvailable = true },
                new() { NamaRuangan = "R201", Lokasi = "Gedung A Lantai 2", Kapasitas = 50, IsAvailable = true },
                new() { NamaRuangan = "R202", Lokasi = "Gedung A Lantai 2", Kapasitas = 35, IsAvailable = true },
                new() { NamaRuangan = "R301", Lokasi = "Gedung B Lantai 3", Kapasitas = 60, IsAvailable = true },
                new() { NamaRuangan = "Aula Utama", Lokasi = "Gedung C Lantai 1", Kapasitas = 200, IsAvailable = true },
                new() { NamaRuangan = "Lab Komputer 1", Lokasi = "Gedung D Lantai 1", Kapasitas = 40, IsAvailable = true },
                new() { NamaRuangan = "Lab Komputer 2", Lokasi = "Gedung D Lantai 2", Kapasitas = 40, IsAvailable = true },
                new() { NamaRuangan = "Ruang Seminar", Lokasi = "Gedung B Lantai 1", Kapasitas = 100, IsAvailable = true },
                new() { NamaRuangan = "Ruang Rapat", Lokasi = "Gedung A Lantai 3", Kapasitas = 20, IsAvailable = true },
            };

            context.Ruangan.AddRange(ruangan);
            context.SaveChanges();
        }

        // Seed Peminjaman
        if (!context.Peminjaman.Any())
        {
            var peminjaman = new List<Peminjaman>
            {
                new()
                {
                    NamaPeminjam = "Dr. Budi Santoso",
                    RuanganId = 1,
                    TanggalPinjam = new DateTime(2026, 2, 10, 8, 0, 0, DateTimeKind.Utc),
                    TanggalSelesai = new DateTime(2026, 2, 10, 10, 0, 0, DateTimeKind.Utc),
                    Keperluan = "Kuliah Pengantar Basis Data",
                    Status = "Approved"
                },
                new()
                {
                    NamaPeminjam = "Prof. Siti Aminah",
                    RuanganId = 3,
                    TanggalPinjam = new DateTime(2026, 2, 11, 13, 0, 0, DateTimeKind.Utc),
                    TanggalSelesai = new DateTime(2026, 2, 11, 15, 0, 0, DateTimeKind.Utc),
                    Keperluan = "Seminar Nasional Teknologi",
                    Status = "Approved"
                },
                new()
                {
                    NamaPeminjam = "Andi Prasetyo",
                    RuanganId = 6,
                    TanggalPinjam = new DateTime(2026, 2, 15, 9, 0, 0, DateTimeKind.Utc),
                    TanggalSelesai = new DateTime(2026, 2, 15, 17, 0, 0, DateTimeKind.Utc),
                    Keperluan = "Acara Dies Natalis",
                    Status = "Pending"
                },
                new()
                {
                    NamaPeminjam = "Rina Wulandari",
                    RuanganId = 7,
                    TanggalPinjam = new DateTime(2026, 2, 12, 10, 0, 0, DateTimeKind.Utc),
                    TanggalSelesai = new DateTime(2026, 2, 12, 12, 0, 0, DateTimeKind.Utc),
                    Keperluan = "Praktikum Pemrograman Web",
                    Status = "Approved"
                },
                new()
                {
                    NamaPeminjam = "Hendra Gunawan",
                    RuanganId = 10,
                    TanggalPinjam = new DateTime(2026, 2, 13, 14, 0, 0, DateTimeKind.Utc),
                    TanggalSelesai = new DateTime(2026, 2, 13, 16, 0, 0, DateTimeKind.Utc),
                    Keperluan = "Rapat Koordinasi Dosen",
                    Status = "Rejected"
                },
            };

            context.Peminjaman.AddRange(peminjaman);
            context.SaveChanges();
        }
    }
}
