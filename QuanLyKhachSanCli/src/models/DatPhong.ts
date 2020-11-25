import { HoaDon } from "@/models/HoaDon";
import { LoaiPhong } from "@/models/LoaiPhong";
import { NhanVien } from "@/models/NhanVien";
import { DichVu } from "./DichVu";

export interface DatPhong {
    DatPhongID: number;
    NhanVienID: number;
    LoaiPhongID: number;
    HoTen: string;
    SoDienThoai: string;
    ThoiGianDat: Date;
    SoLuongNguoi: number;
    SoNgayDat: number;
    TienCoc: number;
    NgayTao: Date;
    TrangThai: number;
    LoaiPhong?: LoaiPhong;
    DichVu?: DichVu;
    NhanVien?: NhanVien;
    HoaDon?: HoaDon[];
}
