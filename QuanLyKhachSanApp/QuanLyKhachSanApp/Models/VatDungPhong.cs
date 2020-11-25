using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("VatDungPhong")]
    public class VatDungPhong
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int VatDungPhongID { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [MaxLength(500)]
        public string GhiChu { get; set; }

        public int? VatDungID { get; set; }

        public int? PhongID { get; set; }

        [ForeignKey("PhongID")]
        public virtual Phong Phong { get; set; }

        [ForeignKey("VatDungID")]
        public virtual VatDung VatDung { get; set; }

        [InverseProperty("VatDungPhong")]
        public virtual ICollection<ChiTietKiemKe> ChiTietKiemKe { get; set; }

    }
}
