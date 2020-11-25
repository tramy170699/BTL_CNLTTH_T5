using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("HoaDon")]
    public class HoaDon
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int HoaDonID { get; set; }

        public int? NhanVienID { get; set; }

        public int? DatPhongID { get; set; }

        public int? KhachHangID { get; set; }

        [MaxLength(20)]
        public string MaHoaDon { get; set; }

        public DateTime? NgayThanhToan { get; set; }

        [MaxLength(20)]
        public string SoTaiKhoan { get; set; }

        public DateTime? ThoiGianNhanPhong { get; set; }

        public DateTime? ThoiGianTraPhong { get; set; }

        public double? PhuThu { get; set; }
        [MaxLength(500)]
        public string LyDo { get; set; }

        public bool? TrangThai { get; set; }

        [ForeignKey("DatPhongID")]
        public virtual DatPhong DatPhong { get; set; }

        [ForeignKey("KhachHangID")]
        public virtual KhachHang KhachHang { get; set; }

        [ForeignKey("NhanVienID")]
        public virtual NhanVien NhanVien { get; set; }

        [InverseProperty("HoaDon")]
        public virtual ICollection<DatDichVu> DatDichVu { get; set; }

        [InverseProperty("HoaDon")]
        public virtual ICollection<ThuePhong> ThuePhong { get; set; }

    }
}
