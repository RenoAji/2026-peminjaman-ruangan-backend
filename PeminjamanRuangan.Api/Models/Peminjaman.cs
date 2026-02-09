using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeminjamanRuangan.Api.Models;

[Table("peminjaman")]
public class Peminjaman
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nama_peminjam")]
    public string NamaPeminjam { get; set; } = string.Empty;

    [Required]
    [Column("ruangan_id")]
    public int RuanganId { get; set; }

    [Required]
    [Column("tanggal_pinjam")]
    public DateTime TanggalPinjam { get; set; }

    [Required]
    [Column("tanggal_selesai")]
    public DateTime TanggalSelesai { get; set; }

    [MaxLength(255)]
    [Column("keperluan")]
    public string? Keperluan { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Done

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    [ForeignKey("RuanganId")]
    public Ruangan Ruangan { get; set; } = null!;
}
