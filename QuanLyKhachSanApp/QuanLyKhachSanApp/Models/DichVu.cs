using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("DichVu")]
    public class DichVu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int DichVuID { get; set; }

        

        public string TenDichVu { get; set; }

        [MaxLength(200)]
        public string MoTa { get; set; }

        public double? GiaBan { get; set; }

        [MaxLength(20)]
        public string DonViTinh { get; set; }

        public bool? TrangThai { get; set; }

        [InverseProperty("DichVu")]
        public virtual ICollection<DatDichVu> DatDichVu { get; set; }
        [InverseProperty("DichVu")]
        public virtual ICollection<DatPhong> DatPhong { get; set; }
    }
}
