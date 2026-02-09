# Changelog

Semua perubahan pada proyek ini akan didokumentasikan di file ini.

## [0.1.0] - 2026-02-09

### Added

- Inisialisasi struktur proyek ASP.NET Core (.NET 10)
- Dockerfile untuk containerization aplikasi
- Model `Ruangan` dan `Peminjaman` dengan relasi one-to-many
- `AppDbContext` dengan konfigurasi Entity Framework Core (PostgreSQL)
- Database migration `InitialCreate` untuk tabel `ruangan` dan `peminjaman`
- Seeder data awal (10 ruangan, 5 peminjaman)
- Konfigurasi connection string PostgreSQL di `appsettings.json`
- Auto-migrate dan seeding saat startup di `Program.cs`

### Fixed

- Hapus folder `obj` dari Git tracking
