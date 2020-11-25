import { PhieuNhapKho } from "@/models/PhieuNhapKho";
import { VatDung } from "@/models/VatDung";

export interface ChiTietPhieuNhap {
    ChiTietPhieuNhapID: number;
    PhieuNhapID: number;
    ThuocTinhID: number;
    SoLuong: number;
    GiaNhap: number;
    GhiChu: string;
    PhieuNhapKho?: PhieuNhapKho;
    VatDung?: VatDung;
}
