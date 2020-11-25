import { ChiTietKiemKe } from "@/models/ChiTietKiemKe";
import { NhanVien } from "@/models/NhanVien";

export interface KiemKe {
    KiemKeID: number;
    NgayKiemKe: Date;
    NhanVienID: number;
    GhiChu: string;
    NhanVien?: NhanVien;
    ChiTietKiemKe?: ChiTietKiemKe[];
}
