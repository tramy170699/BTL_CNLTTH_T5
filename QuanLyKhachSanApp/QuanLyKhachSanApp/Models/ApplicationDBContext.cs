using System.Data.Entity;

namespace QuanLyKhachSanApp.Models
{
    public partial class dbQuanLyKhachSan : DbContext
    {
        public dbQuanLyKhachSan(): base("name=dbQuanLyKhachSan")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, HVITCore.Migrations.Configuration>());
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<BangGia> BangGia { get; set; }
        public DbSet<BoPhan> BoPhan { get; set; }
        public DbSet<ChiTietKiemKe> ChiTietKiemKe { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }
        public DbSet<DatDichVu> DatDichVu { get; set; }
        public DbSet<DatPhong> DatPhong { get; set; }
        public DbSet<DichVu> DichVu { get; set; }
        public DbSet<HoaDon> HoaDon { get; set; }
        public DbSet<KiemKe> KiemKe { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<LoaiPhong> LoaiPhong { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<PhieuNhapKho> PhieuNhapKho { get; set; }
        public DbSet<Phong> Phong { get; set; }
        public DbSet<ThuePhong> ThuePhong { get; set; }
        public DbSet<VatDung> VatDung { get; set; }
        public DbSet<VatDungPhong> VatDungPhong { get; set; }
        public DbSet<AccessTokens> AccessTokens { get; set; }
        public DbSet<Users> Users { get; set; }

        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
        }
    }
}
