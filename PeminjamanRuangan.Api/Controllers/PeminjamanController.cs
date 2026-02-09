using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeminjamanRuangan.Api.Data;
using PeminjamanRuangan.Api.DTOs;
using PeminjamanRuangan.Api.Models;

namespace PeminjamanRuangan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeminjamanController : ControllerBase
{
    private readonly AppDbContext _db;

    private static readonly string[] ValidStatuses = ["Pending", "Approved", "Rejected", "Done"];

    public PeminjamanController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/peminjaman
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var peminjaman = await _db.Peminjaman
            .AsNoTracking()
            .Include(p => p.Ruangan)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PeminjamanResponse(
                p.Id,
                p.NamaPeminjam,
                p.RuanganId,
                p.Ruangan.NamaRuangan,
                p.TanggalPinjam,
                p.TanggalSelesai,
                p.Keperluan,
                p.Status,
                p.CreatedAt,
                p.UpdatedAt
            ))
            .ToListAsync();

        return Ok(peminjaman);
    }

    // GET /api/peminjaman/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var peminjaman = await _db.Peminjaman
            .AsNoTracking()
            .Include(p => p.Ruangan)
            .Where(p => p.Id == id)
            .Select(p => new PeminjamanResponse(
                p.Id,
                p.NamaPeminjam,
                p.RuanganId,
                p.Ruangan.NamaRuangan,
                p.TanggalPinjam,
                p.TanggalSelesai,
                p.Keperluan,
                p.Status,
                p.CreatedAt,
                p.UpdatedAt
            ))
            .FirstOrDefaultAsync();

        if (peminjaman is null)
            return NotFound(new { message = "Peminjaman tidak ditemukan" });

        return Ok(peminjaman);
    }

    // POST /api/peminjaman
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePeminjamanRequest request)
    {
        // Validate ruangan exists
        var ruangan = await _db.Ruangan.FindAsync(request.RuanganId);
        if (ruangan is null)
            return BadRequest(new { message = "Ruangan tidak ditemukan" });

        // Validate tanggal_selesai > tanggal_pinjam
        if (request.TanggalSelesai <= request.TanggalPinjam)
            return BadRequest(new { message = "Tanggal selesai harus setelah tanggal pinjam" });

        // Check for time conflict (overlapping bookings on the same room that are not Rejected)
        var conflict = await _db.Peminjaman.AnyAsync(p =>
            p.RuanganId == request.RuanganId
            && p.Status != "Rejected"
            && p.TanggalPinjam < request.TanggalSelesai
            && p.TanggalSelesai > request.TanggalPinjam
        );
        if (conflict)
            return Conflict(new { message = "Ruangan sudah dipinjam pada waktu tersebut" });

        var peminjaman = new Peminjaman
        {
            NamaPeminjam = request.NamaPeminjam,
            RuanganId = request.RuanganId,
            TanggalPinjam = request.TanggalPinjam,
            TanggalSelesai = request.TanggalSelesai,
            Keperluan = request.Keperluan
        };

        _db.Peminjaman.Add(peminjaman);
        await _db.SaveChangesAsync();

        // Reload with navigation property
        await _db.Entry(peminjaman).Reference(p => p.Ruangan).LoadAsync();

        var response = new PeminjamanResponse(
            peminjaman.Id,
            peminjaman.NamaPeminjam,
            peminjaman.RuanganId,
            peminjaman.Ruangan.NamaRuangan,
            peminjaman.TanggalPinjam,
            peminjaman.TanggalSelesai,
            peminjaman.Keperluan,
            peminjaman.Status,
            peminjaman.CreatedAt,
            peminjaman.UpdatedAt
        );

        return CreatedAtAction(nameof(GetById), new { id = peminjaman.Id }, response);
    }

    // PUT /api/peminjaman/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePeminjamanRequest request)
    {
        var peminjaman = await _db.Peminjaman
            .Include(p => p.Ruangan)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (peminjaman is null)
            return NotFound(new { message = "Peminjaman tidak ditemukan" });

        // Validate status if provided
        if (request.Status is not null && !ValidStatuses.Contains(request.Status))
            return BadRequest(new { message = $"Status tidak valid. Gunakan: {string.Join(", ", ValidStatuses)}" });

        // Validate ruangan if provided
        if (request.RuanganId is not null)
        {
            var ruangan = await _db.Ruangan.FindAsync(request.RuanganId);
            if (ruangan is null)
                return BadRequest(new { message = "Ruangan tidak ditemukan" });
        }

        // Apply updates
        var newRuanganId = request.RuanganId ?? peminjaman.RuanganId;
        var newTanggalPinjam = request.TanggalPinjam ?? peminjaman.TanggalPinjam;
        var newTanggalSelesai = request.TanggalSelesai ?? peminjaman.TanggalSelesai;

        // Validate dates
        if (newTanggalSelesai <= newTanggalPinjam)
            return BadRequest(new { message = "Tanggal selesai harus setelah tanggal pinjam" });

        // Check for time conflict if ruangan or dates changed
        if (request.RuanganId is not null || request.TanggalPinjam is not null || request.TanggalSelesai is not null)
        {
            var conflict = await _db.Peminjaman.AnyAsync(p =>
                p.Id != id
                && p.RuanganId == newRuanganId
                && p.Status != "Rejected"
                && p.TanggalPinjam < newTanggalSelesai
                && p.TanggalSelesai > newTanggalPinjam
            );
            if (conflict)
                return Conflict(new { message = "Ruangan sudah dipinjam pada waktu tersebut" });
        }

        peminjaman.NamaPeminjam = request.NamaPeminjam ?? peminjaman.NamaPeminjam;
        peminjaman.RuanganId = newRuanganId;
        peminjaman.TanggalPinjam = newTanggalPinjam;
        peminjaman.TanggalSelesai = newTanggalSelesai;
        peminjaman.Keperluan = request.Keperluan ?? peminjaman.Keperluan;
        peminjaman.Status = request.Status ?? peminjaman.Status;
        peminjaman.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        // Reload ruangan if it changed
        if (request.RuanganId is not null)
            await _db.Entry(peminjaman).Reference(p => p.Ruangan).LoadAsync();

        var response = new PeminjamanResponse(
            peminjaman.Id,
            peminjaman.NamaPeminjam,
            peminjaman.RuanganId,
            peminjaman.Ruangan.NamaRuangan,
            peminjaman.TanggalPinjam,
            peminjaman.TanggalSelesai,
            peminjaman.Keperluan,
            peminjaman.Status,
            peminjaman.CreatedAt,
            peminjaman.UpdatedAt
        );

        return Ok(response);
    }

    // DELETE /api/peminjaman/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var peminjaman = await _db.Peminjaman.FindAsync(id);
        if (peminjaman is null)
            return NotFound(new { message = "Peminjaman tidak ditemukan" });

        _db.Peminjaman.Remove(peminjaman);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
