using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeminjamanRuangan.Api.Data;
using PeminjamanRuangan.Api.DTOs;
using PeminjamanRuangan.Api.Models;

namespace PeminjamanRuangan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RuanganController : ControllerBase
{
    private readonly AppDbContext _db;

    public RuanganController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/ruangan
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var ruangan = await _db.Ruangan
            .AsNoTracking()
            .OrderBy(r => r.NamaRuangan)
            .Select(r => new RuanganResponse(
                r.Id,
                r.NamaRuangan,
                r.Lokasi,
                r.Kapasitas,
                r.CreatedAt,
                r.UpdatedAt
            ))
            .ToListAsync();

        return Ok(ruangan);
    }

    // GET /api/ruangan/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ruangan = await _db.Ruangan
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new RuanganResponse(
                r.Id,
                r.NamaRuangan,
                r.Lokasi,
                r.Kapasitas,
                r.CreatedAt,
                r.UpdatedAt
            ))
            .FirstOrDefaultAsync();

        if (ruangan is null)
            return NotFound(new { message = "Ruangan tidak ditemukan" });

        return Ok(ruangan);
    }

    // POST /api/ruangan
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRuanganRequest request)
    {
        var exists = await _db.Ruangan.AnyAsync(r => r.NamaRuangan == request.NamaRuangan);
        if (exists)
            return Conflict(new { message = "Nama ruangan sudah digunakan" });

        var ruangan = new Ruangan
        {
            NamaRuangan = request.NamaRuangan,
            Lokasi = request.Lokasi,
            Kapasitas = request.Kapasitas
        };

        _db.Ruangan.Add(ruangan);
        await _db.SaveChangesAsync();

        var response = new RuanganResponse(
            ruangan.Id,
            ruangan.NamaRuangan,
            ruangan.Lokasi,
            ruangan.Kapasitas,
            ruangan.CreatedAt,
            ruangan.UpdatedAt
        );

        return CreatedAtAction(nameof(GetById), new { id = ruangan.Id }, response);
    }

    // PUT /api/ruangan/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRuanganRequest request)
    {
        var ruangan = await _db.Ruangan.FindAsync(id);
        if (ruangan is null)
            return NotFound(new { message = "Ruangan tidak ditemukan" });
        var exists = await _db.Ruangan.AnyAsync(r => r.NamaRuangan == request.NamaRuangan && r.Id != id);
        if (exists)
            return Conflict(new { message = "Nama ruangan sudah digunakan" });

        ruangan.NamaRuangan = request.NamaRuangan ?? ruangan.NamaRuangan;
        ruangan.Lokasi = request.Lokasi ?? ruangan.Lokasi;
        ruangan.Kapasitas = request.Kapasitas ?? ruangan.Kapasitas;
        ruangan.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        var response = new RuanganResponse(
            ruangan.Id,
            ruangan.NamaRuangan,
            ruangan.Lokasi,
            ruangan.Kapasitas,
            ruangan.CreatedAt,
            ruangan.UpdatedAt
        );

        return Ok(response);
    }

    // DELETE /api/ruangan/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ruangan = await _db.Ruangan.FindAsync(id);
        if (ruangan is null)
            return NotFound(new { message = "Ruangan tidak ditemukan" });
            
        _db.Ruangan.Remove(ruangan);

        await _db.SaveChangesAsync();
        
        return NoContent();
    }

    // GET /api/ruangan/available?startDate={date}&endDate={date}
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableRooms(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        if (!startDate.HasValue || !endDate.HasValue)
            return BadRequest(new { message = "Parameter startDate dan endDate wajib diisi" });

        if (endDate.Value <= startDate.Value)
            return BadRequest(new { message = "endDate harus > startDate" });

        var availableRooms = await _db.Ruangan
            .AsNoTracking()
            .Where(r => !_db.Peminjaman.Any(p =>
                p.RuanganId == r.Id
                && p.Status != "Rejected"
                && p.TanggalPinjam < endDate.Value
                && p.TanggalSelesai > startDate.Value))
            .OrderBy(r => r.NamaRuangan)
            .Select(r => new RuanganResponse(
                r.Id,
                r.NamaRuangan,
                r.Lokasi,
                r.Kapasitas,
                r.CreatedAt,
                r.UpdatedAt
            ))
            .ToListAsync();

        return Ok(availableRooms);
    }

    // GET /api/ruangan/{id}/availability?startDate={date}&endDate={date}
    [HttpGet("{id:int}/availability")]
    public async Task<IActionResult> GetAvailability(
        int id,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        // Validate ruangan exists
        var ruangan = await _db.Ruangan.FindAsync(id);
        if (ruangan is null)
            return NotFound(new { message = "Ruangan tidak ditemukan" });

        // Validate query parameters
        if (!startDate.HasValue || !endDate.HasValue)
            return BadRequest(new { message = "Parameter startDate dan endDate wajib diisi" });

        if (endDate.Value < startDate.Value)
            return BadRequest(new { message = "endDate harus >= startDate" });

        // Query booked periods (exclude Rejected status)
        var bookedPeriods = await _db.Peminjaman
            .AsNoTracking()
            .Where(p => p.RuanganId == id
                && p.Status != "Rejected"
                && p.TanggalPinjam < endDate.Value
                && p.TanggalSelesai > startDate.Value)
            .OrderBy(p => p.TanggalPinjam)
            .Select(p => new BookedPeriod(
                p.Id,
                p.NamaPeminjam,
                p.TanggalPinjam,
                p.TanggalSelesai,
                p.Status
            ))
            .ToListAsync();

        // Calculate available periods (gaps between bookings)
        var availablePeriods = new List<AvailablePeriod>();
        var currentStart = startDate.Value;

        foreach (var booked in bookedPeriods)
        {
            // If there's a gap before this booking
            if (currentStart < booked.TanggalPinjam)
            {
                availablePeriods.Add(new AvailablePeriod(currentStart, booked.TanggalPinjam));
            }
            
            // Move current pointer to end of this booking
            if (booked.TanggalSelesai > currentStart)
            {
                currentStart = booked.TanggalSelesai;
            }
        }

        // Add final period if there's time left after last booking
        if (currentStart < endDate.Value)
        {
            availablePeriods.Add(new AvailablePeriod(currentStart, endDate.Value));
        }

        var response = new RuanganAvailabilityResponse(
            ruangan.Id,
            ruangan.NamaRuangan,
            startDate.Value,
            endDate.Value,
            bookedPeriods.ToList(),
            availablePeriods
        );

        return Ok(response);
    }
}
