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
    [RoutePrefix("api/loaiphong")]
    public class LoaiPhongController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]string query = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<LoaiPhong> results = db.LoaiPhong;

                if (!string.IsNullOrWhiteSpace(query))
                {
                    results = results.Where(o => o.TenLoaiPhong.Contains(query));
                    results = results.OrderBy(o => o.LoaiPhongID);
                    return Ok((await GetPaginatedResponse(results, pagination)));
                }
                 
                results = results.OrderBy(o => o.LoaiPhongID);
                
                var res = results.Select(x => new
                {
                    x.LoaiPhongID,
                    x.TenLoaiPhong,
                    x.MoTa,
                    GiaPhong = x.BangGia.Where(p => p.ApDungTuNgay <= DateTime.Now &&p.ApDungDenNgay >= DateTime.Now).FirstOrDefault().GiaPhong,
                });

                return Ok((await GetPaginatedResponse(res, pagination)));
            }
        }

        [HttpGet, Route("{loaiPhongID:int}")]
        public async Task<IHttpActionResult> Get(int loaiPhongID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var loaiPhong = await db.LoaiPhong
                    .SingleOrDefaultAsync(o => o.LoaiPhongID == loaiPhongID);

                if (loaiPhong == null)
                    return NotFound();

                return Ok(loaiPhong);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]LoaiPhong loaiPhong)
        {
            if (loaiPhong.LoaiPhongID != 0) return BadRequest("Invalid LoaiPhongID");

            using (var db = new dbQuanLyKhachSan())
            {
                db.LoaiPhong.Add(loaiPhong);
                await db.SaveChangesAsync();
            }

            return Ok(loaiPhong);
        }

        [HttpPut, Route("{loaiPhongID:int}")]
        public async Task<IHttpActionResult> Update(int loaiPhongID, [FromBody]LoaiPhong loaiPhong)
        {
            if (loaiPhong.LoaiPhongID != loaiPhongID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(loaiPhong).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.LoaiPhong.Count(o => o.LoaiPhongID == loaiPhongID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(loaiPhong);
            }
        }

        [HttpDelete, Route("{loaiPhongID:int}")]
        public async Task<IHttpActionResult> Delete(int loaiPhongID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var loaiPhong = await db.LoaiPhong.SingleOrDefaultAsync(o => o.LoaiPhongID == loaiPhongID);

                if (loaiPhong == null)
                    return NotFound();

                if (await db.BangGia.AnyAsync(o => o.LoaiPhongID == loaiPhong.LoaiPhongID))
                    return BadRequest("Unable to delete the loaiphong as it has related banggia");

                if (await db.DatPhong.AnyAsync(o => o.LoaiPhongID == loaiPhong.LoaiPhongID))
                    return BadRequest("Unable to delete the loaiphong as it has related datphong");

                if (await db.Phong.AnyAsync(o => o.LoaiPhongID == loaiPhong.LoaiPhongID))
                    return BadRequest("Unable to delete the loaiphong as it has related phong");

                db.Entry(loaiPhong).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
