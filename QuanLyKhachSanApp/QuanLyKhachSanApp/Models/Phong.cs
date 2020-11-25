using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("Phong")]
    public class Phong
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int PhongID { get; set; }

        [Required]
        public int SoPhong { get; set; }

        public bool? TrangThai { get; set; }

        public int? LoaiPhongID { get; set; }

        [ForeignKey("LoaiPhongID")]
        public virtual LoaiPhong LoaiPhong { get; set; }

        [InverseProperty("Phong")]
        public virtual ICollection<ThuePhong> ThuePhong { get; set; }

        [InverseProperty("Phong")]
        public virtual ICollection<VatDungPhong> VatDungPhong { get; set; }

    }
}
