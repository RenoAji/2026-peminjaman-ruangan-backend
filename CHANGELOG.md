# Changelog

Semua perubahan pada proyek ini akan didokumentasikan di file ini.

## [0.2.0] - 2026-02-09

### Added

- CRUD endpoints untuk table `Ruangan`:
  - GET `/api/ruangan` — Retrieve semua ruangan (sorted by nama)
  - GET `/api/ruangan/{id}` — Retrieve ruangan by ID
  - POST `/api/ruangan` — Create ruangan baru dengan validasi duplikat nama
  - PUT `/api/ruangan/{id}` — Update ruangan dengan partial update support
  - DELETE `/api/ruangan/{id}` — Delete ruangan dengan cascade delete behavior
- Controllers folder dengan `RuanganController` menggunakan Dependency Injection
- DTOs: `CreateRuanganRequest`, `UpdateRuanganRequest`, `RuanganResponse`
- Migration `UpdateCascadeDelete` untuk mengubah foreign key behavior dari RESTRICT ke CASCADE

### Changed

- Update `AppDbContext` untuk cascade delete pada relasi Peminjaman-Ruangan

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

