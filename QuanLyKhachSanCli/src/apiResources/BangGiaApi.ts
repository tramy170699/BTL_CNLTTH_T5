import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { BangGia } from '@/models/BangGia'
export interface BangGiaApiSearchParams extends Pagination {
    loaiPhongID?:number;
}
class BangGiaApi extends BaseApi {
    search(searchParams: BangGiaApiSearchParams): Promise<PaginatedResponse<BangGia>> {

        return new Promise<PaginatedResponse<BangGia>>((resolve: any, reject: any) => {
            HTTP.get('banggia', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    giaapdung(ngayNhanPhong: Date, loaiPhongID: number): Promise<BangGia> {
        return new Promise<BangGia>((resolve: any, reject: any) => {
            HTTP.get('banggia/giaapdung?ngayNhanPhong=' + ngayNhanPhong
                + '&loaiPhongID=' + loaiPhongID
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<BangGia> {
        return new Promise<BangGia>((resolve: any, reject: any) => {
            HTTP.get('banggia/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, bangGia: BangGia): Promise<BangGia> {
        return new Promise<BangGia>((resolve: any, reject: any) => {
            HTTP.put('banggia/' + id, 
                bangGia
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(bangGia: BangGia): Promise<BangGia> {
        return new Promise<BangGia>((resolve: any, reject: any) => {
            HTTP.post('banggia', 
                bangGia
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<BangGia> {
        return new Promise<BangGia>((resolve: any, reject: any) => {
            HTTP.delete('banggia/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new BangGiaApi();
