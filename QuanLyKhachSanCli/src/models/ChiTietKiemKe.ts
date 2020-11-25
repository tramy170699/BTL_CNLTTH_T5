import { KiemKe } from "@/models/KiemKe";
import { VatDungPhong } from "@/models/VatDungPhong";

export interface ChiTietKiemKe {
    ChiTietKiemKeID: number;
    KiemKeID: number;
    VatDungPhongID: number;
    SoLuongKiemKe: number;
    GhiChu: string;
    KiemKe?: KiemKe;
    VatDungPhong?: VatDungPhong;
}
