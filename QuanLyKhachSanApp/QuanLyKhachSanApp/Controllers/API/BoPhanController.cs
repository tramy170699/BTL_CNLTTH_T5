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
    [RoutePrefix("api/bophan")]
    public class BoPhanController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<BoPhan> results = db.BoPhan;

                results = results.OrderBy(o => o.BoPhanID);

                return Ok((await GetPaginatedResponse(results, pagination)));
            }
        }

        [HttpGet, Route("{boPhanID:int}")]
        public async Task<IHttpActionResult> Get(int boPhanID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var boPhan = await db.BoPhan
                    .SingleOrDefaultAsync(o => o.BoPhanID == boPhanID);

                if (boPhan == null)
                    return NotFound();

                return Ok(boPhan);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]BoPhan boPhan)
        {
            if (boPhan.BoPhanID != 0) return BadRequest("Invalid BoPhanID");

            using (var db = new dbQuanLyKhachSan())
            {
                db.BoPhan.Add(boPhan);
                await db.SaveChangesAsync();
            }

            return Ok(boPhan);
        }

        [HttpPut, Route("{boPhanID:int}")]
        public async Task<IHttpActionResult> Update(int boPhanID, [FromBody]BoPhan boPhan)
        {
            if (boPhan.BoPhanID != boPhanID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(boPhan).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.BoPhan.Count(o => o.BoPhanID == boPhanID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(boPhan);
            }
        }

        [HttpDelete, Route("{boPhanID:int}")]
        public async Task<IHttpActionResult> Delete(int boPhanID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var boPhan = await db.BoPhan.SingleOrDefaultAsync(o => o.BoPhanID == boPhanID);

                if (boPhan == null)
                    return NotFound();

                if (await db.NhanVien.AnyAsync(o => o.BoPhanID == boPhan.BoPhanID))
                    return BadRequest("Unable to delete the bophan as it has related nhanvien");

                db.Entry(boPhan).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
