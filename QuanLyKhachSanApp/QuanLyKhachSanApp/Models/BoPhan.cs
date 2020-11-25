using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("BoPhan")]
    public class BoPhan
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int BoPhanID { get; set; }

        [MaxLength(50)]
        public string TenBoPhan { get; set; }

        [MaxLength(500)]
        public string NhiemVu { get; set; }

        [MaxLength(200)]
        public string GhiChu { get; set; }

        [InverseProperty("BoPhan")]
        public virtual ICollection<NhanVien> NhanVien { get; set; }

    }
}
