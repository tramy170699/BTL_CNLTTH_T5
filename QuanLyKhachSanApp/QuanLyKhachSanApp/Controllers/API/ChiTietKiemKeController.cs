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
    [RoutePrefix("api/chitietkiemke")]
    public class ChiTietKiemKeController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? kiemKeID = null, [FromUri]int? vatDungPhongID = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                //IQueryable<ChiTietKiemKe> results = db.ChiTietKiemKe;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                }

                //if (vatDungPhongID.HasValue) results = results.Where(o => o.VatDungPhongID == vatDungPhongID);

                var res = db.VatDung.Include(x => x.VatDungPhong)
                                    .Include(x => x.VatDungPhong.Select(y => y.Phong))
                                    .Include(x => x.VatDungPhong.Select(y => y.ChiTietKiemKe));

               
                var results = res.Select(x => new
                {
                    x.VatDungID,
                    x.TenVatDung,
                    x.SoLuongKho,
                    SoLuongTieuChuan = x.VatDungPhong
                                        .Sum(y => y.SoLuong),
                    SoLuongKiemKe = x.VatDungPhong
                                     .Sum(y => y.ChiTietKiemKe.Where(z => z.KiemKeID == kiemKeID).Sum(z => z.SoLuongKiemKe))
                });
                results = results.OrderBy(o => o.VatDungID);

                return Ok((await GetPaginatedResponse(results, pagination)));
            }
        }
        [HttpGet, Route("getds")]
        public async Task<IHttpActionResult> GetDS([FromUri]Pagination pagination, [FromUri]int? kiemKeID = null, [FromUri]int? phongID = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<ChiTietKiemKe> results = db.ChiTietKiemKe;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(x => x.VatDungPhong)
                                     .Include(x => x.VatDungPhong.VatDung)
                                     .Include(x => x.VatDungPhong.Phong);
                }

                if (kiemKeID.HasValue) results = results.Where(o => o.KiemKeID == kiemKeID);
                if (phongID.HasValue) results = results.Where(o => o.VatDungPhong.PhongID == phongID);

                var res = results.Select(x => new
                {
                    x.ChiTietKiemKeID,
                    x.VatDungPhongID,
                    x.KiemKeID,
                    x.VatDungPhong.Phong.SoPhong,
                    x.VatDungPhong.VatDung.TenVatDung,
                    x.VatDungPhong.SoLuong,
                    x.SoLuongKiemKe,
                    x.GhiChu
                });
                res = res.OrderBy(o => o.ChiTietKiemKeID);

                return Ok((await GetPaginatedResponse(res, pagination)));
            }
        }
        [HttpGet, Route("{chiTietKiemKeID:int}")]
        public async Task<IHttpActionResult> Get(int chiTietKiemKeID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var chiTietKiemKe = await db.ChiTietKiemKe
                    .SingleOrDefaultAsync(o => o.ChiTietKiemKeID == chiTietKiemKeID);

                if (chiTietKiemKe == null)
                    return NotFound();

                return Ok(chiTietKiemKe);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]ChiTietKiemKe chiTietKiemKe)
        {
            if (chiTietKiemKe.ChiTietKiemKeID != 0) return BadRequest("Invalid ChiTietKiemKeID");

            using (var db = new dbQuanLyKhachSan())
            {
                db.ChiTietKiemKe.Add(chiTietKiemKe);
                await db.SaveChangesAsync();
            }

            return Ok(chiTietKiemKe);
        }
        [HttpPut, Route("capnhatdanhsach")]
        public async Task<IHttpActionResult> UpdateDanhSach([FromBody]List<ChiTietKiemKe> lstchiTietKiemKe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                lstchiTietKiemKe.ForEach(p => db.Entry(p).State = EntityState.Modified);
                await db.SaveChangesAsync();
                return Ok(lstchiTietKiemKe);
            }
        }

        [HttpPut, Route("{chiTietKiemKeID:int}")]
        public async Task<IHttpActionResult> Update(int chiTietKiemKeID, [FromBody]ChiTietKiemKe chiTietKiemKe)
        {
            if (chiTietKiemKe.ChiTietKiemKeID != chiTietKiemKeID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(chiTietKiemKe).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.ChiTietKiemKe.Count(o => o.ChiTietKiemKeID == chiTietKiemKeID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(chiTietKiemKe);
            }
        }

        [HttpDelete, Route("{chiTietKiemKeID:int}")]
        public async Task<IHttpActionResult> Delete(int chiTietKiemKeID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var chiTietKiemKe = await db.ChiTietKiemKe.SingleOrDefaultAsync(o => o.ChiTietKiemKeID == chiTietKiemKeID);

                if (chiTietKiemKe == null)
                    return NotFound();

                db.Entry(chiTietKiemKe).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
