import { NhanVien } from "@/models/NhanVien";

export interface BoPhan {
    BoPhanID: number;
    TenBoPhan: string;
    NhiemVu: string;
    GhiChu: string;
    NhanVien?: NhanVien[];
}
