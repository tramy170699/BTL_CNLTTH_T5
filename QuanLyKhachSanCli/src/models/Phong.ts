import { ThuePhong } from "@/models/ThuePhong";
import { VatDungPhong } from "@/models/VatDungPhong";
import { LoaiPhong } from "@/models/LoaiPhong";

export interface Phong {
    PhongID: number;
    SoPhong: number;
    TrangThai: boolean;
    LoaiPhongID: number;
    LoaiPhong?: LoaiPhong;
    ThuePhong?: ThuePhong[];
    VatDungPhong?: VatDungPhong[];
}
