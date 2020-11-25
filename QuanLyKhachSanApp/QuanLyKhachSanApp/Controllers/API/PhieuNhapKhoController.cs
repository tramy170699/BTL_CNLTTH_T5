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
    [RoutePrefix("api/phieunhapkho")]
    public class PhieuNhapKhoController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? phieuNhapID = null, [FromUri]DateTime? ngayBD = null, [FromUri]DateTime? ngayKT = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<PhieuNhapKho> results = db.PhieuNhapKho;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(x => x.NhanVien);
                }

                if (phieuNhapID.HasValue) results = results.Where(o => o.PhieuNhapID == phieuNhapID);
                if (ngayBD.HasValue) results = results.Where(o => o.NgayNhap >= ngayBD);
                if (ngayKT.HasValue) results = results.Where(o => o.NgayNhap <= ngayKT);

                results = results.OrderBy(o => o.PhieuNhapID);

                return Ok((await GetPaginatedResponse(results, pagination)));
            }
        }

        [HttpGet, Route("{phieuNhapID:int}")]
        public async Task<IHttpActionResult> Get(int phieuNhapID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var phieuNhapKho = await db.PhieuNhapKho
                    .SingleOrDefaultAsync(o => o.PhieuNhapID == phieuNhapID);

                if (phieuNhapKho == null)
                    return NotFound();

                return Ok(phieuNhapKho);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]PhieuNhapKho phieuNhapKho)
        {
            if (phieuNhapKho.PhieuNhapID != 0) return BadRequest("Invalid PhieuNhapID");

            using (var db = new dbQuanLyKhachSan())
            {
                db.PhieuNhapKho.Add(phieuNhapKho);
                await db.SaveChangesAsync();
            }
            return Ok(phieuNhapKho);
        }

        [HttpPut, Route("{phieuNhapID:int}")]
        public async Task<IHttpActionResult> Update(int phieuNhapID, [FromBody]PhieuNhapKho phieuNhapKho)
        {
            if (phieuNhapKho.PhieuNhapID != phieuNhapID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(phieuNhapKho).State = EntityState.Modified;
                
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.PhieuNhapKho.Count(o => o.PhieuNhapID == phieuNhapID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(phieuNhapKho);
            }
        }

        [HttpDelete, Route("{phieuNhapID:int}")]
        public async Task<IHttpActionResult> Delete(int phieuNhapID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var phieuNhapKho = await db.PhieuNhapKho.SingleOrDefaultAsync(o => o.PhieuNhapID == phieuNhapID);

                if (phieuNhapKho == null)
                    return NotFound();

                if (await db.ChiTietPhieuNhap.AnyAsync(o => o.PhieuNhapID == phieuNhapKho.PhieuNhapID))
                    return BadRequest("Unable to delete the phieunhapkho as it has related chitietphieunhap");

                db.Entry(phieuNhapKho).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
