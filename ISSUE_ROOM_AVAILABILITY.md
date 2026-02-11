# Issue #1: [Feature] Get Room Availability by Room ID

## Description

Implementasi endpoint untuk mengecek ketersediaan ruangan tertentu berdasarkan rentang tanggal. Endpoint ini akan mengembalikan daftar tanggal/periode waktu ketika ruangan tersebut sedang tidak dipinjam (available).

**Use Case:**

- User ingin melihat kapan saja Ruangan A tersedia dalam minggu/bulan tertentu
- Admin ingin mengecek schedule booking ruangan tertentu
- Membantu user menentukan waktu yang tepat untuk booking

## Proposed Solution

**Endpoint:**

```
GET /api/ruangan/:roomId/availability?startDate={date}&endDate={date}
```

**Query Parameters:**

- `startDate` (required): ISO 8601 date format (e.g., `2026-03-01`)
- `endDate` (required): ISO 8601 date format (e.g., `2026-03-31`)

**Response Structure:**

```json
{
  "ruanganId": 1,
  "namaRuangan": "Ruang A101",
  "startDate": "2026-03-01",
  "endDate": "2026-03-31",
  "bookedPeriods": [
    {
      "id": 5,
      "namaPeminjam": "John Doe",
      "tanggalPinjam": "2026-03-05T08:00:00Z",
      "tanggalSelesai": "2026-03-05T10:00:00Z",
      "status": "Approved"
    }
  ],
  "availablePeriods": [
    {
      "start": "2026-03-01T00:00:00Z",
      "end": "2026-03-05T08:00:00Z"
    },
    {
      "start": "2026-03-05T10:00:00Z",
      "end": "2026-03-31T23:59:59Z"
    }
  ]
}
```

**Logic:**

- Query peminjaman dengan `RuanganId = :roomId` dan `Status != "Rejected"`
- Filter peminjaman yang overlap dengan range `[startDate, endDate]`
- Sort by `TanggalPinjam`
- Calculate gaps between bookings sebagai available periods

## Acceptance Criteria

- [ ] Endpoint `GET /api/ruangan/:roomId/availability` tersedia
- [ ] Validasi `roomId` exists, return 404 jika tidak ditemukan
- [ ] Validasi `startDate` dan `endDate` required dan format valid
- [ ] Validasi `endDate` >= `startDate`, return 400 jika invalid
- [ ] Return list of booked periods (exclude status "Rejected")
- [ ] Return list of available periods (gaps between bookings)
- [ ] Response include metadata ruangan (id, nama)
- [ ] Booked periods sorted by tanggal pinjam ascending
- [ ] Handle edge case: no bookings = entire period available
- [ ] Handle edge case: fully booked = empty availablePeriods array
- [ ] Return 200 OK dengan data availability
