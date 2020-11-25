import { ThuePhong } from "@/models/ThuePhong";
import { LoaiPhong } from "@/models/LoaiPhong";

export interface BangGia {
    BangGiaID: number;
    LoaiPhongID: number;
    GiaPhong: number;
    ApDungTuNgay: Date;
    ApDungDenNgay: Date;
    LoaiPhong?: LoaiPhong;
    ThuePhong?: ThuePhong[];
}
