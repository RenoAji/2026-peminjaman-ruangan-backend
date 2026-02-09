namespace PeminjamanRuangan.Api.DTOs;

public record CreatePeminjamanRequest(
    string NamaPeminjam,
    int RuanganId,
    DateTime TanggalPinjam,
    DateTime TanggalSelesai,
    string? Keperluan
);

public record UpdatePeminjamanRequest(
    string? NamaPeminjam,
    int? RuanganId,
    DateTime? TanggalPinjam,
    DateTime? TanggalSelesai,
    string? Keperluan,
    string? Status
);

public record PeminjamanResponse(
    int Id,
    string NamaPeminjam,
    int RuanganId,
    string NamaRuangan,
    DateTime TanggalPinjam,
    DateTime TanggalSelesai,
    string? Keperluan,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
