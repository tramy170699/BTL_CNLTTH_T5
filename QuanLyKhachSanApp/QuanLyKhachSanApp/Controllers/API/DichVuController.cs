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
    [RoutePrefix("api/dichvu")]
    public class DichVuController : BaseApiController
    {
        // fromBody - Lon, fromUri - Nho
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]string tenDichVu = null, [FromUri]float? giaBan = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<DichVu> results = db.DichVu;

                if (!string.IsNullOrWhiteSpace(tenDichVu))
                    results = results.Where(x => x.TenDichVu.Contains(tenDichVu));
                if (giaBan.HasValue)
                    results = results.Where(x => x.GiaBan == giaBan);
                results = results.OrderBy(o => o.DichVuID);

                return Ok((await GetPaginatedResponse(results, pagination)));
            }
        }

        [HttpGet, Route("{dichVuID:int}")]
        public async Task<IHttpActionResult> Get(int dichVuID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var dichVu = await db.DichVu
                    .SingleOrDefaultAsync(o => o.DichVuID == dichVuID);

                if (dichVu == null)
                    return NotFound();

                return Ok(dichVu);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]DichVu dichVu)
        {
            if (dichVu.DichVuID != 0) return BadRequest("Invalid DichVuID");

            using (var db = new dbQuanLyKhachSan())
            {
                db.DichVu.Add(dichVu);
                await db.SaveChangesAsync();
            }

            return Ok(dichVu);
        }

        [HttpPut, Route("{dichVuID:int}")]
        public async Task<IHttpActionResult> Update(int dichVuID, [FromBody]DichVu dichVu)
        {
            if (dichVu.DichVuID != dichVuID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(dichVu).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.DichVu.Count(o => o.DichVuID == dichVuID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(dichVu);
            }
        }

        [HttpDelete, Route("{dichVuID:int}")]
        public async Task<IHttpActionResult> Delete(int dichVuID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var dichVu = await db.DichVu.SingleOrDefaultAsync(o => o.DichVuID == dichVuID);

                if (dichVu == null)
                    return NotFound();

                if (await db.DatDichVu.AnyAsync(o => o.DichVuID == dichVu.DichVuID))
                    return BadRequest("Unable to delete the dichvu as it has related datdichvu");

                db.Entry(dichVu).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
