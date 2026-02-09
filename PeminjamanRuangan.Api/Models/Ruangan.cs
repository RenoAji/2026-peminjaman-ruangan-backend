using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeminjamanRuangan.Api.Models;

[Table("ruangan")]
public class Ruangan
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("nama_ruangan")]
    public string NamaRuangan { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("lokasi")]
    public string? Lokasi { get; set; }

    [Column("kapasitas")]
    public int Kapasitas { get; set; }

    [Column("is_available")]
    public bool IsAvailable { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public ICollection<Peminjaman> Peminjaman { get; set; } = new List<Peminjaman>();
}
