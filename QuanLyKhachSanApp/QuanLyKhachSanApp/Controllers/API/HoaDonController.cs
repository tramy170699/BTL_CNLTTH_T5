using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using HVITCore.Controllers;
using System.Data.Entity.Infrastructure;
using QuanLyKhachSanApp.Models;

namespace QuanLyKhachSanApp.Controllers
{
    [RoutePrefix("api/hoadon")]
    public class HoaDonController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, 
                                                    [FromUri]int? nhanVienID = null, 
                                                    [FromUri]int? datPhongID = null, 
                                                    [FromUri]int? khachHangID = null, 
                                                    [FromUri]string query = null, 
                                                    [FromUri]bool? trangThai = null,
                                                    [FromUri]DateTime? tuNgay = null,
                                                    [FromUri]DateTime? denNgay = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<HoaDon> results = db.HoaDon;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(o => o.KhachHang)
                                     .Include(o => o.DatDichVu.Select(x => x.DichVu))
                                     .Include(o => o.ThuePhong.Select(z => z.BangGia));
                }

                if (nhanVienID.HasValue) results = results.Where(o => o.NhanVienID == nhanVienID);
                if (datPhongID.HasValue) results = results.Where(o => o.DatPhongID == datPhongID);
                if (khachHangID.HasValue) results = results.Where(o => o.KhachHangID == khachHangID);
                if (trangThai.HasValue) results = results.Where(o => o.TrangThai == trangThai);
                if (tuNgay.HasValue) results = results.Where(o => o.NgayThanhToan >= tuNgay);
                if (denNgay.HasValue) results = results.Where(o => o.NgayThanhToan <= denNgay);
                if (!string.IsNullOrWhiteSpace(query))
                    results = results.Where(o => o.MaHoaDon.Contains(query) || o.KhachHang.SoDienThoai.Contains(query));

                var res = results.Select(x => new
                {
                    x.HoaDonID,
                    x.MaHoaDon,
                    x.NgayThanhToan,
                    x.ThoiGianNhanPhong,
                    x.PhuThu,
                    x.TrangThai,
                    KhachHang = new
                    {
                        x.KhachHang.HoTen,
                        x.KhachHang.SoDienThoai
                    },
                    TongThanhToanPhong = x.ThuePhong.Sum(y => y.BangGia.GiaPhong),
                    TongThanhToanDichVu = x.DatDichVu.Sum(y => y.DichVu.GiaBan * y.SoLuong),
                });

                res = res.OrderBy(o => o.HoaDonID);

                return Ok((await GetPaginatedResponse(res, pagination)));
            }
        }

        [HttpGet, Route("{hoaDonID:int}")]
        public async Task<IHttpActionResult> Get(int hoaDonID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var hoaDon = await db.HoaDon
                    .Include(o => o.KhachHang)
                    .Include(o => o.ThuePhong)
                    .Include(o => o.ThuePhong.Select(y => y.BangGia))
                    .Include(o => o.ThuePhong.Select(y => y.Phong))
                    .Include(o => o.ThuePhong.Select(y => y.Phong.LoaiPhong))
                    .Include(o => o.DatDichVu)
                    .Include(o => o.DatDichVu.Select(y => y.DichVu))
                    .SingleOrDefaultAsync(o => o.HoaDonID == hoaDonID);

                if (hoaDon == null)
                    return NotFound();

                var res = new
                {
                    hoaDon.HoaDonID,
                    hoaDon.KhachHangID,
                    hoaDon.DatPhongID,
                    hoaDon.NhanVienID,
                    hoaDon.MaHoaDon,
                    hoaDon.NgayThanhToan,
                    hoaDon.ThoiGianNhanPhong,
                    hoaDon.ThoiGianTraPhong,
                    hoaDon.TrangThai,
                    hoaDon.KhachHang,
                    hoaDon.LyDo,
                    hoaDon.PhuThu,
                    hoaDon.SoTaiKhoan,
                    TongThanhToanPhong = hoaDon.ThuePhong.Sum(y => y.BangGia.GiaPhong),
                    TongThanhToanDichVu = hoaDon.DatDichVu.Sum(y => y.DichVu.GiaBan * y.SoLuong),
                    ThuePhong = hoaDon.ThuePhong.Select(x => new
                    {
                        x.ThuePhongID,
                        x.BangGia.GiaPhong,
                        x.Phong.SoPhong,
                        x.Phong.LoaiPhong.TenLoaiPhong
                    }),
                    DatDichVu = hoaDon.DatDichVu.Select(x => new
                    {
                        x.DatDichVuID,
                        x.DichVu.TenDichVu,
                        x.DichVu.GiaBan,
                        x.DichVu.MoTa,
                        x.SoLuong
                    })
                };

                return Ok(res);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]HoaDon hoaDon)
        {
            if (hoaDon.HoaDonID != 0) return BadRequest("Invalid HoaDonID");

            using (var db = new dbQuanLyKhachSan())
            {
                db.HoaDon.Add(hoaDon);
                await db.SaveChangesAsync();
                hoaDon.MaHoaDon = "HD-" + hoaDon.HoaDonID.ToString();
                db.Entry(hoaDon).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            return Ok(hoaDon);
        }

        [HttpPut, Route("{hoaDonID:int}")]
        public async Task<IHttpActionResult> Update(int hoaDonID, [FromBody]HoaDon hoaDon)
        {
            if (hoaDon.HoaDonID != hoaDonID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(hoaDon).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.HoaDon.Count(o => o.HoaDonID == hoaDonID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(hoaDon);
            }
        }

        [HttpDelete, Route("{hoaDonID:int}")]
        public async Task<IHttpActionResult> Delete(int hoaDonID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var hoaDon = await db.HoaDon.SingleOrDefaultAsync(o => o.HoaDonID == hoaDonID);

                if (hoaDon == null)
                    return NotFound();
                var lstDatDichVu = db.DatDichVu.Where(o => o.HoaDonID == hoaDon.HoaDonID);
                var lstThuePhong = db.ThuePhong.Where(o => o.HoaDonID == hoaDon.HoaDonID);
                if (lstDatDichVu.Count() > 0)
                    db.DatDichVu.RemoveRange(lstDatDichVu);

                if (lstThuePhong.Count() > 0)
                    db.ThuePhong.RemoveRange(lstThuePhong);

                db.Entry(hoaDon).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
