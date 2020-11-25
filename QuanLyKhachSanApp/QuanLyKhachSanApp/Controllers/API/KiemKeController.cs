using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using HVITCore.Controllers;
using System.Data.Entity.Infrastructure;
using QuanLyKhachSanApp.Models;
using System.Collections.Generic;

namespace QuanLyKhachSanApp.Controllers
{
    [RoutePrefix("api/kiemke")]
    public class KiemKeController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? nhanVienID = null, [FromUri]DateTime? tuNgay = null, [FromUri]DateTime? denNgay = null, [FromUri]string maKiemKe = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<KiemKe> results = db.KiemKe;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(x => x.NhanVien);
                }

                if (nhanVienID.HasValue) results = results.Where(o => o.NhanVienID == nhanVienID);
                if (tuNgay.HasValue) results = results.Where(o => o.NgayKiemKe >= tuNgay);
                if (denNgay.HasValue) results = results.Where(o => o.NgayKiemKe <= denNgay);
                if (!string.IsNullOrWhiteSpace(maKiemKe)) results = results.Where(o => o.MaKiemKe.Contains(maKiemKe));

                results = results.OrderBy(o => o.KiemKeID);

                return Ok((await GetPaginatedResponse(results, pagination)));
            }
        }

        [HttpGet, Route("{kiemKeID:int}")]
        public async Task<IHttpActionResult> Get(int kiemKeID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var kiemKe = await db.KiemKe
                    .SingleOrDefaultAsync(o => o.KiemKeID == kiemKeID);

                if (kiemKe == null)
                    return NotFound();

                return Ok(kiemKe);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]KiemKe kiemKe)
        {
            if (kiemKe.KiemKeID != 0) return BadRequest("Invalid KiemKeID");

            using (var db = new dbQuanLyKhachSan())
            {
                db.KiemKe.Add(kiemKe);
                await db.SaveChangesAsync();
                kiemKe.MaKiemKe = "K-" + kiemKe.KiemKeID;
                List<ChiTietKiemKe> chiTietKiemKes = new List<ChiTietKiemKe>();
                db.VatDungPhong.ToList().ForEach(vd =>
                {
                    ChiTietKiemKe chiTietKiemKe = new ChiTietKiemKe();
                    chiTietKiemKe.KiemKeID = kiemKe.KiemKeID;
                    chiTietKiemKe.VatDungPhongID = vd.VatDungPhongID;
                    chiTietKiemKe.SoLuongKiemKe = vd.SoLuong;
                    chiTietKiemKes.Add(chiTietKiemKe);
                });
                db.ChiTietKiemKe.AddRange(chiTietKiemKes);
                await db.SaveChangesAsync();
                return Ok(kiemKe);
            }
        }
        [HttpPut, Route("{kiemKeID:int}")]
        public async Task<IHttpActionResult> Update(int kiemKeID, [FromBody]KiemKe kiemKe)
        {
            if (kiemKe.KiemKeID != kiemKeID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(kiemKe).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.KiemKe.Count(o => o.KiemKeID == kiemKeID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(kiemKe);
            }
        }

        [HttpDelete, Route("{kiemKeID:int}")]
        public async Task<IHttpActionResult> Delete(int kiemKeID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var kiemKe = await db.KiemKe.SingleOrDefaultAsync(o => o.KiemKeID == kiemKeID);

                if (kiemKe == null)
                    return NotFound();

                if (await db.ChiTietKiemKe.AnyAsync(o => o.KiemKeID == kiemKe.KiemKeID))
                    return BadRequest("Unable to delete the kiemke as it has related chitietkiemke");

                db.Entry(kiemKe).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
