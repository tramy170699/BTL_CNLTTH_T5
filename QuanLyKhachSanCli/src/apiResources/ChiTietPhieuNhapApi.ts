import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { ChiTietPhieuNhap } from '@/models/ChiTietPhieuNhap'
export interface ChiTietPhieuNhapApiSearchParams extends Pagination {
    phieuNhapID?:number;
    thuocTinhID?:number;
}
class ChiTietPhieuNhapApi extends BaseApi {
    search(searchParams: ChiTietPhieuNhapApiSearchParams): Promise<PaginatedResponse<ChiTietPhieuNhap>> {

        return new Promise<PaginatedResponse<ChiTietPhieuNhap>>((resolve: any, reject: any) => {
            HTTP.get('chitietphieunhap', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<ChiTietPhieuNhap> {
        return new Promise<ChiTietPhieuNhap>((resolve: any, reject: any) => {
            HTTP.get('chitietphieunhap/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, chiTietPhieuNhap: ChiTietPhieuNhap): Promise<ChiTietPhieuNhap> {
        return new Promise<ChiTietPhieuNhap>((resolve: any, reject: any) => {
            HTTP.put('chitietphieunhap/' + id, 
                chiTietPhieuNhap
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(chiTietPhieuNhap: ChiTietPhieuNhap): Promise<ChiTietPhieuNhap> {
        return new Promise<ChiTietPhieuNhap>((resolve: any, reject: any) => {
            HTTP.post('chitietphieunhap', 
                chiTietPhieuNhap
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<ChiTietPhieuNhap> {
        return new Promise<ChiTietPhieuNhap>((resolve: any, reject: any) => {
            HTTP.delete('chitietphieunhap/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new ChiTietPhieuNhapApi();
