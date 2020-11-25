using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("PhieuNhapKho")]
    public class PhieuNhapKho
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int PhieuNhapID { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? NgayNhap { get; set; }

        public double? TongTien { get; set; }

        public int? NhanVienID { get; set; }

        [ForeignKey("NhanVienID")]
        public virtual NhanVien NhanVien { get; set; }

        [InverseProperty("PhieuNhapKho")]
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }

    }
}
