import { ChiTietKiemKe } from "@/models/ChiTietKiemKe";
import { Phong } from "@/models/Phong";
import { VatDung } from "@/models/VatDung";

export interface VatDungPhong {
    VatDungPhongID: number;
    SoLuong: number;
    GhiChu: string;
    VatDungID: number;
    PhongID: number;
    Phong?: Phong;
    VatDung?: VatDung;
    ChiTietKiemKe?: ChiTietKiemKe[];
}
