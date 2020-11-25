using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Hosting;
using System.IO;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Diagnostics;
using System.Globalization;
using QuanLyKhachSanApp.Models;
using OfficeOpenXml.Core.ExcelPackage;

namespace QuanLyKhachSanApp.Controllers
{
    public class ExportController : Controller
    {
        [HttpGet]
        public FileResult ExportHoaDon(string tuNgay = null, string denNgay = null)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                string sWebRootFolder = HostingEnvironment.ApplicationPhysicalPath + "\\FileUpload";
                IQueryable<HoaDon> results = db.HoaDon.Where(x => x.TrangThai == true);
                results = results.Include(x => x.ThuePhong).Include(x => x.NhanVien)
                                 .Include(x => x.KhachHang).Include(x => x.ThuePhong.Select(y => y.BangGia))
                                 .Include(x => x.DatDichVu).Include(x => x.DatDichVu.Select(y => y.DichVu));

                string[] strTu, strDen;
                DateTime? tu = null, den = null;

                if (!string.IsNullOrWhiteSpace(tuNgay) && tuNgay != "undefined" && tuNgay != "null")
                {
                    strTu = tuNgay.Split(' ');
                    tu = DateTime.Parse(strTu[0]);
                    if (tu.HasValue)
                        results = results.Where(o => o.NgayThanhToan >= tu);
                }

                if (!string.IsNullOrWhiteSpace(denNgay) && denNgay != "undefined" && denNgay != "null")
                {
                    strDen = denNgay.Split(' ');
                    den = DateTime.Parse(strDen[0]);
                    if (den.HasValue)
                        results = results.Where(o => o.NgayThanhToan <= den);
                }
                
                results = results.OrderBy(o => o.NgayThanhToan);

