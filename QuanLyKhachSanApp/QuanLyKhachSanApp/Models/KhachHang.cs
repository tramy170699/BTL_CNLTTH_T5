using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("KhachHang")]
    public class KhachHang
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int KhachHangID { get; set; }

        [MaxLength(50)]
        public string HoTen { get; set; }

        public bool? GioiTinh { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? NgaySinh { get; set; }

        [MaxLength(20)]
        public string SoDienThoai { get; set; }

        [MaxLength(200)]
        public string DiaChi { get; set; }

        [MaxLength(20)]
        public string CMND { get; set; }

        [InverseProperty("KhachHang")]
        public virtual ICollection<HoaDon> HoaDon { get; set; }

    }
}
