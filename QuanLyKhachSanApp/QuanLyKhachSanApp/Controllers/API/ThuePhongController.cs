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
    [RoutePrefix("api/thuephong")]
    public class ThuePhongController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? phongID = null, [FromUri]int? hoaDonID = null, [FromUri]int? bangGiaID = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<ThuePhong> results = db.ThuePhong;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(x => x.BangGia)
                                     .Include(x => x.Phong)
                                     .Include(x => x.Phong.LoaiPhong);
                }

                if (phongID.HasValue) results = results.Where(o => o.PhongID == phongID);
                if (hoaDonID.HasValue) results = results.Where(o => o.HoaDonID == hoaDonID);
                if (bangGiaID.HasValue) results = results.Where(o => o.BangGiaID == bangGiaID);

                var res = results.Select(x => new
                {
                    x.ThuePhongID,
                    x.BangGia.GiaPhong,
                    x.Phong.SoPhong,
                    x.Phong.LoaiPhong.TenLoaiPhong
                });

                res = res.OrderBy(o => o.ThuePhongID);

                return Ok((await GetPaginatedResponse(res, pagination)));
            }
        }

        [HttpGet, Route("{thuePhongID:int}")]
        public async Task<IHttpActionResult> Get(int thuePhongID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var thuePhong = await db.ThuePhong
                    .SingleOrDefaultAsync(o => o.ThuePhongID == thuePhongID);

                if (thuePhong == null)
                    return NotFound();

                return Ok(thuePhong);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]ThuePhong thuePhong)
        {
            if (thuePhong.ThuePhongID != 0) return BadRequest("Invalid ThuePhongID");

            using (var db = new dbQuanLyKhachSan())
            {
                db.ThuePhong.Add(thuePhong);
                await db.SaveChangesAsync();
            }

            return Ok(thuePhong);
        }

        [HttpPut, Route("{thuePhongID:int}")]
        public async Task<IHttpActionResult> Update(int thuePhongID, [FromBody]ThuePhong thuePhong)
        {
            if (thuePhong.ThuePhongID != thuePhongID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(thuePhong).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.ThuePhong.Count(o => o.ThuePhongID == thuePhongID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(thuePhong);
            }
        }

        [HttpDelete, Route("{thuePhongID:int}")]
        public async Task<IHttpActionResult> Delete(int thuePhongID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var thuePhong = await db.ThuePhong.SingleOrDefaultAsync(o => o.ThuePhongID == thuePhongID);

                if (thuePhong == null)
                    return NotFound();

                db.Entry(thuePhong).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
