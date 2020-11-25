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
    [RoutePrefix("api/datdichvu")]
    public class DatDichVuController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? dichVuID = null, [FromUri]int? hoaDonID = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<DatDichVu> results = db.DatDichVu;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(o => o.DichVu);
                }

                if (dichVuID.HasValue) results = results.Where(o => o.DichVuID == dichVuID);
                if (hoaDonID.HasValue) results = results.Where(o => o.HoaDonID == hoaDonID);

                var res = results.Select(x => new
                {
                    x.DatDichVuID,
                    x.DichVu.TenDichVu,
                    x.DichVu.GiaBan,
                    x.DichVu.MoTa,
                    x.SoLuong
                });

                res = res.OrderBy(o => o.DatDichVuID);

                return Ok((await GetPaginatedResponse(res, pagination)));
            }
        }

        [HttpGet, Route("{datDichVuID:int}")]
        public async Task<IHttpActionResult> Get(int datDichVuID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var datDichVu = await db.DatDichVu
                    .Include(o => o.DichVu)
                    .SingleOrDefaultAsync(o => o.DatDichVuID == datDichVuID);

                if (datDichVu == null)
                    return NotFound();

                return Ok(datDichVu);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]DatDichVu datDichVu)
        {
            if (datDichVu.DatDichVuID != 0) return BadRequest("Invalid DatDichVuID");

            using (var db = new dbQuanLyKhachSan())
            {
                db.DatDichVu.Add(datDichVu);
                await db.SaveChangesAsync();
            }

            return Ok(datDichVu);
        }

        [HttpPut, Route("{datDichVuID:int}")]
        public async Task<IHttpActionResult> Update(int datDichVuID, [FromBody]DatDichVu datDichVu)
        {
            if (datDichVu.DatDichVuID != datDichVuID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(datDichVu).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.DatDichVu.Count(o => o.DatDichVuID == datDichVuID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(datDichVu);
            }
        }

        [HttpDelete, Route("{datDichVuID:int}")]
        public async Task<IHttpActionResult> Delete(int datDichVuID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var datDichVu = await db.DatDichVu.SingleOrDefaultAsync(o => o.DatDichVuID == datDichVuID);

                if (datDichVu == null)
                    return NotFound();

                db.Entry(datDichVu).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
