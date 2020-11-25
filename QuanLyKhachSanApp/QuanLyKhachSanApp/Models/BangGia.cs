using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("BangGia")]
    public class BangGia
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int BangGiaID { get; set; }

        public int? LoaiPhongID { get; set; }

        public double? GiaPhong { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? ApDungTuNgay { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? ApDungDenNgay { get; set; }

        [ForeignKey("LoaiPhongID")]
        public virtual LoaiPhong LoaiPhong { get; set; }

        [InverseProperty("BangGia")]
        public virtual ICollection<ThuePhong> ThuePhong { get; set; }

    }
}
