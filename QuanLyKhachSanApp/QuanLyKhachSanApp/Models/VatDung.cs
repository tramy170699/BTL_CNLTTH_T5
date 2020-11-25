using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSanApp.Models
{
    [Table("VatDung")]
    public class VatDung
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int VatDungID { get; set; }

        [MaxLength(50)]
        public string TenVatDung { get; set; }

        public int? SoLuongKho { get; set; }

        [MaxLength(200)]
        public string MoTa { get; set; }

        [InverseProperty("VatDung")]
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }

        [InverseProperty("VatDung")]
        public virtual ICollection<VatDungPhong> VatDungPhong { get; set; }

    }
}
