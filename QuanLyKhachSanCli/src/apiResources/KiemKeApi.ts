import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { KiemKe } from '@/models/KiemKe'
export interface KiemKeApiSearchParams extends Pagination {
    nhanVienID?: number;
    tuNgay?: Date;
    denNgay?: Date;
    maKiemKe?: string;
}
class KiemKeApi extends BaseApi {
    search(searchParams: KiemKeApiSearchParams): Promise<PaginatedResponse<KiemKe>> {

        return new Promise<PaginatedResponse<KiemKe>>((resolve: any, reject: any) => {
            HTTP.get('kiemke', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<KiemKe> {
        return new Promise<KiemKe>((resolve: any, reject: any) => {
            HTTP.get('kiemke/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, kiemKe: KiemKe): Promise<KiemKe> {
        return new Promise<KiemKe>((resolve: any, reject: any) => {
            HTTP.put('kiemke/' + id, 
                kiemKe
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(kiemKe: KiemKe): Promise<KiemKe> {
        return new Promise<KiemKe>((resolve: any, reject: any) => {
            HTTP.post('kiemke', 
                kiemKe
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<KiemKe> {
        return new Promise<KiemKe>((resolve: any, reject: any) => {
            HTTP.delete('kiemke/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new KiemKeApi();
