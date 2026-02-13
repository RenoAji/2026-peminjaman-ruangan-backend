# Peminjaman Ruangan - Backend API

Backend API untuk sistem peminjaman ruangan menggunakan ASP.NET Core 10.0 dan PostgreSQL.

## 🐳 Docker

Untuk menjalankan dengan Docker Compose (backend + frontend + database), lihat [repository infrastructure](https://github.com/RenoAji/2026-peminjaman-ruangan-infrastructure).

## 🔧 Persyaratan Sistem

- **.NET SDK 10.0** atau lebih baru ([Download](https://dotnet.microsoft.com/download))
- **PostgreSQL 14** atau lebih baru ([Download](https://www.postgresql.org/download/))
- **Git**

## 📥 Quick Start

### 1. Clone dan Install Dependencies

```bash
git clone https://github.com/RenoAji/2026-peminjaman-ruangan-backend.git
cd 2026-peminjaman-ruangan-backend/PeminjamanRuangan.Api
dotnet restore
```

### 2. Konfigurasi Environment Variables

Copy `.env.example` ke `.env` dan sesuaikan jika diperlukan:

```bash
cp .env.example .env
```

Edit `.env` dengan kredensial PostgreSQL Anda:

```env
DB_HOST=localhost
DB_PORT=5432
DB_NAME=peminjaman_ruangan
DB_USER=postgres
DB_PASSWORD=your_password_here
```

### 3. Setup Database

```bash
# Login ke PostgreSQL dan buat database
psql -U postgres
CREATE DATABASE peminjaman_ruangan;
\q
```

### 4. Jalankan Aplikasi

```bash
# Development mode dengan hot reload
dotnet watch run

# Atau mode normal
dotnet run
```

**API akan berjalan di:**

- HTTP: `http://localhost:5000`

> **Note:** Migrasi database dan seeding data berjalan otomatis saat aplikasi pertama kali dijalankan.

## 📚 API Documentation

### Scalar UI (Interactive)

Akses dokumentasi interaktif di: **`http://localhost:5000`**

Fitur Scalar UI:

- Browse semua endpoints
- Test API langsung dari browser
- Lihat request/response schema

### OpenAPI Specification

OpenAPI spec tersedia di: **`http://localhost:5000/openapi/v1.json`**

## 🧪 Endpoints

### Ruangan Management

| Method | Endpoint                         | Deskripsi                                    |
| ------ | -------------------------------- | -------------------------------------------- |
| GET    | `/api/ruangan`                   | List semua ruangan                           |
| GET    | `/api/ruangan/{id}`              | Detail ruangan                               |
| POST   | `/api/ruangan`                   | Tambah ruangan                               |
| PUT    | `/api/ruangan/{id}`              | Update ruangan                               |
| DELETE | `/api/ruangan/{id}`              | Hapus ruangan                                |
| GET    | `/api/ruangan/available`         | Cek ketersediaan (query: startDate, endDate) |
| GET    | `/api/ruangan/{id}/availability` | Detail availability ruangan                  |

### Peminjaman Management

| Method | Endpoint               | Deskripsi             |
| ------ | ---------------------- | --------------------- |
| GET    | `/api/peminjaman`      | List semua peminjaman |
| GET    | `/api/peminjaman/{id}` | Detail peminjaman     |
| POST   | `/api/peminjaman`      | Buat peminjaman       |
| PUT    | `/api/peminjaman/{id}` | Update peminjaman     |
| DELETE | `/api/peminjaman/{id}` | Hapus peminjaman      |

## 🔄 Development Workflow

1. Jalankan dari folder `PeminjamanRuangan.Api`: `dotnet watch run`
2. Edit file `.cs`, simpan → aplikasi auto-restart
3. Test di Scalar UI: `http://localhost:5000`
4. Commit changes

**Note:** Jangan commit file `.env` (sudah di `.gitignore`). Hanya commit `.env.example`.

## 🐛 Troubleshooting

**Database connection failed:**

```bash
sudo systemctl start postgresql  # Linux
brew services start postgresql   # Mac
```

**Port already in use:**

```bash
lsof -i :5000  # Cek process yang pakai port
# Atau ubah port di Properties/launchSettings.json
```

**.env file not found:**

Pastikan `.env` ada di root folder backend:

```bash
cp .env.example .env
```

## 📝 Changelog

Lihat [CHANGELOG.md](CHANGELOG.md) untuk riwayat perubahan.
