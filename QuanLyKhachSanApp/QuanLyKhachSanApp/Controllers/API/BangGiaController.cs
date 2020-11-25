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
    [RoutePrefix("api/banggia")]
    public class BangGiaController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? loaiPhongID = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<BangGia> results = db.BangGia;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(o => o.LoaiPhong);
                }

                if (loaiPhongID.HasValue) results = results.Where(o => o.LoaiPhongID == loaiPhongID);

                results = results.OrderByDescending(o => o.ApDungDenNgay);

                return Ok((await GetPaginatedResponse(results, pagination)));
            }
        }

        [HttpGet, Route("{bangGiaID:int}")]
        public async Task<IHttpActionResult> Get(int bangGiaID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var bangGia = await db.BangGia
                    .Include(o => o.LoaiPhong)
                    .SingleOrDefaultAsync(o => o.BangGiaID == bangGiaID);

                if (bangGia == null)
                    return NotFound();

                return Ok(bangGia);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]BangGia bangGia)
        {
            if (bangGia.BangGiaID != 0) return BadRequest("Invalid BangGiaID");

            using (var db = new dbQuanLyKhachSan())
            {
                db.BangGia.Add(bangGia);
                await db.SaveChangesAsync();
            }

            return Ok(bangGia);
        }

        [HttpGet, Route("giaapdung")]
        public async Task<IHttpActionResult> GetGiaApDung([FromUri]DateTime? ngayNhanPhong, [FromUri]int loaiPhongID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var loaiPhong = await db.LoaiPhong.Include(x => x.BangGia)
                    .SingleOrDefaultAsync(o => o.LoaiPhongID == loaiPhongID);

                if (loaiPhong == null)
                    return NotFound();

                var giaApDung = loaiPhong.BangGia.FirstOrDefault(x => x.ApDungTuNgay <= ngayNhanPhong
                                                          && x.ApDungDenNgay >= ngayNhanPhong);

                return Ok(giaApDung);
            }
        }

        [HttpPut, Route("{bangGiaID:int}")]
        public async Task<IHttpActionResult> Update(int bangGiaID, [FromBody]BangGia bangGia)
        {
            if (bangGia.BangGiaID != bangGiaID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(bangGia).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.BangGia.Count(o => o.BangGiaID == bangGiaID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(bangGia);
            }
        }

        [HttpDelete, Route("{bangGiaID:int}")]
        public async Task<IHttpActionResult> Delete(int bangGiaID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var bangGia = await db.BangGia.SingleOrDefaultAsync(o => o.BangGiaID == bangGiaID);

                if (bangGia == null)
                    return NotFound();

                if (await db.ThuePhong.AnyAsync(o => o.BangGiaID == bangGia.BangGiaID))
                    return BadRequest("Unable to delete the banggia as it has related thuephong");

                db.Entry(bangGia).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
