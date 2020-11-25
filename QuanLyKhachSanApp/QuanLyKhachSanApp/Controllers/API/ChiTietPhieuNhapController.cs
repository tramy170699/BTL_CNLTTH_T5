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
    [RoutePrefix("api/chitietphieunhap")]
    public class ChiTietPhieuNhapController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? phieuNhapID = null, [FromUri]int? thuocTinhID = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<ChiTietPhieuNhap> results = db.ChiTietPhieuNhap;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(o => o.VatDung);
                }

                if (phieuNhapID.HasValue) results = results.Where(o => o.PhieuNhapID == phieuNhapID);
                if (thuocTinhID.HasValue) results = results.Where(o => o.ThuocTinhID == thuocTinhID);

                results = results.OrderBy(o => o.ChiTietPhieuNhapID);

                return Ok((await GetPaginatedResponse(results, pagination)));
            }
        }

        [HttpGet, Route("{chiTietPhieuNhapID:int}")]
        public async Task<IHttpActionResult> Get(int chiTietPhieuNhapID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var chiTietPhieuNhap = await db.ChiTietPhieuNhap
                    .Include(o => o.VatDung)
                    .SingleOrDefaultAsync(o => o.ChiTietPhieuNhapID == chiTietPhieuNhapID);

                if (chiTietPhieuNhap == null)
                    return NotFound();

                return Ok(chiTietPhieuNhap);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]ChiTietPhieuNhap chiTietPhieuNhap)
        {
            if (chiTietPhieuNhap.ChiTietPhieuNhapID != 0) return BadRequest("Invalid ChiTietPhieuNhapID");
            
            using (var db = new dbQuanLyKhachSan())
            {
                var vatDung = await db.VatDung.SingleOrDefaultAsync(o => o.VatDungID == chiTietPhieuNhap.ThuocTinhID);
                vatDung.SoLuongKho += chiTietPhieuNhap.SoLuong;
                db.Entry(vatDung).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch { }
                db.ChiTietPhieuNhap.Add(chiTietPhieuNhap);
                await db.SaveChangesAsync();
            }
            return Ok(chiTietPhieuNhap);
        }

        [HttpPut, Route("{chiTietPhieuNhapID:int}")]
        public async Task<IHttpActionResult> Update(int chiTietPhieuNhapID, [FromBody]ChiTietPhieuNhap chiTietPhieuNhap)
        {
            if (chiTietPhieuNhap.ChiTietPhieuNhapID != chiTietPhieuNhapID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                var chiTiet = await db.ChiTietPhieuNhap.SingleOrDefaultAsync(o => o.ChiTietPhieuNhapID == chiTietPhieuNhap.ChiTietPhieuNhapID);
                var vatDung = await db.VatDung.SingleOrDefaultAsync(o => o.VatDungID == chiTietPhieuNhap.ThuocTinhID);
                vatDung.SoLuongKho += chiTietPhieuNhap.SoLuong - chiTiet.SoLuong;
                db.Entry(vatDung).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch { }
            }

            using (var db = new dbQuanLyKhachSan())
            {

                db.Entry(chiTietPhieuNhap).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.ChiTietPhieuNhap.Count(o => o.ChiTietPhieuNhapID == chiTietPhieuNhapID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(chiTietPhieuNhap);
            }
        }

        [HttpDelete, Route("{chiTietPhieuNhapID:int}")]
        public async Task<IHttpActionResult> Delete(int chiTietPhieuNhapID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var chiTietPhieuNhap = await db.ChiTietPhieuNhap.SingleOrDefaultAsync(o => o.ChiTietPhieuNhapID == chiTietPhieuNhapID);
                
                if (chiTietPhieuNhap == null)
                    return NotFound();
                //sl = chiTietPhieuNhap.SoLuong;
                var vatDung = await db.VatDung.SingleOrDefaultAsync(o => o.VatDungID == chiTietPhieuNhap.ThuocTinhID);
                vatDung.SoLuongKho -= chiTietPhieuNhap.SoLuong;
                db.Entry(vatDung).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch { }
                db.Entry(chiTietPhieuNhap).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                
            }
            using (var db = new dbQuanLyKhachSan())
            {

            }
            return Ok();
        }

    }
}