                ExportFileHoaDon(results.ToList(),"BaoCaoDoanhThu", sWebRootFolder, tu, den);
                string fPath = sWebRootFolder + "\\" + "BaoCaoDoanhThu.xlsx";
                System.IO.FileInfo fi = new System.IO.FileInfo(fPath);
                return File(fPath, System.Net.Mime.MediaTypeNames.Application.Octet, "BaoCaoDoanhThu" + fi.Extension);
            }
        }
       
        public static void ExportFileHoaDon(List<HoaDon> lst, string FName, string sWebRootFolder, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            string sFileName = FName + @".xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }

            string[] header = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            using (OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage(file))
            {
                OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Báo cáo doanh thu");
                int dem = 1;
                //worksheet.View.FreezePanes(2, 1);
                foreach (string s in header)
                {
                    worksheet.Cells[7, dem].Value = header[dem - 1];
                    dem++;
                }
                worksheet.Cells["A1:D1"].Merge = true;
                worksheet.Cells["A1:D1"].Value = "Quản lý khách sạn";
                worksheet.Cells["A1:D1"].Style.Font.Italic = true;

                worksheet.Cells["A2:D2"].Merge = true;
                worksheet.Cells["A2:D2"].Value = "CÔNG TY CP BẤT ĐỘNG SẢN";

                worksheet.Cells["A4:L4"].Merge = true;
                worksheet.Cells["A4:L4"].Value = "Từ ngày " + tuNgay?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " đến ngày " + denNgay?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                worksheet.Cells["A3:L3"].Merge = true;
                worksheet.Cells["A3:L3"].Value = "BÁO CÁO DOANH THU ĐỊNH KỲ";

               
                worksheet.Cells[6, 1].Value = "STT";
                worksheet.Cells[6, 2].Value = "Mã hóa đơn";
                worksheet.Cells[6, 3].Value = "Tên khách hàng";
                worksheet.Cells[6, 4].Value = "Số điện thoại";
                worksheet.Cells[6, 5].Value = "Ngày nhận phòng";
                worksheet.Cells[6, 6].Value = "Ngày trả phòng";
                worksheet.Cells[6, 7].Value = "Tiền phòng";
                worksheet.Cells[6, 8].Value = "Tiền dịch vụ";
                worksheet.Cells[6, 9].Value = "Phụ thu";
                worksheet.Cells[6, 10].Value = "Tổng thanh toán";
                worksheet.Cells[6, 11].Value = "Thu ngân";
                worksheet.Cells[6, 12].Value = "Ghi chú";

                using (var range = worksheet.Cells["A3:L5"])
                {
                    range.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                using (var range = worksheet.Cells["A6:L7"])
                {
                    // Set Border
                    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.AutoFitColumns();
                }

                // add a new worksheet to the empty workbook
                int i = 0;
                double? tongTienPhong = 0;
                double? tongTienDichVu = 0;
                double? tongTienPhuThu = 0; 
                double? tongTien = 0;
                for (i = 0; i < lst.Count ; i++)
                {
                    for (int j = 1; j <= 12; j++)
                    {
                        worksheet.Cells[i + 8, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[i + 8, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[i + 8, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[i + 8, j].AutoFitColumns();
                    }

                    worksheet.Cells[i + 8, 1].Value = i + 1;
                    worksheet.Cells[i + 8, 2].Value = lst[i].MaHoaDon + " - " + lst[i].NgayThanhToan.ToString();
                    worksheet.Cells[i + 8, 3].Value = lst[i].KhachHang != null ? lst[i].KhachHang.HoTen : "";
                    worksheet.Cells[i + 8, 4].Value = lst[i].KhachHang != null ? lst[i].KhachHang.SoDienThoai : "";
                    worksheet.Cells[i + 8, 5].Value = lst[i].ThoiGianNhanPhong != null ? lst[i].ThoiGianNhanPhong.ToString() : "";
                    worksheet.Cells[i + 8, 6].Value = lst[i].ThoiGianTraPhong != null ? lst[i].ThoiGianTraPhong.ToString() : "";
                    worksheet.Cells[i + 8, 7].Value = lst[i].ThuePhong != null? lst[i].ThuePhong.Sum(x => x.BangGia.GiaPhong) : 0;
                    worksheet.Cells[i + 8, 7].Style.Numberformat.Format = "###,###,###";
                    worksheet.Cells[i + 8, 8].Value = lst[i].DatDichVu !=null ? lst[i].DatDichVu.Sum(x => x.SoLuong*x.DichVu.GiaBan) : 0;
                    worksheet.Cells[i + 8, 8].Style.Numberformat.Format = "###,###,###";
                    worksheet.Cells[i + 8, 9].Value = lst[i].PhuThu;
                    worksheet.Cells[i + 8, 9].Style.Numberformat.Format = "###,###,###";
                    worksheet.Cells[i + 8, 10].Value = (lst[i].ThuePhong != null ? lst[i].ThuePhong.Sum(x => x.BangGia.GiaPhong) : 0)
                                                      + (lst[i].DatDichVu != null ? lst[i].DatDichVu.Sum(x => x.SoLuong * x.DichVu.GiaBan) : 0)
                                                      + lst[i].PhuThu;
                    worksheet.Cells[i + 8, 10].Style.Numberformat.Format = "###,###,###";
                    worksheet.Cells[i + 8, 11].Value = lst[i].NhanVien != null ? lst[i].NhanVien.TenNhanVien : "";
                    worksheet.Cells[i + 8, 12].Value = "";
                    tongTienPhong += lst[i].ThuePhong != null ? lst[i].ThuePhong.Sum(x => x.BangGia.GiaPhong) : 0;
                    tongTienDichVu += lst[i].DatDichVu != null ? lst[i].DatDichVu.Sum(x => x.SoLuong * x.DichVu.GiaBan) : 0;
                    tongTienPhuThu += lst[i].PhuThu;
                    tongTien += tongTienPhong + tongTienDichVu + tongTienPhuThu;

                }

                worksheet.Cells[i + 8, 1, i + 8, 6].Merge = true;
                worksheet.Cells[i + 8, 1, i + 8, 6].Value = "TỔNG";
                worksheet.Cells[i + 8, 7].Value = tongTienPhong;
                worksheet.Cells[i + 8, 8].Value = tongTienDichVu;
                worksheet.Cells[i + 8, 9].Value = tongTienPhuThu;
                worksheet.Cells[i + 8, 10].Value = tongTien;
                worksheet.Cells[i + 8, 7].Style.Numberformat.Format = "###,###,###";
                worksheet.Cells[i + 8, 8].Style.Numberformat.Format = "###,###,###";
                worksheet.Cells[i + 8, 9].Style.Numberformat.Format = "###,###,###";
                worksheet.Cells[i + 8, 10].Style.Numberformat.Format = "###,###,###";

                using (var range = worksheet.Cells[i + 8, 1, i + 8, 12])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.AutoFitColumns();
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                worksheet.Cells[i + 10, 4].Value = "NGƯỜI LẬP";
                worksheet.Cells[i + 10, 9, i + 10, 10].Merge = true;
                worksheet.Cells[i + 10, 9, i + 10, 10].Value = "PHỤ TRÁCH BÁO CÁO";

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                worksheet.Cells[worksheet.Dimension.Address].Style.WrapText = true;
                package.Save();
            }
        }

    }
}