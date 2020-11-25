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
    [RoutePrefix("api/vatdungphong")]
    public class VatDungPhongController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? vatDungID = null, [FromUri]int? phongID = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<VatDungPhong> results = db.VatDungPhong;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(o => o.VatDung)
                                     .Include(o => o.Phong);
                }

                if (vatDungID.HasValue) results = results.Where(o => o.VatDungID == vatDungID);
                if (phongID.HasValue) results = results.Where(o => o.PhongID == phongID);

                results = results.OrderBy(o => o.VatDungPhongID);

                return Ok((await GetPaginatedResponse(results, pagination)));
            }
        }

        [HttpGet, Route("{vatDungPhongID:int}")]
        public async Task<IHttpActionResult> Get(int vatDungPhongID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var vatDungPhong = await db.VatDungPhong
                    .Include(o => o.VatDung)
                    .SingleOrDefaultAsync(o => o.VatDungPhongID == vatDungPhongID);

                if (vatDungPhong == null)
                    return NotFound();

                return Ok(vatDungPhong);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]VatDungPhong vatDungPhong)
        {
            if (vatDungPhong.VatDungPhongID != 0) return BadRequest("Invalid VatDungPhongID");

            using (var db = new dbQuanLyKhachSan())
            {
                db.VatDungPhong.Add(vatDungPhong);
                await db.SaveChangesAsync();
            }

            return Ok(vatDungPhong);
        }

        [HttpPut, Route("{vatDungPhongID:int}")]
        public async Task<IHttpActionResult> Update(int vatDungPhongID, [FromBody]VatDungPhong vatDungPhong)
        {
            if (vatDungPhong.VatDungPhongID != vatDungPhongID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(vatDungPhong).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.VatDungPhong.Count(o => o.VatDungPhongID == vatDungPhongID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(vatDungPhong);
            }
        }

        [HttpDelete, Route("{vatDungPhongID:int}")]
        public async Task<IHttpActionResult> Delete(int vatDungPhongID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var vatDungPhong = await db.VatDungPhong.SingleOrDefaultAsync(o => o.VatDungPhongID == vatDungPhongID);

                if (vatDungPhong == null)
                    return NotFound();

                if (await db.ChiTietKiemKe.AnyAsync(o => o.VatDungPhongID == vatDungPhong.VatDungPhongID))
                    return BadRequest("Unable to delete the vatdungphong as it has related chitietkiemke");

                db.Entry(vatDungPhong).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
