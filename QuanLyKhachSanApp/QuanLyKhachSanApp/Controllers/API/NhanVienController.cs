using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using HVITCore.Controllers;
using System.Data.Entity.Infrastructure;
using QuanLyKhachSanApp.Models;
using HVIT.Security;

namespace QuanLyKhachSanApp.Controllers
{
    [RoutePrefix("api/nhanvien")]
    public class NhanVienController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? boPhanID = null, [FromUri]string query = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                IQueryable<NhanVien> results = db.NhanVien;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(o => o.BoPhan).Include(o => o.Users);
                }
                if (boPhanID.HasValue) results = results.Where(o => o.BoPhanID == boPhanID);
             if (!string.IsNullOrWhiteSpace(query)) results = results.Where(o => o.TenNhanVien.Contains(query) || o.SoDienThoai.Contains(query));

                results = results.OrderBy(o => o.NhanVienID);

                return Ok((await GetPaginatedResponse(results, pagination)));
            }
        }

        [HttpGet, Route("{nhanVienID:int}")]
        public async Task<IHttpActionResult> Get(int nhanVienID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var nhanVien = await db.NhanVien
                    .Include(o => o.BoPhan).Include(o => o.Users)
                    .SingleOrDefaultAsync(o => o.NhanVienID == nhanVienID);

                if (nhanVien == null)
                    return NotFound();

                return Ok(nhanVien);
            }
        }
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]NhanVien nhanVien)
        {
            if (nhanVien.NhanVienID != 0) return BadRequest("Invalid NhanVienID");

            using (var db = new dbQuanLyKhachSan())
            {
                nhanVien.Users.CreatedTime = DateTime.Now;
                nhanVien.Users.Lang = "vi";
                nhanVien.Users.Active = true;
                nhanVien.Users.PasswordSalt = AuthenticationHelper.RamdomString(5);
                nhanVien.Users.Password = AuthenticationHelper.GetMd5Hash(nhanVien.Users.PasswordSalt + nhanVien.Users.Password);
                db.NhanVien.Add(nhanVien);
                //db.Users.Add();
                await db.SaveChangesAsync();
            }

            return Ok(nhanVien);
        }

        [HttpPut, Route("{nhanVienID:int}")]
        public async Task<IHttpActionResult> Update(int nhanVienID, [FromBody]NhanVien nhanVien)
        {
            if (nhanVien.NhanVienID != nhanVienID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new dbQuanLyKhachSan())
            {
                db.Entry(nhanVien).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.NhanVien.Count(o => o.NhanVienID == nhanVienID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(nhanVien);
            }
        }

        [HttpDelete, Route("{nhanVienID:int}")]
        public async Task<IHttpActionResult> Delete(int nhanVienID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var nhanVien = await db.NhanVien.Include(o => o.Users).SingleOrDefaultAsync(o => o.NhanVienID == nhanVienID);

                if (nhanVien == null)
                    return NotFound();

                if (await db.DatPhong.AnyAsync(o => o.NhanVienID == nhanVien.NhanVienID))
                    return BadRequest("Unable to delete the nhanvien as it has related datphong");

                if (await db.HoaDon.AnyAsync(o => o.NhanVienID == nhanVien.NhanVienID))
                    return BadRequest("Unable to delete the nhanvien as it has related hoadon");

                if (await db.KiemKe.AnyAsync(o => o.NhanVienID == nhanVien.NhanVienID))
                    return BadRequest("Unable to delete the nhanvien as it has related kiemke");

                if (await db.PhieuNhapKho.AnyAsync(o => o.NhanVienID == nhanVien.NhanVienID))
                    return BadRequest("Unable to delete the nhanvien as it has related phieunhapkho");

                if (nhanVien.Users != null)
                    db.Entry(nhanVien.Users).State = EntityState.Deleted;
                db.Entry(nhanVien).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
