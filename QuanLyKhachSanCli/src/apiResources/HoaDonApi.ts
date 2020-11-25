import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { HoaDon } from '@/models/HoaDon'
export interface HoaDonApiSearchParams extends Pagination {
    nhanVienID?:number;
    datPhongID?:number;
    khachHangID?: number;
    query?: string;
    trangThai?: boolean;
    tuNgay?: Date;
    denNgay?: Date;
}
class HoaDonApi extends BaseApi {
    search(searchParams: HoaDonApiSearchParams): Promise<PaginatedResponse<HoaDon>> {

        return new Promise<PaginatedResponse<HoaDon>>((resolve: any, reject: any) => {
            HTTP.get('hoadon', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<HoaDon> {
        return new Promise<HoaDon>((resolve: any, reject: any) => {
            HTTP.get('hoadon/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, hoaDon: HoaDon): Promise<HoaDon> {
        return new Promise<HoaDon>((resolve: any, reject: any) => {
            HTTP.put('hoadon/' + id, 
                hoaDon
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(hoaDon: HoaDon): Promise<HoaDon> {
        return new Promise<HoaDon>((resolve: any, reject: any) => {
            HTTP.post('hoadon', 
                hoaDon
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<HoaDon> {
        return new Promise<HoaDon>((resolve: any, reject: any) => {
            HTTP.delete('hoadon/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new HoaDonApi();
