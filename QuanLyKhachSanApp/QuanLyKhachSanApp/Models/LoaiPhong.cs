using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("LoaiPhong")]
    public class LoaiPhong
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int LoaiPhongID { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(50)]
        public string TenLoaiPhong { get; set; }

        public double? GiaPhong { get; set; }

        [MaxLength(500)]
        public string MoTa { get; set; }

        [InverseProperty("LoaiPhong")]
        public virtual ICollection<BangGia> BangGia { get; set; }

        [InverseProperty("LoaiPhong")]
        public virtual ICollection<DatPhong> DatPhong { get; set; }

        [InverseProperty("LoaiPhong")]
        public virtual ICollection<Phong> Phong { get; set; }

    }
}
