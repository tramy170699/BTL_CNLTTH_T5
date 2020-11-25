using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("DatDichVu")]
    public class DatDichVu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int DatDichVuID { get; set; }

        [Required]
        public int SoLuong { get; set; }

        public int? DichVuID { get; set; }

        public int? HoaDonID { get; set; }

        [ForeignKey("DichVuID")]
        public virtual DichVu DichVu { get; set; }

        [ForeignKey("HoaDonID")]
        public virtual HoaDon HoaDon { get; set; }

    }
}
