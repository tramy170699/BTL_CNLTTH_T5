import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { KhachHang } from '@/models/KhachHang'
export interface KhachHangApiSearchParams extends Pagination {
    query?: string;
}
class KhachHangApi extends BaseApi {
    search(searchParams: KhachHangApiSearchParams): Promise<PaginatedResponse<KhachHang>> {

        return new Promise<PaginatedResponse<KhachHang>>((resolve: any, reject: any) => {
            HTTP.get('khachhang', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    kiemtra(cmnd: string): Promise<KhachHang> {
        return new Promise<KhachHang>((resolve: any, reject: any) => {
            HTTP.get('khachhang/kiemtrathongtin?cmnd=' + cmnd).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<KhachHang> {
        return new Promise<KhachHang>((resolve: any, reject: any) => {
            HTTP.get('khachhang/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, khachHang: KhachHang): Promise<KhachHang> {
        return new Promise<KhachHang>((resolve: any, reject: any) => {
            HTTP.put('khachhang/' + id, 
                khachHang
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(khachHang: KhachHang): Promise<KhachHang> {
        return new Promise<KhachHang>((resolve: any, reject: any) => {
            HTTP.post('khachhang', 
                khachHang
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<KhachHang> {
        return new Promise<KhachHang>((resolve: any, reject: any) => {
            HTTP.delete('khachhang/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new KhachHangApi();
