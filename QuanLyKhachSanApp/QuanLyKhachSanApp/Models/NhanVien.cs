using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("NhanVien")]
    public class NhanVien
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int NhanVienID { get; set; }

        [MaxLength(50)]
        public string TenNhanVien { get; set; }

        [MaxLength(20)]
        public string SoDienThoai { get; set; }

        public int? BoPhanID { get; set; }

        public int? UsersID { get; set; }

        [ForeignKey("BoPhanID")]
        public virtual BoPhan BoPhan { get; set; }

        [ForeignKey("UsersID")]
        public virtual Users Users { get; set; }

        [InverseProperty("NhanVien")]
        public virtual ICollection<DatPhong> DatPhong { get; set; }

        [InverseProperty("NhanVien")]
        public virtual ICollection<HoaDon> HoaDon { get; set; }

        [InverseProperty("NhanVien")]
        public virtual ICollection<KiemKe> KiemKe { get; set; }

        [InverseProperty("NhanVien")]
        public virtual ICollection<PhieuNhapKho> PhieuNhapKho { get; set; }

    }
}
