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
    [RoutePrefix("api/datphong")]
    public class DatPhongController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? nhanVienID = null, [FromUri]string HoTen = null,[FromUri]DateTime? NgayBD = null, [FromUri]DateTime? NgayKT = null,[FromUri]int? TrangThai = null, [FromUri]bool? laDatPhong = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<DatPhong> results = db.DatPhong;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(o => o.LoaiPhong).Include(o => o.DichVu);
                }
                if(TrangThai.HasValue) results = results.Where(o => o.TrangThai == TrangThai);
                if (nhanVienID.HasValue) results = results.Where(o => o.NhanVienID == nhanVienID);
                if (!string.IsNullOrWhiteSpace(HoTen)) results = results.Where(o => o.HoTen.Contains(HoTen));
                if (NgayBD.HasValue) results = results.Where(o => o.ThoiGianDat >= NgayBD);
                if (NgayKT.HasValue) results = results.Where(o => o.ThoiGianDat <= NgayKT);
                if (laDatPhong.HasValue && laDatPhong == true)
                    results = results.Where(o => o.LoaiPhongID != null);
                if (laDatPhong.HasValue && laDatPhong == false)
                    results = results.Where(o => o.DichVuID != null);
                results = results.OrderBy(o => o.TrangThai);

                return Ok((await GetPaginatedResponse(results, pagination)));
            }
        }

        [HttpGet, Route("{datPhongID:int}")]
        public async Task<IHttpActionResult> Get(int datPhongID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var datPhong = await db.DatPhong
                    .Include(o => o.LoaiPhong)
                    .SingleOrDefaultAsync(o => o.DatPhongID == datPhongID);

                if (datPhong == null)
                    return NotFound();

                return Ok(datPhong);
            }
        }
        
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]DatPhong datPhong)
        {
            if (datPhong.DatPhongID != 0) return BadRequest("Invalid DatPhongID");
            datPhong.NgayTao = DateTime.Now;
            datPhong.TrangThai = 0;
            using (var db = new dbQuanLyKhachSan())
            {
                db.DatPhong.Add(datPhong);
                await db.SaveChangesAsync();
            }

            return Ok(datPhong);
        }

        [HttpPut, Route("{datPhongID:int}")]
        public async Task<IHttpActionResult> Update(int datPhongID, [FromBody]DatPhong datPhong)
        {
            if (datPhong.DatPhongID != datPhongID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                datPhong.NgayTao = DateTime.Now;
                db.Entry(datPhong).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.DatPhong.Count(o => o.DatPhongID == datPhongID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(datPhong);
            }
        }

        [HttpDelete, Route("{datPhongID:int}")]
        public async Task<IHttpActionResult> Delete(int datPhongID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var datPhong = await db.DatPhong.SingleOrDefaultAsync(o => o.DatPhongID == datPhongID);

                if (datPhong == null)
                    return NotFound();
                datPhong.TrangThai = 2;

                db.Entry(datPhong).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.DatPhong.Count(o => o.DatPhongID == datPhongID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(datPhong);
            }
        }

    }
}
