import { DatDichVu } from "@/models/DatDichVu";

export interface DichVu {
    DichVuID: number;
    TenDichVu: string;
    MoTa: string;
    GiaBan: number;
    DonViTinh: string;
    TrangThai: boolean;
    DatDichVu?: DatDichVu[];
}
