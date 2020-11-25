using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("ChiTietKiemKe")]
    public class ChiTietKiemKe
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int ChiTietKiemKeID { get; set; }

        public int? KiemKeID { get; set; }

        public int? VatDungPhongID { get; set; }

        public int? SoLuongKiemKe { get; set; }

        [MaxLength(500)]
        public string GhiChu { get; set; }

        [ForeignKey("KiemKeID")]
        public virtual KiemKe KiemKe { get; set; }

        [ForeignKey("VatDungPhongID")]
        public virtual VatDungPhong VatDungPhong { get; set; }

    }
}
