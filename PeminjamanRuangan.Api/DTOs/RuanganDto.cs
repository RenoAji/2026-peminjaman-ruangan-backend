namespace PeminjamanRuangan.Api.DTOs;

public record CreateRuanganRequest(
    string NamaRuangan,
    string? Lokasi,
    int Kapasitas
);

public record RuanganResponse(
    int Id,
    string NamaRuangan,
    string? Lokasi,
    int Kapasitas,
    bool IsAvailable,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
