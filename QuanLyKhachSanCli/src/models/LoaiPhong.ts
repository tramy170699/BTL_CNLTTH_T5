import { BangGia } from "@/models/BangGia";
import { DatPhong } from "@/models/DatPhong";
import { Phong } from "@/models/Phong";

export interface LoaiPhong {
    LoaiPhongID: number;
    TenLoaiPhong: string;
    GiaPhong: number;
    MoTa: string;
    BangGia?: BangGia[];
    DatPhong?: DatPhong[];
    Phong?: Phong[];
}
