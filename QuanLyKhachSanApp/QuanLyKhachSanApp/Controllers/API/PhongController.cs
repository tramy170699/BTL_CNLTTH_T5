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
    [RoutePrefix("api/phong")]
    public class PhongController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? loaiPhongID = null, [FromUri]DateTime? ngayNhanPhong = null, [FromUri]int? query = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<Phong> results = db.Phong;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(o => o.LoaiPhong);
                }

                if (loaiPhongID.HasValue) results = results.Where(o => o.LoaiPhongID == loaiPhongID);
                if (ngayNhanPhong.HasValue) results = results.Where(o => !o.ThuePhong
                                                                          .Any(x => x.HoaDon.ThoiGianTraPhong > ngayNhanPhong));
                if (query.HasValue) results = results.Where(o => o.SoPhong == query);

                results = results.OrderBy(o => o.PhongID);

                return Ok((await GetPaginatedResponse(results, pagination)));
            }
        }

        [HttpGet, Route("{phongID:int}")]
        public async Task<IHttpActionResult> Get(int phongID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var phong = await db.Phong
                    .Include(o => o.LoaiPhong)
                    .SingleOrDefaultAsync(o => o.PhongID == phongID);

                if (phong == null)
                    return NotFound();

                return Ok(phong);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]Phong phong)
        {
            if (phong.PhongID != 0) return BadRequest("Invalid PhongID");

            using (var db = new dbQuanLyKhachSan())
            {
                phong.VatDungPhong.ToList().ForEach(x => x.VatDung = null);
                db.Phong.Add(phong);
                await db.SaveChangesAsync();
            }

            return Ok(phong);
        }

        [HttpPut, Route("{phongID:int}")]
        public async Task<IHttpActionResult> Update(int phongID, [FromBody]Phong phong)
        {
            if (phong.PhongID != phongID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(phong).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.Phong.Count(o => o.PhongID == phongID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(phong);
            }
        }

        [HttpDelete, Route("{phongID:int}")]
        public async Task<IHttpActionResult> Delete(int phongID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var phong = await db.Phong.SingleOrDefaultAsync(o => o.PhongID == phongID);

                if (phong == null)
                    return NotFound();

                if (await db.ThuePhong.AnyAsync(o => o.PhongID == phong.PhongID))
                    return BadRequest("Unable to delete the phong as it has related thuephong");

                if (await db.VatDungPhong.AnyAsync(o => o.PhongID == phong.PhongID))
                    return BadRequest("Unable to delete the phong as it has related vatdungphong");

                db.Entry(phong).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
