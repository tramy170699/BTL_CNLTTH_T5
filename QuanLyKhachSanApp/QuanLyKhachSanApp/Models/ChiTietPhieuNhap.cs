using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("ChiTietPhieuNhap")]
    public class ChiTietPhieuNhap
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int ChiTietPhieuNhapID { get; set; }

        [Required]
        public int PhieuNhapID { get; set; }

        public int? ThuocTinhID { get; set; }

        public int? SoLuong { get; set; }

        public double? GiaNhap { get; set; }

        [MaxLength(500)]
        public string GhiChu { get; set; }

        [ForeignKey("PhieuNhapID")]
        public virtual PhieuNhapKho PhieuNhapKho { get; set; }

        [ForeignKey("ThuocTinhID")]
        public virtual VatDung VatDung { get; set; }

    }
}
