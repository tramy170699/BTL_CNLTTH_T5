import { DatDichVu } from "@/models/DatDichVu";
import { ThuePhong } from "@/models/ThuePhong";
import { DatPhong } from "@/models/DatPhong";
import { KhachHang } from "@/models/KhachHang";
import { NhanVien } from "@/models/NhanVien";

export interface HoaDon {
    HoaDonID: number;
    NhanVienID: number;
    DatPhongID: number;
    KhachHangID: number;
    MaHoaDon: string;
    NgayThanhToan: Date;
    SoTaiKhoan: string;
    ThoiGianNhanPhong: Date;
    ThoiGianTraPhong: Date;
    PhuThu: number;
    LyDo: string;
    DatPhong?: DatPhong;
    KhachHang?: KhachHang;
    NhanVien?: NhanVien;
    DatDichVu?: DatDichVu[];
    ThuePhong?: ThuePhong[];
}
