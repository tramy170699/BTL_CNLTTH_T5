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
    [RoutePrefix("api/khachhang")]
    public class KhachHangController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]string query = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<KhachHang> results = db.KhachHang;
                if (!string.IsNullOrWhiteSpace(query))
                    results = results.Where(x => x.HoTen.Contains(query) || x.SoDienThoai.Contains(query));
                results = results.OrderBy(o => o.KhachHangID);

                return Ok((await GetPaginatedResponse(results, pagination)));
            }
        }

        [HttpGet, Route("kiemtrathongtin")]
        public async Task<IHttpActionResult> Get([FromUri]string cmnd)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var khachHang = await db.KhachHang
                    .SingleOrDefaultAsync(o => o.CMND == cmnd);

                return Ok(khachHang);
            }
        }


        [HttpGet, Route("{khachHangID:int}")]
        public async Task<IHttpActionResult> Get(int khachHangID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var khachHang = await db.KhachHang
                    .SingleOrDefaultAsync(o => o.KhachHangID == khachHangID);

                if (khachHang == null)
                    return NotFound();

                return Ok(khachHang);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]KhachHang khachHang)
        {
            if (khachHang.KhachHangID != 0) return BadRequest("Invalid KhachHangID");

            using (var db = new dbQuanLyKhachSan())
            {
                db.KhachHang.Add(khachHang);
                await db.SaveChangesAsync();
            }

            return Ok(khachHang);
        }

        [HttpPut, Route("{khachHangID:int}")]
        public async Task<IHttpActionResult> Update(int khachHangID, [FromBody]KhachHang khachHang)
        {
            if (khachHang.KhachHangID != khachHangID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(khachHang).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.KhachHang.Count(o => o.KhachHangID == khachHangID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(khachHang);
            }
        }

        [HttpDelete, Route("{khachHangID:int}")]
        public async Task<IHttpActionResult> Delete(int khachHangID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var khachHang = await db.KhachHang.SingleOrDefaultAsync(o => o.KhachHangID == khachHangID);

                if (khachHang == null)
                    return NotFound();

                if (await db.HoaDon.AnyAsync(o => o.KhachHangID == khachHang.KhachHangID))
                    return BadRequest("Unable to delete the khachhang as it has related hoadon");

                db.Entry(khachHang).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
