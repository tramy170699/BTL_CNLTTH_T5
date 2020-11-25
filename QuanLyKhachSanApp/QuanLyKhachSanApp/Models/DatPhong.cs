using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("DatPhong")]
    public class DatPhong
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int DatPhongID { get; set; }

        public int? NhanVienID { get; set; }

        public int? LoaiPhongID { get; set; }
        public int? DichVuID { get; set; }

        [MaxLength(50)]
        public string HoTen { get; set; }

        [MaxLength(20)]
        public string SoDienThoai { get; set; }

        public DateTime? ThoiGianDat { get; set; }

        public int? SoLuongNguoi { get; set; }

        public int? SoNgayDat { get; set; }

        public double? TienCoc { get; set; }

        public DateTime? NgayTao { get; set; }

        public int TrangThai { get; set; }

        [ForeignKey("LoaiPhongID")]
        public virtual LoaiPhong LoaiPhong { get; set; }
        [ForeignKey("DichVuID")]
        public virtual DichVu DichVu { get; set; }

        [ForeignKey("NhanVienID")]
        public virtual NhanVien NhanVien { get; set; }

        [InverseProperty("DatPhong")]
        public virtual ICollection<HoaDon> HoaDon { get; set; }

    }
}
