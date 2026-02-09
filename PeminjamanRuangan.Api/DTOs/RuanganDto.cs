namespace PeminjamanRuangan.Api.DTOs;

public record CreateRuanganRequest(
    string NamaRuangan,
    string? Lokasi,
    int Kapasitas
);

public record UpdateRuanganRequest(
    string? NamaRuangan,
    string? Lokasi,
    int? Kapasitas
);

public record RuanganResponse(
    int Id,
    string NamaRuangan,
    string? Lokasi,
    int Kapasitas,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
