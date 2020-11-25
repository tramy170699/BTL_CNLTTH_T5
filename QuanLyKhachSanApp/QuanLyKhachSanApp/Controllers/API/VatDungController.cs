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
    [RoutePrefix("api/vatdung")]
    public class VatDungController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<VatDung> results = db.VatDung;

                results = results.OrderBy(o => o.VatDungID);

                return Ok((await GetPaginatedResponse(results, pagination)));
            }
        }

        [HttpGet, Route("{vatDungID:int}")]
        public async Task<IHttpActionResult> Get(int vatDungID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var vatDung = await db.VatDung
                    .SingleOrDefaultAsync(o => o.VatDungID == vatDungID);

                if (vatDung == null)
                    return NotFound();

                return Ok(vatDung);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]VatDung vatDung)
        {
            if (vatDung.VatDungID != 0) return BadRequest("Invalid VatDungID");

            using (var db = new dbQuanLyKhachSan())
            {
                db.VatDung.Add(vatDung);
                await db.SaveChangesAsync();
            }

            return Ok(vatDung);
        }

        [HttpPut, Route("{vatDungID:int}")]
        public async Task<IHttpActionResult> Update(int vatDungID, [FromBody]VatDung vatDung)
        {
            if (vatDung.VatDungID != vatDungID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(vatDung).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.VatDung.Count(o => o.VatDungID == vatDungID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(vatDung);
            }
        }

        [HttpDelete, Route("{vatDungID:int}")]
        public async Task<IHttpActionResult> Delete(int vatDungID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var vatDung = await db.VatDung.SingleOrDefaultAsync(o => o.VatDungID == vatDungID);

                if (vatDung == null)
                    return NotFound();

                if (await db.ChiTietPhieuNhap.AnyAsync(o => o.ThuocTinhID == vatDung.VatDungID))
                    return BadRequest("Unable to delete the vatdung as it has related chitietphieunhap");

                if (await db.VatDungPhong.AnyAsync(o => o.VatDungID == vatDung.VatDungID))
                    return BadRequest("Unable to delete the vatdung as it has related vatdungphong");

                db.Entry(vatDung).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
