import { DichVu } from "@/models/DichVu";
import { HoaDon } from "@/models/HoaDon";

export interface DatDichVu {
    DatDichVuID: number;
    SoLuong: number;
    DichVuID: number;
    HoaDonID: number;
    DichVu?: DichVu;
    HoaDon?: HoaDon;
}
