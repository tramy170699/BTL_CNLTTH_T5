import { HoaDon } from "@/models/HoaDon";

export interface KhachHang {
    KhachHangID: number;
    HoTen: string;
    GioiTinh: boolean;
    NgaySinh: Date;
    SoDienThoai: string;
    DiaChi: string;
    CMND: string;
    HoaDon?: HoaDon[];
}
