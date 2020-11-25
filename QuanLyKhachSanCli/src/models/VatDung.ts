import { ChiTietPhieuNhap } from "@/models/ChiTietPhieuNhap";
import { VatDungPhong } from "@/models/VatDungPhong";

export interface VatDung {
    VatDungID: number;
    TenVatDung: string;
    SoLuongKho?: number;
    MoTa: string;
    ChiTietPhieuNhap?: ChiTietPhieuNhap[];
    VatDungPhong?: VatDungPhong[];
}
