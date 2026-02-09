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
}