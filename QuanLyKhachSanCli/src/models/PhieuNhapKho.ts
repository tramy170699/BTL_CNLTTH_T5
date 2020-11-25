import { ChiTietPhieuNhap } from "@/models/ChiTietPhieuNhap";
import { NhanVien } from "@/models/NhanVien";

export interface PhieuNhapKho {
    PhieuNhapID: number;
    NgayNhap: Date;
    TongTien: number;
    NhanVienID: number;
    NhanVien?: NhanVien;
    ChiTietPhieuNhap?: ChiTietPhieuNhap[];
}
