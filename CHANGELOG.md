# Changelog

Semua perubahan pada proyek ini akan didokumentasikan di file ini.

## [0.4.0] - 2026-02-11

### Added

- Endpoint availability ruangan:
  - GET `/api/ruangan/{id}/availability` — Return booked periods dan available periods dalam range `startDate` dan `endDate`
- DTOs untuk availability:
  - `BookedPeriod`, `AvailablePeriod`, `RuanganAvailabilityResponse`
- Insomnia collection (`insomnia_collection.json`) dengan request availability

### Changed

- Validasi query params `startDate` dan `endDate` untuk availability endpoint

## [0.3.0] - 2026-02-09

### Added

- CRUD endpoints untuk table `Peminjaman`:
  - GET `/api/peminjaman` — Retrieve semua peminjaman (sorted by newest, includes ruangan name)
  - GET `/api/peminjaman/{id}` — Retrieve peminjaman by ID
  - POST `/api/peminjaman` — Create peminjaman baru dengan validasi ruangan, tanggal, dan time conflict check
  - PUT `/api/peminjaman/{id}` — Update peminjaman dengan partial update support dan status validation
  - DELETE `/api/peminjaman/{id}` — Delete peminjaman
- `PeminjamanController` dengan validasi:
  - Ruangan existence check
  - Date validation (tanggal selesai harus setelah tanggal pinjam)
  - Time conflict detection (mencegah double booking pada ruangan yang sama, mengabaikan status "Rejected")
  - Status validation (Pending, Approved, Rejected, Done)
- DTOs: `CreatePeminjamanRequest`, `UpdatePeminjamanRequest`, `PeminjamanResponse`
- Migration `AddPeminjamanCrud` untuk mengubah column types ke `timestamp without time zone`
- Insomnia collection (`insomnia_peminjaman.json`) untuk testing API

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
