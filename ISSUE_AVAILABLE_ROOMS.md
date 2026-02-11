# Issue #2: [Feature] Get Available Rooms in Date Range

## Description

Implementasi endpoint untuk mendapatkan daftar ruangan yang tersedia (tidak ada booking atau conflict) dalam rentang tanggal tertentu. Endpoint ini berguna untuk user mencari ruangan yang bisa langsung di-book tanpa bentrok dengan peminjaman lain.

**Use Case:**

- User ingin booking ruangan untuk event tanggal 10-12 Maret, perlu tahu ruangan mana saja yang available
- Admin ingin melihat overview ketersediaan semua ruangan dalam periode tertentu
- Mempercepat proses pemilihan ruangan untuk booking baru

## Proposed Solution

**Endpoint:**

```
GET /api/ruangan/available?startDate={date}&endDate={date}
```

**Query Parameters:**

- `startDate` (required): ISO 8601 date format (e.g., `2026-03-10T08:00:00`)
- `endDate` (required): ISO 8601 date format (e.g., `2026-03-10T12:00:00`)

**Response Structure:**

```json
{
  "startDate": "2026-03-10T08:00:00Z",
  "endDate": "2026-03-10T12:00:00Z",
  "availableRooms": [
    {
      "id": 1,
      "namaRuangan": "Ruang A101",
      "lokasi": "Gedung A Lantai 1",
      "kapasitas": 30
    },
    {
      "id": 3,
      "namaRuangan": "Ruang B201",
      "lokasi": "Gedung B Lantai 2",
      "kapasitas": 50
    }
  ],
  "totalAvailable": 2,
  "totalRooms": 10
}
```

**Logic:**

- Query semua ruangan
- Untuk setiap ruangan, check apakah ada peminjaman yang overlap dengan range `[startDate, endDate]` dan `Status != "Rejected"`
- Filter ruangan yang **tidak memiliki** overlap booking
- Return list ruangan available dengan detail lengkap

**Alternative Query (SQL-optimized):**

```sql
SELECT * FROM ruangan r
WHERE r.id NOT IN (
  SELECT DISTINCT ruangan_id FROM peminjaman
  WHERE status != 'Rejected'
    AND tanggal_pinjam < @endDate
    AND tanggal_selesai > @startDate
)
```

## Acceptance Criteria

- [ ] Endpoint `GET /api/ruangan/available` tersedia
- [ ] Query parameters `startDate` dan `endDate` required dan format valid
- [ ] Validasi `endDate` > `startDate`, return 400 jika invalid
- [ ] Return only ruangan yang **tidak memiliki conflict** dalam range tersebut
- [ ] Ignore peminjaman dengan status "Rejected" dalam conflict check
- [ ] Response include ruangan details (id, nama, lokasi, kapasitas)
- [ ] Response include metadata: `totalAvailable`, `totalRooms`
- [ ] Handle edge case: semua ruangan booked = empty array dengan `totalAvailable: 0`
- [ ] Handle edge case: tidak ada peminjaman = semua ruangan available
- [ ] Return 200 OK dengan list available rooms
- [ ] Performance: query optimized untuk avoid N+1 problem

## Technical Notes

- Gunakan LINQ `Where()` dengan date range overlap logic: `TanggalPinjam < endDate && TanggalSelesai > startDate`
- Consider timezone handling (semua DateTime harus UTC)
- Add index pada `peminjaman(ruangan_id, tanggal_pinjam, tanggal_selesai)` untuk performance
- Consider caching jika data availability sering di-query
