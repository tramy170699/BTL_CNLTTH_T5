using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("KiemKe")]
    public class KiemKe
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int KiemKeID { get; set; }

        public DateTime? NgayKiemKe { get; set; }

        public int? NhanVienID { get; set; }

        [MaxLength(500)]
        public string GhiChu { get; set; }

        [MaxLength(20)]
        public string MaKiemKe { get; set; }

        [ForeignKey("NhanVienID")]
        public virtual NhanVien NhanVien { get; set; }

        [InverseProperty("KiemKe")]
        public virtual ICollection<ChiTietKiemKe> ChiTietKiemKe { get; set; }

    }
}
