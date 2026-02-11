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

public record BookedPeriod(
    int Id,
    string NamaPeminjam,
    DateTime TanggalPinjam,
    DateTime TanggalSelesai,
    string Status
);

public record AvailablePeriod(
    DateTime Start,
    DateTime End
);

public record RuanganAvailabilityResponse(
    int RuanganId,
    string NamaRuangan,
    DateTime StartDate,
    DateTime EndDate,
    List<BookedPeriod> BookedPeriods,
    List<AvailablePeriod> AvailablePeriods
);
