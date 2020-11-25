import { DatPhong } from "@/models/DatPhong";
import { HoaDon } from "@/models/HoaDon";
import { KiemKe } from "@/models/KiemKe";
import { PhieuNhapKho } from "@/models/PhieuNhapKho";
import { BoPhan } from "@/models/BoPhan";
import { Users } from "./Users";

export interface NhanVien {
    NhanVienID: number;
    TenNhanVien: string;
    SoDienThoai: string;
    BoPhanID: number;
    TenDangNhap: string;
    MatKhau: string;
    LoaiTaiKhoanID: number;
    BoPhan?: BoPhan;
    Users?: Users;
    DatPhong?: DatPhong[];
    HoaDon?: HoaDon[];
    KiemKe?: KiemKe[];
    PhieuNhapKho?: PhieuNhapKho[];
}
