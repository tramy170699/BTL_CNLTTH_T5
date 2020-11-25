using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("ThuePhong")]
    public class ThuePhong
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int ThuePhongID { get; set; }

        public int? PhongID { get; set; }

        public int? HoaDonID { get; set; }


        public int? BangGiaID { get; set; }

        [ForeignKey("BangGiaID")]
        public virtual BangGia BangGia { get; set; }

        [ForeignKey("HoaDonID")]
        public virtual HoaDon HoaDon { get; set; }

        [ForeignKey("PhongID")]
        public virtual Phong Phong { get; set; }

    }
}
